using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using UnityEngine.VFX;
using UnityEngine.Rendering;
using UnityEngine.XR;
using UnityEngine.VFX.Utility;

public class Ability_Teleport : Ability_Template
{
    [Header("Debug")]
    [SerializeField]
    bool debugActive = false;
    [SerializeField]
    bool debugDoTeleport = false;


    public GameObject teleportMarkerTop;
    public GameObject teleportMarkerBottom;
    public VisualEffect teleportMarkerVFX;
    public VisualEffect teleportHandVFX;
    public LayerMask teleportFilter;

    Volume abilityPP;

    Vector3 hitPos, hitPosFinal;
    Vector3 lastTopPos, lastBottomPos;

    int horDist = 5;
    int vertDist = 2;

    float fadeTime = 0;
    float fadeDuration = 0.5f;

    bool clearSpot = false;

    float useDelay = 1f;
    float nextUseDelay = 0;

    ExposedProperty scatterDeltaProp, teleportActiveProp;

    float scatterDelta = 0f;

    protected override void OnStart()
    {
        abilityPP = GetComponentInChildren<Volume>();

        scatterDeltaProp = "Scatter Delta";
        teleportActiveProp = "Teleport Active";
    }

    
    protected override void OnUpdate()
    {
        if (!IsAttached())
            return;

        if (handObject == null)
            return;

        CheckGesture();
        UpdatePostProcess();
        CheckHandVFX();
    }

    protected override void OnLateUpdate()
    {
        if (!IsAttached())
            return;

        if (handObject == null)
            return;

        CheckTeleport();
    }

    void FixedUpdate()
    {

        teleportMarkerTop.transform.rotation = Quaternion.identity;

        if (!IsAttached())
            return;

        if (handObject == null)
            return;

        if (!IsActive())
        {
            //lil' workaround
            teleportMarkerBottom.transform.position = lastBottomPos;
            teleportMarkerTop.transform.position = lastTopPos;
            return;
        }
            

        RaycastHit hitInfoMain, hitInfoVertical;

        //horizontal trace
        bool hit = Physics.Raycast(handObject.transform.position, handObject.transform.forward, out hitInfoMain, horDist, teleportFilter);

        if (hit)
            hitPos = hitInfoMain.point + hitInfoMain.normal * (playerController.radius + 0.05f);
        else
            hitPos = handObject.transform.position + handObject.transform.forward * horDist;

        //vertical hull
        hit = Physics.Raycast(hitPos, Vector3.up * -1 , out hitInfoVertical, vertDist, teleportFilter);

        if (hit)
            hitPosFinal = hitInfoVertical.point + hitInfoVertical.normal * (playerController.radius + 0.05f);
        else
            hitPosFinal = hitPos - Vector3.up * vertDist;

        // hull check

        Vector3 trStart = hitPosFinal + playerController.center + Vector3.up * -playerController.height * 0.5F;
        Vector3 trEnd = trStart + Vector3.up * playerController.height;

        clearSpot = false;

        for (int i = 0; i < 3; i++)
        {
            Vector3 offset = handObject.transform.forward * -1 * i * playerController.radius;
            hit = Physics.CheckCapsule(trStart + offset, trEnd + offset, playerController.radius, teleportFilter);

            if (!hit)
            {
                hitPosFinal += offset;
                clearSpot = true;
                break;
            }
        }

        //ground marker
        teleportMarkerBottom.transform.position = hitPosFinal - new Vector3(0, playerController.radius, 0);
        lastBottomPos = teleportMarkerBottom.transform.position;

        float effectThreshold = 0.3f;

        //top marker
        Vector3 clampedHeight = new Vector3(0, 0, 0);
        if ((hitPos - hitPosFinal).y < vertDist * effectThreshold)
            clampedHeight.y = Mathf.Clamp(vertDist * effectThreshold - (hitPos - hitPosFinal).y, 0, vertDist * effectThreshold);


        teleportMarkerTop.transform.position = hitPos + clampedHeight;
        lastTopPos = teleportMarkerTop.transform.position;
    }

    protected override void CheckGesture()
    {
        // pointing gesture, hold the grip and make sure that hand is somewhat horizontal
        if (inp.LeftGripDelta() >= 0.2 && Vector3.Dot(handObject.transform.right * -1, Vector3.up) > 0.45 || debugActive)
        {
            TryActivateAbility();
        }
        else
            DeactivateThisAbility();
    }

    private void CheckTeleport()
    {
        if (!IsActive())
            return;

        float t = Time.time;

        if (nextUseDelay < t && (inp.GetInputTriggerPress().GetLastStateDown(SteamVR_Input_Sources.LeftHand) || debugDoTeleport) && clearSpot)
        {
            DoTeleport();
            //DeactivateThisAbility();
            nextUseDelay = t + useDelay;
        }
        
    }

    private void UpdatePostProcess()
    {
        if (abilityPP == null)
            return;

        if ( fadeTime >= Time.time )
        {
            float delta = Mathf.Clamp01((fadeTime - Time.time) / fadeDuration);
            abilityPP.weight = delta;
        }
        else
            abilityPP.weight = 0;
    }

    private void CheckHandVFX()
    {
        scatterDelta = Mathf.Lerp(scatterDelta, IsActive() ? 1 : 0, Time.deltaTime * (IsActive() ? 4 : 8));

        // fix non-zero lerp just in case
        if (scatterDelta <= 0.001 && !IsActive())
            scatterDelta = 0;

        teleportHandVFX?.SetFloat(scatterDeltaProp, scatterDelta);
    }

    private void DoTeleport()
    {
        if (XRSettings.enabled)
            hapticAction.Execute(0, 0.6f, 10, 50, SteamVR_Input_Sources.LeftHand);

        //anim.DoTriggerInteraction(false);

        animController.SetTrigger("Do Teleport");

        if (debugDoTeleport) 
            debugDoTeleport = false;

        playerController.enabled = false;
        playerController.transform.position = hitPosFinal;
        playerMovement.ResetCameraMove();
        playerController.enabled = true;

        fadeTime = Time.time + fadeDuration;
    }

    protected override void OnActivate()
    {
        if (teleportMarkerVFX != null)
            teleportMarkerVFX.Play();

        animController?.SetBool("Active", true);
        teleportHandVFX?.SetBool("Teleport Active", true);
    }

    protected override void OnDeactivate()
    {
        if (teleportMarkerVFX != null)
            teleportMarkerVFX.Stop();

        animController?.SetBool("Active", false);
        teleportHandVFX?.SetBool("Teleport Active", false);
    }

    protected override void OnAttach(HandAbilities parent)
    {
       
    }

    protected override void OnDetach()
    {
       
    }

    protected override float AnimationBlendOutRate() => 1f;
}

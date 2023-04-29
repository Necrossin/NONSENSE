using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
using UnityEngine.VFX.Utility;
using Valve.VR;

public class Ability_Grenade : Ability_Template
{
    [SerializeField]
    GameObject ProjectilePrefab;
    GameObject ProjectileObject { get; set; } = null;
    ProjectileGrenade ProjectileScript { get; set; } = null;

    VisualEffect handVFX;

    Camera ownerCamera;

    float useDelay = 1f;
    float nextUseDelay = 0;

    ExposedProperty highlightProp;

    void Start()
    {
        abilityHoldtype = 2;

        highlightProp = "Highlight";
    }

    protected override void OnAttach(HandAbilities parent)
    {
        handVFX = parent.GetHandVFX();
    }

    protected override void OnDetach()
    {
        handVFX = null;
    }

    void Update()
    {
        CheckCamera();

        if (!IsAttached())
            return;

        if (handObject == null)
            return;

        CheckGesture();
    }

    private void LateUpdate()
    {
        if (!IsAttached())
            return;

        if (handObject == null)
            return;

        CheckPriming();
    }

    protected override void CheckGesture()
    {
        // old man yells at the cloud type of gesture, semi-clenched fist, palm towards the player

        Vector3 handDir = (handObject.transform.position - ownerCamera.transform.position).normalized;//handObject.transform.position - playerController.transform.position + playerController.center;

        //Debug.DrawLine(handObject.transform.position, playerController.transform.position + playerController.center, Color.white, 1);

        // check for grenade first
        bool validGesture = inp.LeftGripDelta() >= 0.2 && Vector3.Dot(handObject.transform.right * -1, handDir) > 0.55;
        bool throwGesture = inp.LeftGripDelta() < 0.2;

        // TODO: priming
        if (IsGrenadeValid() && ProjectileScript.isPrimed)
        {
            // throw logic here
            if (throwGesture)
                ThrowGrenade();
            return;
        }

        if (validGesture)
        {
            TryActivateAbility();
        }
        else
            DeactivateThisAbility();
    }

    protected override void OnActivate()
    {
        velEstimator.BeginEstimatingVelocity();
        SpawnGrenade();
        ProjectileScript.OnSpawn();

        handVFX?.SetBool(highlightProp, true);
    }

    protected override void OnDeactivate()
    {
        velEstimator.FinishEstimatingVelocity();
        DisableGrenade();
        handVFX?.SetBool(highlightProp, false);
    }

    // spawn or reenable grenade if we have one (should probably add these to pooling when that's gonna be a thing)
    void SpawnGrenade()
    {
        if (ProjectileObject == null)
        {
            ProjectileObject = Instantiate(ProjectilePrefab, abilityManager.palmAttachment);
            ProjectileScript = ProjectileObject.GetComponent<ProjectileGrenade>();
            ProjectileObject.GetComponent<Rigidbody>().isKinematic = true;
            return;
        }

        if (!ProjectileObject.activeSelf)
            ProjectileObject.SetActive(true);

    }

    void DisableGrenade()
    {
        if (ProjectileObject == null)
            return;

        if (ProjectileObject.activeSelf)
            ProjectileObject.SetActive(false);
    }

    void ThrowGrenade()
    {
        if (ProjectileObject == null)
            return;

        velEstimator.FinishEstimatingVelocity();

        ProjectileObject.transform.SetParent(null);

        var rb = ProjectileObject.GetComponent<Rigidbody>();
        rb.isKinematic = false;

        rb.AddForce(velEstimator.GetVelocityEstimate(), ForceMode.Impulse);
        rb.AddTorque(velEstimator.GetAngularVelocityEstimate(), ForceMode.Impulse);

        ProjectileScript.OnThrow();

        ProjectileObject = null;
        ProjectileScript = null;

        DeactivateThisAbility();
    }

    bool IsGrenadeValid() => ProjectileObject != null && ProjectileObject.activeSelf && ProjectileScript != null;

    private void CheckCamera()
    {
        if (playerObject != null)
        {
            if (ownerCamera == null)
                ownerCamera = playerObject.GetComponentInChildren<Camera>();
        }
        else
        {
            if (ownerCamera != null)
                ownerCamera = null;
        }

    }

    private void CheckPriming()
    {
        if (!IsActive())
            return;

        float t = Time.time;

        if (nextUseDelay < t && inp.GetInputTriggerPress().GetLastStateDown(SteamVR_Input_Sources.LeftHand) && IsGrenadeValid() && !ProjectileScript.isPrimed)
        {
            ProjectileScript.Prime();
            hapticAction.Execute(0, 0.35f, 10, 50, SteamVR_Input_Sources.LeftHand);

            anim.DoTriggerInteraction(false);
            nextUseDelay = t + useDelay;
        }

    }
}

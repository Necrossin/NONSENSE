using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.VFX;
using UnityEngine.VFX.Utility;
using Valve.VR;

public class BaseMeleeWeapon : BaseInteractable
{

    [Header("Hit mask")]
    [SerializeField]
    private LayerMask playerFilter;

    [Header("Audio")]
    [SerializeField]
    protected AudioSource sndSource;
    [SerializeField]
    protected WeaponSoundData sndData;

    [Header("VFX")]
    [SerializeField]
    protected GameObject scrapeVfxPrefab;

    [Header("Haptics")]
    [SerializeField]
    protected SteamVR_Action_Vibration hapticAction;

    ExposedProperty LookAtProp = "LookAt";
    ExposedProperty DrawPosProp = "DrawPos";

    private float nextCollisionCheck = 0f;
    private Collider lastActiveCollider;
    protected VisualEffect scrapeVfx;

    protected RaycastHit hitInfo;

    protected new void Start()
    {
        base.Start();


    }

    
    void Update()
    {
 
    }

    // We need to know which collider starts the collision, but Stay and OnExit do not matter much, so we only check the initial hit
    public void OnEdgeTrigger( Collider thisCollider, Collider other, Vector3 hitPos, Vector3 hitNormal)
    {

        if (GetHandObject() == null) return;

        GameObject hitObject = other.gameObject;

        if (hitObject == null) return;
        if (lastActiveCollider != null) return;

        lastActiveCollider = thisCollider;

        if (nextCollisionCheck >= Time.time) return;

        nextCollisionCheck = Time.time + 0.01f;

        SurfaceMaterial surface = hitObject.GetComponent<SurfaceMaterial>();

        if (hitNormal.sqrMagnitude <= 0)
            hitNormal = (this.transform.position - hitPos).normalized;

        //Debug.DrawLine(hitPos, hitPos + hitNormal * 0.1f, Color.white, 5);

        if (surface != null)
        {
            //RaycastHit hitInfo;

            //Physics.Raycast(thisCollider.transform.position, hitNormal * -1, out hitInfo, 10, playerFilter);

            //surface.PlaceDecal(hitInfo, hitNormal * -1, true);
            //surface.PlaceDecal(hitPos, hitNormal, true);
        }

        GameObject scrapeObject = Pool.Instance.InstantiateFromPool(scrapeVfxPrefab, hitPos, Quaternion.identity);
        
        if (scrapeObject == null) return;

        scrapeVfx?.Stop();

        //scrapeVfx?.Reinit();

        scrapeVfx = scrapeObject.GetComponent<VisualEffect>();

        bool hit = Physics.Raycast(thisCollider.transform.position, hitNormal * -1, out hitInfo, 0.1f, playerFilter);
        //bool hit = Physics.Raycast(thisCollider.transform.position, (hitPos - this.transform.position).normalized, out hitInfo, 0.3f, playerFilter);

        if (hit)
        {
            scrapeVfx?.SetVector3(DrawPosProp, hitInfo.point + hitInfo.normal * 0.005f);
            scrapeVfx?.SetVector3(LookAtProp, hitInfo.point + hitInfo.normal * 0.1f);
        }
        else
        {
            scrapeVfx?.SetVector3(DrawPosProp, hitPos + hitNormal * 0.005f);
            scrapeVfx?.SetVector3(LookAtProp, hitPos + hitNormal * 0.1f);
        }

         scrapeVfx?.Play();
    }
    private void OnTriggerStay(Collider other)
    {

        if (lastActiveCollider == null) return;

        Vector3 hitPos = other.ClosestPointOnBounds(lastActiveCollider.transform.position);
        Vector3 hitNormal = (this.transform.position - hitPos).normalized;

        bool hit = Physics.Raycast(lastActiveCollider.transform.position, hitNormal * -1, out hitInfo, 0.1f, playerFilter);
        //bool hit = Physics.Raycast(lastActiveCollider.transform.position, (hitPos - this.transform.position).normalized, out hitInfo, 0.3f, playerFilter);

        if (hit)
        {
            scrapeVfx?.SetVector3(DrawPosProp, hitInfo.point + hitInfo.normal * 0.005f);
            scrapeVfx?.SetVector3(LookAtProp, hitInfo.point + hitInfo.normal * 0.1f);
        }
        else
        {
            scrapeVfx?.SetVector3(DrawPosProp, hitPos + hitNormal * 0.005f);
            scrapeVfx?.SetVector3(LookAtProp, hitPos + hitNormal * 0.1f);
        }

    }


    private void OnTriggerExit(Collider other)
    {
        if (lastActiveCollider == null) return;

        lastActiveCollider = null;

        scrapeVfx?.Stop();

    }
}

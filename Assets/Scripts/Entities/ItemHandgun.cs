using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class ItemHandgun : BaseRangedWeapon
{

    bool isSmgMode = false;
    [SerializeField]
    GameObject smgParts;

    //[SerializeField]
    Camera ownerCamera;

    [SerializeField]
    protected ParticleSystem muzzleSMGFx;
    [SerializeField]
    protected VisualEffect muzzleSMGVfx;

    [SerializeField]
    private WeaponSoundData sndDataSMG;

    static float handgunFireDelay = 0.3f;
    static float smgFireDelay = handgunFireDelay * 0.33f;

    static float handgunSpread = 0.01f;
    static float smgSpread = 2.5f;

    protected override void OnStart()
    {
        itemHoldtype = (int)Holdtype.Pistol;
    }
    protected override void OnUpdate()
    {
        //if (GetHandObject() != null)
        //Debug.Log(GetHandObject().GetComponentInParent<Camera>());

        CheckCamera();
        CheckRotation();

    }

    private void CheckCamera()
    {
        if (GetOwnerObject() != null)
        {
            if (ownerCamera == null)
                ownerCamera = GetOwnerObject().GetComponentInChildren<Camera>();
        }
        else
        {
            if (ownerCamera != null)
                ownerCamera = null;
        }

    }


    private void CheckRotation()
    {

        if (ownerCamera != null)
        {
            Vector3 armDir = (transform.position - ownerCamera.transform.position).normalized;
            Vector3 left = Vector3.Cross(armDir, ownerCamera.transform.up).normalized*-1;
            Vector3 leftGun = Vector3.Cross(armDir, transform.up).normalized*-1;

            /*Debug.DrawLine(ownerCamera.transform.position, transform.position, Color.red, 0.1f);
            Debug.DrawRay(transform.position, left * 5, Color.green);
            Debug.DrawRay(transform.position, leftGun * 5, Color.blue);*/

            float ang = Vector3.SignedAngle(left, leftGun, armDir);

            if ( !isSmgMode )
            {
                //if ( ( ang > 80 && ang <= 180 ) || ( ang >= -180 && ang < -70 ) )
                if (ang > 80)
                    SetSMGMode(true);   
            }
            else
            {
                if (ang < 10 && ang > -10)
                    SetSMGMode(false);
            }


            //Debug.Log(ang);
        }


    }

    void SetSMGMode( bool bl )
    {
        isSmgMode = bl;
        smgParts.SetActive(bl);
        auto = bl;

        triggerHold = false;

    }

    protected override float GetFireDelay()
    {
        return isSmgMode ? smgFireDelay : handgunFireDelay;
    }

    protected override float GetSpread()
    {
        return isSmgMode ? smgSpread : handgunSpread;
    }

    protected override void ShootEffects()
    {
        if (isSmgMode)
        {
            if (muzzleSMGFx != null)
                muzzleSMGFx.Play(true);

            if (muzzleSMGVfx != null)
                muzzleSMGVfx.Play();

            if (shellFx != null)
                shellFx.Emit(1);
        }
        else
            base.ShootEffects();        
    }

    protected override WeaponSoundData GetSoundData()
    {
        return isSmgMode ? sndDataSMG : sndData;
    }

    protected override float GetRecoil()
    {
        if (isSmgMode)
            return base.GetRecoil();

        return 3f;
    }
}

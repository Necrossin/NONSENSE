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

            float ang = Vector3.SignedAngle(left, leftGun, armDir);

            if ( !isSmgMode )
            {
                if (ang > 80)
                    SetSMGMode(true);   
            }
            else
            {
                if (ang < 10 && ang > -10)
                    SetSMGMode(false);
            }
        }

    }

    void SetSMGMode( bool bl )
    {
        isSmgMode = bl;
        smgParts.SetActive(bl);
        auto = bl;

        triggerHold = false;

    }

    protected override float GetFireDelay() => isSmgMode ? smgFireDelay : handgunFireDelay;

    protected override float GetSpread() => isSmgMode ? smgSpread : handgunSpread;

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

    protected override WeaponSoundData GetSoundData() => isSmgMode ? sndDataSMG : sndData;

    protected override float GetRecoil()
    {
        if (isSmgMode)
            return base.GetRecoil();

        return 3f;
    }
}

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

    private Animation ammoAnim;

    static float handgunFireDelay = 0.3f;
    static float smgFireDelay = handgunFireDelay * 0.33f;

    static float handgunSpread = 0.01f;
    static float smgSpread = 2.5f;

    private int smgCurClip = 30;
    private int smgMaxClip = 30;

    private int handgunCurClip = 10;
    private int handgunMaxClip = 10;

    private int ammoMul = 3;

    protected new void Start()
    {
        base.Start();

        itemHoldtype = (int)Holdtype.Pistol;

        MaxClip = 10;
        CurClip = MaxClip;

        handgunCurClip = 10;
        handgunMaxClip = handgunCurClip;

        smgMaxClip = (int)Mathf.Round(handgunCurClip * ammoMul);
        smgCurClip = smgMaxClip;

        MaxClip = handgunMaxClip;
        CurClip = MaxClip;

        ammoAnim = GetComponent<Animation>();

        //SetSMGMode(true);
    }

    protected override void OnStart()
    {
        /*itemHoldtype = (int)Holdtype.Pistol;

        MaxClip = 10;
        CurClip = MaxClip;*/
    }
    protected override void OnUpdate()
    {
        // enemies will have their own check for handgun/smg mode maybe
        if (IsHeldByEnemy())
            return;

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

    void SetSMGMode(bool bl)
    {
        isSmgMode = bl;
        smgParts.SetActive(bl);
        auto = bl;

        triggerHold = false;

        CurClip = bl ? smgCurClip : handgunCurClip;

        if (ammoCounter != null && ammoCounter.GetAmmoVFX() != null)
        {
            ammoCounter.GetAmmoVFX().Stop();
            ammoCounter.GetAmmoVFX().Play();
        }

        if (ammoAnim != null)
        {
            ammoAnim["HandgunToSMGAmmo"].speed = bl ? 1.3f : -1.3f;
            ammoAnim["HandgunToSMGAmmo"].normalizedTime = bl ? 0 : 1;
            ammoAnim.Play("HandgunToSMGAmmo");
        }
        

    }

    public override float GetFireDelay() => isSmgMode ? smgFireDelay : handgunFireDelay;

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

    protected override void TakeAmmo(int am)
    {
        if (isSmgMode)
        {
            smgCurClip = Mathf.Clamp(smgCurClip - am, 0, smgMaxClip);
            CurClip = smgCurClip;

            if (Mathf.Ceil(smgCurClip / ammoMul) < handgunCurClip && handgunCurClip > 0)
                handgunCurClip -= 1;
        }
        else
        {
            handgunCurClip = Mathf.Clamp(handgunCurClip - am, 0, handgunMaxClip);
            CurClip = handgunCurClip;

            if (smgCurClip >= ammoMul)
                smgCurClip -= ammoMul;
        }

        //Debug.Log("Handgun ammo " + handgunCurClip + "   SMG ammo " + smgCurClip);

        if (ammoCounter != null)
            ammoCounter.PlayFireEffects();
    }

    public override float IsAIInRange() => 35f;

    public override int GetMaxClip() => isSmgMode ? smgMaxClip : handgunMaxClip;
}

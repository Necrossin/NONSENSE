using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
using Valve.VR;

public class ItemShotgun : BaseRangedWeapon
{
    //[SerializeField]
    protected VisualEffect muzzleShotgunVfx;

    Animator ownerAnimator;
    VelocityEstimatorParent ownerVelocityEstimator;
    bool chambered = true;
    float nextChamber = 0f;

    [Header("Debug")]
    [SerializeField]
    private bool debugChambering = false;

    protected new void Start()
    {
        base.Start();

        itemHoldtype = (int)Holdtype.Shotgun;  //todo: remove this
        numShots = 6;
        spread = 5f;
        //gunAnimations[gunAnimations.clip.name].speed = 1.7f;

        MaxClip = 6;
        CurClip = MaxClip;

        animEventHandler?.OnLeverClickEvent.AddListener(DoLeverSound);
        animEventHandler?.OnShellEjectEvent.AddListener(DoShellFX);
        animEventHandler?.OnFullReloadEvent.AddListener(LoadChamber);

        if (muzzleFx != null)
            muzzleShotgunVfx = muzzleFx.gameObject.GetComponentInChildren<VisualEffect>();

    }

    protected override void OnUpdate()
    {
        //CheckAnimator();
        CheckVelocityEstimator();
        CheckChambering();
    }

    public override float GetFireDelay() => 1f;
    public override float GetSmokeDelay() => 2;

    protected override float GetSpread() => 3f;

    protected override void ShootEffects()
    {
        if (muzzleFx != null)
            muzzleFx.Play(true);

        if (muzzleShotgunVfx != null)
            muzzleShotgunVfx.Play();

        chambered = false;
        //StartChamberingAuto();
    }

    protected override void PlayShootAnimation()
    {
        animController?.SetBool("Shoot", true);
    }

    private void CheckAnimator()
    {
        if (GetHandObject() != null)
        {
            if (ownerAnimator == null)
                ownerAnimator = GetHandObject().GetComponent<Animator>();
        }
        else
        {
            if (ownerAnimator != null)
                ownerAnimator = null;
        }
    }

    // grab player's local velocity estimator so we can handle reload gesture
    private void CheckVelocityEstimator()
    {
        if (GetHandObject() != null)
        {
            if (ownerVelocityEstimator == null)
                ownerVelocityEstimator = GetHandObject().GetComponentInChildren<VelocityEstimatorParent>();
        }
        else
        {
            if (ownerVelocityEstimator != null)
                ownerVelocityEstimator = null;
        }

    }

    private void CheckChambering()
    {
        if (IsHeldByEnemy() || debugChambering)
        {
            if (!chambered && (nextChamber + 1f) < Time.time)
                StartChamberingAuto();

            return;
        }
        
        
        if (!chambered && nextChamber < Time.time && ownerVelocityEstimator != null)
        {
            Vector3 vel = ownerVelocityEstimator.GetVelocityEstimate();
            Vector3 dir = vel.normalized;
            // check vertical (gun relative) direction to prevent recoil from triggering the flip
            if (vel.sqrMagnitude > 1f && Vector3.Dot( dir, transform.up ) > 0.55 )
            {
                StartChambering();
            }
        }
    }

    public void LoadChamber()
    {
        chambered = true;
        animController?.SetBool("Shoot", false);
        if (IsHeldByEnemy() || GetHandObject() == null)
            return;
        if (UnityEngine.XR.XRSettings.enabled)
            hapticAction?.Execute(0, 0.07f, 80, 20, SteamVR_Input_Sources.RightHand);
    }

    protected override void DoRecoilHapticFeedback()
    {
        if (IsHeldByEnemy() || GetHandObject() == null)
            return;
        if (UnityEngine.XR.XRSettings.enabled)
            hapticAction?.Execute(0, 1.2f, 120, 1, SteamVR_Input_Sources.RightHand);
    }

    public void DoShellFX()
    {
        if (shellFx != null)
            shellFx.Emit(1);
    }

    public void DoLeverSound()
    {
        if (GetSoundData() != null && GetSoundData().miscSnd1 != null)
        {
            PlaySound(GetSoundData().miscSnd1);
        }
        animController?.SetBool("Shoot", false);
        animController?.SetTrigger("HammerPull");
    }

    public void StartChambering()
    {
        if (nextChamber > Time.time) return;
        //if (ownerAnimator == null) return;
        //if (gunAnimations.isPlaying) return;

        //gunAnimations.Play();
        //ownerAnimator.SetTrigger("Right Hand Trigger Special");

        animController?.SetTrigger("Reload");

        nextChamber = 0.34f + Time.time;
    }

    public void StartChamberingAuto()
    {
        if (nextChamber > Time.time) return;

        //gunAnimations.Play();

        animController?.SetTrigger("Reload");

        nextChamber = 0.34f + Time.time;
    }

    //public override bool CanBeDropped() => !gunAnimations.isPlaying;

    protected override float GetRecoil() => 10f;

    public override bool CanShoot() => CurClip > 0 && chambered;

    public override float IsAIInRange() => 10f;
}

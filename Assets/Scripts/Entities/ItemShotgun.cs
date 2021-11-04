using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
using Valve.VR;

public class ItemShotgun : BaseRangedWeapon
{
    [SerializeField]
    Animation gunAnimations;
    [SerializeField]
    protected VisualEffect muzzleShotgunVfx;

    Animator ownerAnimator;
    VelocityEstimatorParent ownerVelocityEstimator;
    bool chambered = true;
    float nextChamber = 0f;

    protected override void OnStart()
    {
        itemHoldtype = (int)Holdtype.Shotgun;
        numShots = 6;
        spread = 5f;
        gunAnimations[gunAnimations.clip.name].speed = 1.7f;
    }
    protected override void OnUpdate()
    {
        CheckAnimator();
        CheckVelocityEstimator();
        CheckChambering();
    }

    protected override float GetFireDelay()
    {
        return 1f;
    }

    protected override float GetSpread()
    {
        return 3f;
    }

    protected override void ShootEffects()
    {
        if (muzzleFx != null)
            muzzleFx.Play(true);

        if (muzzleShotgunVfx != null)
            muzzleShotgunVfx.Play();

        chambered = false;
        //StartChambering();
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
        hapticAction.Execute(0, 0.07f, 80, 20, SteamVR_Input_Sources.RightHand);
    }

    protected override void DoRecoilHapticFeedback()
    {
        hapticAction.Execute(0, 1.5f, 320, 1, SteamVR_Input_Sources.RightHand);
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
    }

    public void StartChambering()
    {
        if (nextChamber > Time.time) return;
        if (ownerAnimator == null) return;
        //if (gunAnimations.isPlaying) return;

        gunAnimations.Play();
        ownerAnimator.SetTrigger("Right Hand Trigger Special");

        nextChamber = 0.34f + Time.time;
    }

    public override bool CanBeDropped()
    {
        return !gunAnimations.isPlaying;
    }

    protected override float GetRecoil()
    {
        return 10f;
    }

    public override bool CanShoot()
    {
        return ( curClip > 0 && chambered );
    }

}

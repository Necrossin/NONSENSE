using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using UnityEngine.VFX;
using Random = UnityEngine.Random;
using Valve.VR;

public class BaseRangedWeapon : BaseInteractable
{

    [SerializeField]
    private Transform muzzleTransform;

    [SerializeField]
    private GameObject decalTest;
    [SerializeField]
    private GameObject impactTest;

    [SerializeField]
    protected ParticleSystem shellFx;
    [SerializeField]
    protected ParticleSystem muzzleFx;
    [SerializeField]
    protected GameObject tracerFx;

    [SerializeField]
    private LayerMask playerFilter;
    [SerializeField]
    private LayerMask enemyFilter;

    [SerializeField]
    protected AudioSource sndSource;
    [SerializeField]
    protected WeaponSoundData sndData;

    [SerializeField]
    protected SteamVR_Action_Vibration hapticAction;

    protected int maxClip = 7;
    protected int curClip = 7;
    protected float curClipDelta = 1;

    protected int damage = 15;
    protected int numShots = 1;

    protected float spread = 0.02f;
    protected float recoil = 1.5f;

    protected float fireDelay = 0.4f;
    protected float nextFireDelay = 0;

    protected bool triggerHold = false;

    
    protected override void OnStart()
    {

    }
    protected override void OnUpdate()
    {

    }

    void Update()
    {
        if (triggerHold)
            DoTriggerInteraction();

        OnUpdate();
    }

    public override void DoTriggerInteraction()
    {
        float t = Time.time;

        if (nextFireDelay < t)
        {
            if (CanShoot())
                Shoot();
            else
                Debug.Log("Empty!");

            nextFireDelay = t + GetFireDelay();
        }
    }

    public override void DoTriggerInteractionHold(bool start)
    {
        triggerHold = start;
    }


    protected virtual void Shoot()
    {
        //float t = Time.time;

        //if ( nextFireDelay < t )
        //{

        //TakeAmmo(1);
            
            FireBullet();
            ShootEffects();
            ShootSound();
            DoRecoil();

        //    nextFireDelay = t + GetFireDelay();
        //}
        //else
        //{

        //}
    }

    protected virtual void FireBullet()
    {
        for (int i=1;i<=numShots;i++)
        {
            RaycastHit hitInfo;

            Vector3 dir = muzzleTransform.forward * 100;

            dir += muzzleTransform.up * Random.Range(-GetSpread(), GetSpread());
            dir += muzzleTransform.right * Random.Range(-GetSpread(), GetSpread());

            dir = dir.normalized;

            bool hit = Physics.Raycast(muzzleTransform.position, dir, out hitInfo, 5000, playerFilter);

            if ( hit )
            {
                BulletCallback(hitInfo, dir, i);
            }
        }
    }

    protected virtual void BulletCallback( RaycastHit hitInfo, Vector3 dir, int bulletNum )
    {
        TracerEffects(hitInfo.point);
        DebugHitInfo(hitInfo, dir, bulletNum);
    }

    private void DebugHitInfo( RaycastHit hitInfo, Vector3 dir, int bulletNum )
    {
        if (decalTest != null)
        {
            GameObject test = Instantiate(decalTest, hitInfo.point + hitInfo.normal * 0.001f, Quaternion.FromToRotation(Vector3.forward * -1, hitInfo.normal));
            test.transform.localScale = test.transform.localScale * Random.Range(0.5f, 1.1f);
            test.transform.rotation *= Quaternion.AngleAxis(Random.Range(-180, 180), Vector3.forward);

            float dot = Vector3.Dot(hitInfo.normal, dir * -1);
            dot = Mathf.Clamp(dot, 0.55f, 1);
            dot = 1 - dot;
            dot = dot / 0.45f;

            float deep = 0.4f - 0.35f * dot;

            var rend = test.GetComponent<Renderer>();
            var props = new MaterialPropertyBlock();

            rend.GetPropertyBlock(props);
            props.SetFloat("_ParallaxDepth", deep);
            rend.SetPropertyBlock(props);


            if (bulletNum == 1)
            {
                var snd = test.GetComponent<AudioSource>();
                snd.pitch = Random.Range(0.9f, 1.1f);
                snd.Play();
            }
            

            Destroy(test, 30f);
        }

        if (impactTest != null)
        {
            //GameObject test2 = Instantiate(impactTest, hitInfo.point + hitInfo.normal * 0.001f, Quaternion.FromToRotation(Vector3.right * -1, hitInfo.normal));
            Quaternion rotation = Quaternion.FromToRotation(Vector3.right * -1, Vector3.Reflect(dir, hitInfo.normal * -1));
            rotation.eulerAngles += new Vector3(Random.Range(-0.05f, 0.05f), Random.Range(-0.05f, 0.05f), Random.Range(-0.05f, 0.05f));

            GameObject test2 = Instantiate(impactTest, hitInfo.point + hitInfo.normal * 0.008f, rotation);
            var vfx = test2.GetComponentInChildren<VisualEffect>();
            if (vfx != null)
                vfx.transform.rotation = Quaternion.FromToRotation(Vector3.right * -1, hitInfo.normal);
        }
    }

    protected virtual void DoRecoil()
    {
        if (GetHandObject() != null)
        {
            var rb = GetHandObject().GetComponent<Rigidbody>();

            if (rb != null)
            {
                rb.AddForceAtPosition(muzzleTransform.transform.forward * -GetRecoil(), muzzleTransform.transform.position, ForceMode.Impulse);
            }

        }

        DoRecoilHapticFeedback();

    }

    protected virtual void DoRecoilHapticFeedback()
    {
        hapticAction.Execute(0, GetFireDelay(), GetRecoil() * 40, 1, SteamVR_Input_Sources.RightHand);
    }

    public override void OnDrop()
    {
        base.OnDrop();
        triggerHold = false;
    }

    private void TakeAmmo( int am )
    {
        curClip = Mathf.Clamp(curClip - am, 0, maxClip);
    }

    public virtual bool CanShoot() => curClip > 0;

    public int GetClip() => curClip;

    public int GetMaxClip() => maxClip;

    protected void PlaySound( AudioClip clip, float pitch = 1 )
    {
        if (sndSource != null && clip != null)
        {
            sndSource.pitch = pitch;
            //sndSource.clip = clip;
            sndSource.PlayOneShot(clip);
        }
    }

    protected virtual void ShootSound()
    {
        if (GetSoundData() != null && GetSoundData().fireSnd != null)
        {
            PlaySound(GetSoundData().fireSnd, Random.Range(0.95f, 1f));
        }
    }

    protected virtual void ShootEffects()
    {
        if (muzzleFx != null)
            muzzleFx.Play(true);

        if (shellFx != null)
        {
            //var main = shellFx.main;
            //var emitterAng = shellFx.transform.rotation.eulerAngles;
            //main.startRotation3D = emitterAng;
            //main.startRotationX = emitterAng.x;//transform.rotation + new Vector3( 0, Random.Range(0.95f, 1f))
            //main.startRotationY = emitterAng.y;
            //main.startRotationZ = emitterAng.z;
            shellFx.Emit(1);
        }
            
    }

    protected virtual void TracerEffects( Vector3 hitPos )
    {
        if (tracerFx != null)
        {
            GameObject fx = Instantiate(tracerFx, muzzleTransform.transform.position, Quaternion.identity);
            if (fx != null)
            {
                var effectComponent = fx.GetComponent<VisualEffect>();
                var effectParams = fx.GetComponent<VFX_BulletTracer>();

                effectParams.SetHitPos( hitPos );

                effectComponent.Play();

                Destroy(fx, 1);
            }

        }
    }

    protected virtual float GetFireDelay() => fireDelay;

    protected virtual float GetSpread() => spread;

    protected virtual float GetRecoil() => recoil;

    protected virtual WeaponSoundData GetSoundData() => sndData;

}

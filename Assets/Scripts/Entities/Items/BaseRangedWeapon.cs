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

    private int maxClip = 8;
    private int curClip = 8;

    protected float curClipDelta = 1;

    protected int damage = 15;
    protected int numShots = 1;

    protected float spread = 0.02f;
    protected float recoil = 1.5f;

    protected float fireDelay = 0.4f;
    protected float nextFireDelay = 0;

    protected bool triggerHold = false;

    protected int MaxClip { get => maxClip; set => maxClip = value; }
    protected int CurClip { get => curClip; set => curClip = value; }

    protected AmmoCounter ammoCounter;

    protected new void Start()
    {
        base.Start();

        ammoCounter = GetComponentInChildren<AmmoCounter>();
    }

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
            //else
            //    Debug.Log("Empty!");

            nextFireDelay = t + GetFireDelay();
        }
    }

    public override void DoTriggerInteractionHold(bool start)
    {
        triggerHold = start;
    }


    protected virtual void Shoot()
    {
        TakeAmmo(1); 
        FireBullet();
        ShootEffects();
        ShootSound();
        DoRecoil();
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

            bool hit = Physics.Raycast(muzzleTransform.position, dir, out hitInfo, 5000, GetBulletLayerMask());

            if ( hit )
            {
                PenetratingRayCast(1 << 7, muzzleTransform.position, hitInfo.point, dir, hitInfo.distance);
                BulletCallback(hitInfo, dir, i);
            }
        }
    }

    protected virtual void BulletCallback( RaycastHit hitInfo, Vector3 dir, int bulletNum )
    {
        TracerEffects(hitInfo.point);

        // only make decals on world for now
        if (hitInfo.collider != null && hitInfo.collider.gameObject.layer == 9)
            DebugHitInfo(hitInfo, dir, bulletNum);
            
    }

    protected virtual void PenetratingRayCast( LayerMask layerMask, Vector3 vStart, Vector3 vEnd, Vector3 vDir, float fDist )
    {
        var hitObjects = Physics.RaycastAll(vStart, vDir, fDist, layerMask);
        RaycastHit hitInfo;

        for (int i = 0; i < hitObjects.Length; i++)
        {
            hitInfo = hitObjects[i];

            var hitObject = hitInfo.collider.gameObject;
            if (hitObject != null)
            {
                var glassScript = hitObject.GetComponent<GlassMesh>();
                glassScript?.Break(hitInfo.point, vDir, Random.Range(2.5f,3f));

                var glassJoint = hitObject.GetComponent<HingeJoint>();
                if (glassJoint != null)
                {
                    var glassRB = hitInfo.collider.attachedRigidbody;
                    if (glassRB != null)
                    {
                        glassRB.AddForceAtPosition(Random.Range(2.5f, 3f) * vDir * 3, hitInfo.point, ForceMode.Impulse);
                    }
                }
            }
        }
    }

    private void DebugHitInfo( RaycastHit hitInfo, Vector3 dir, int bulletNum )
    {
        if (decalTest != null)
        {
            GameObject test = Pool.Instance.InstantiateFromPool(decalTest, hitInfo.point + hitInfo.normal * 0.001f, Quaternion.FromToRotation(Vector3.forward * -1, hitInfo.normal));
            if (test != null)
            {
                test.transform.localScale = new Vector3(0.19f, 0.19f, 0.19f) * Random.Range(0.5f, 1.1f);
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
            }
        }

        if (impactTest != null)
        {
            Quaternion rotation = Quaternion.FromToRotation(Vector3.right * -1, Vector3.Reflect(dir, hitInfo.normal * -1));
            rotation.eulerAngles += new Vector3(Random.Range(-0.05f, 0.05f), Random.Range(-0.05f, 0.05f), Random.Range(-0.05f, 0.05f));

            GameObject test2 = Pool.Instance.InstantiateFromPool(impactTest, hitInfo.point + hitInfo.normal * 0.008f, rotation, true);

            var impactScript = test2.GetComponentInChildren<FX_ImpactDefault>();
            if (impactScript != null)
            {
                impactScript.isSilent = bulletNum != 1;
                //impactScript.SetRotation(Quaternion.FromToRotation(Vector3.right * 1, hitInfo.normal));
            }
                

            test2.SetActive(true);
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

            DoRecoilHapticFeedback();
        }
    }

    protected virtual void DoRecoilHapticFeedback()
    {
        // AI has no haptic feedback
        if (IsHeldByEnemy())
            return;

        hapticAction.Execute(0, GetFireDelay(), GetRecoil() * 40, 1, SteamVR_Input_Sources.RightHand);
    }

    public override void OnDrop()
    {
        base.OnDrop();
        triggerHold = false;
    }

    protected virtual void TakeAmmo( int am )
    {
        CurClip = Mathf.Clamp(CurClip - am, 0, GetMaxClip());

        if (ammoCounter != null)
            ammoCounter.PlayFireEffects();
    }

    public virtual bool CanShoot() => CurClip > 0;

    public virtual int GetClip() => CurClip;

    public virtual int GetMaxClip() => MaxClip;

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
            shellFx.Emit(1);
        }
            
    }

    protected virtual void TracerEffects( Vector3 hitPos )
    {
        if (tracerFx != null)
        {
            //GameObject fx = Instantiate(tracerFx, muzzleTransform.transform.position, Quaternion.identity);
            GameObject fx = Pool.Instance.InstantiateFromPool(tracerFx, muzzleTransform.transform.position, Quaternion.identity);
            if (fx != null)
            {
                var effectComponent = fx.GetComponent<VisualEffect>();
                var effectParams = fx.GetComponent<VFX_BulletTracer>();

                effectParams.SetHitPos( hitPos );

                effectComponent.Play();

                //Destroy(fx, 1);
            }

        }
    }

    public virtual float GetFireDelay() => fireDelay;

    protected virtual float GetSpread() => spread;

    protected virtual float GetRecoil() => recoil;

    protected virtual WeaponSoundData GetSoundData() => sndData;

    public override bool IsGrabbable()
    {
        if (transform.parent != null)
            return false;

        return base.IsGrabbable();
    }

    public LayerMask GetBulletLayerMask()
    {
        return IsHeldByEnemy() ? enemyFilter : playerFilter;
    }

    public Vector3 GetMuzzlePosition() => muzzleTransform.position;

    // by default its about 10 meters
    public virtual float IsAIInRange() => 10f;

}

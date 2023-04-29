using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class FX_ImpactDefault : MonoBehaviour
{
    [SerializeField]
    ParticleSystem fxMain;
    [SerializeField]
    VisualEffect fxDust;
    [SerializeField]
    VisualEffect fxDustBig;

    [SerializeField]
    SoundDataSimpleList lstImpactSounds;
    [SerializeField]
    AudioSource audioSource;

    public float cullingRadius = 2.5f;

    CullingGroup cullingGroup;
    Renderer[] particleRenderers;

    public bool isSilent { get; set; } = false;

    void Start()
    {
        Globals.TryResolvingAudioProbes(gameObject);
    }

    void Update()
    {
        
    }

    void OnEnable()
    {
        fxMain?.Play(true);
        //fxDustBig?.Play();
        PlayImpactSound();

        /*if (particleRenderers == null)
            particleRenderers = fxMain.GetComponentsInChildren<Renderer>();

        if (cullingGroup == null)
        {
            cullingGroup = new CullingGroup();
            cullingGroup.targetCamera = Camera.main;
            cullingGroup.SetBoundingSpheres(new[] { new BoundingSphere(transform.position, cullingRadius) });
            cullingGroup.SetBoundingSphereCount(1);
            cullingGroup.onStateChanged += OnStateChanged;

            Cull(cullingGroup.IsVisible(0));
        }

        cullingGroup.enabled = true;*/

        /*StimulusRecordStruct scs = new StimulusRecordStruct(StimulusType.WeaponImpactSound, transform.position, null );
        scs.m_flAlarmScalar = 1;
        scs.m_flRadiusScalar = 5;

        AI_Globals.g_StimulusManager?.RegisterStimulus(scs);*/
    }

    void OnDisable()
    {
        isSilent = false;

        if (cullingGroup != null)
            cullingGroup.enabled = false;
    }

    void OnDestroy()
    {
        if (cullingGroup != null)
            cullingGroup.Dispose();
    }

    void OnStateChanged(CullingGroupEvent sphere)
    {
        Cull(sphere.isVisible);
    }

    void Cull(bool visible)
    {
        Debug.Log(visible);
        if (visible)
        {
            fxMain.Play(true);
            SetRenderers(true);
        }
        else
        {
            fxMain.Pause(true);
            SetRenderers(false);
        }
    }

    void SetRenderers(bool enable)
    {
        foreach (var particleRenderer in particleRenderers)
        {
            particleRenderer.enabled = enable;
        }
    }

    void OnDrawGizmos()
    {
        if (enabled)
        {
            Color col = Color.yellow;
            if (cullingGroup != null && !cullingGroup.IsVisible(0))
                col = Color.gray;

            Gizmos.color = col;
            Gizmos.DrawWireSphere(transform.position, cullingRadius);
        }
    }

    void PlayImpactSound()
    {
        if (isSilent)
            return;

        if (lstImpactSounds == null)
            return;

        var clip = lstImpactSounds.GetRandomClip();
        if (clip == null)
            return;

        audioSource.pitch = Random.Range(0.85f, 1.2f);
        audioSource.PlayOneShot(clip);
    }

    public void SetRotation( Quaternion qRot )
    {
        //m_fxDustBig.transform.rotation = qRot;
    }
}

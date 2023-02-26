using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class FX_ImpactDefault : MonoBehaviour
{
    [SerializeField]
    ParticleSystem m_fxMain;
    [SerializeField]
    VisualEffect m_fxDust;
    [SerializeField]
    VisualEffect m_fxDustBig;

    [SerializeField]
    SoundDataSimpleList m_lstImpactSounds;
    [SerializeField]
    AudioSource m_AudioSource;

    public bool m_bSilent { get; set; } = false;

    void Start()
    {
        Globals.TryResolvingAudioProbes(gameObject);
    }

    void Update()
    {
        
    }

    void OnEnable()
    {
        m_fxMain?.Play(true);
        //m_fxDustBig.Play();

        PlayImpactSound();

        /*StimulusRecordStruct scs = new StimulusRecordStruct(StimulusType.WeaponImpactSound, transform.position, null );
        scs.m_flAlarmScalar = 1;
        scs.m_flRadiusScalar = 5;

        AI_Globals.g_StimulusManager?.RegisterStimulus(scs);*/
    }

    void OnDisable()
    {
        m_bSilent = false;
    }

    void PlayImpactSound()
    {
        if (m_bSilent)
            return;

        if (m_lstImpactSounds == null)
            return;

        var clip = m_lstImpactSounds.GetRandomClip();
        if (clip == null)
            return;

        m_AudioSource.pitch = Random.Range(0.85f, 1.2f);
        m_AudioSource.PlayOneShot(clip);
    }

    public void SetRotation( Quaternion qRot )
    {
        //m_fxDustBig.transform.rotation = qRot;
    }
}

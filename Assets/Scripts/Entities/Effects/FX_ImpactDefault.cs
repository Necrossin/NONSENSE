using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class FX_ImpactDefault : MonoBehaviour
{
    ParticleSystem fxMain;
    VisualEffect vfxMain;

    [SerializeField]
    [Tooltip("Use EmptySoundList if there are no sounds")]
    SoundDataSimpleList lstImpactSounds;

    AudioSource audioSource;

    public bool isSilent { get; set; } = false;


    private void Awake()
    {
        fxMain = GetComponent<ParticleSystem>();

        if (fxMain == null)
            fxMain = GetComponentInChildren<ParticleSystem>();

        vfxMain = GetComponent<VisualEffect>();
        if (vfxMain == null)
            vfxMain = GetComponentInChildren<VisualEffect>();

        audioSource = GetComponent<AudioSource>();
    }
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
        vfxMain?.Play();
        PlayImpactSound();

        /*StimulusRecordStruct scs = new StimulusRecordStruct(StimulusType.WeaponImpactSound, transform.position, null );
        scs.m_flAlarmScalar = 1;
        scs.m_flRadiusScalar = 5;

        AI_Globals.g_StimulusManager?.RegisterStimulus(scs);*/
    }

    void OnDisable()
    {
        isSilent = false;
    }

    void OnDestroy()
    {
    }

    void PlayImpactSound()
    {
        if (isSilent)
            return;

        if (lstImpactSounds == null)
            return;

        if (lstImpactSounds.lstSoundList.Count < 1)
            return;

        var clip = lstImpactSounds.GetRandomClip();
        if (clip == null)
            return;

        if (audioSource == null)
            return;

        audioSource.pitch = Random.Range(0.85f, 1.2f);
        audioSource.PlayOneShot(clip);
    }
}

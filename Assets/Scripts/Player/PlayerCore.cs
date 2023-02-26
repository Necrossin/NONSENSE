using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Globals
{
    public static AudioListener PlayerAudioListener = null;
    public static GameObject Player = null;
}

public class PlayerCore : MonoBehaviour
{
    HealthManager healthManager;

    void Awake()
    {
        if (Globals.Player == null)
            Globals.Player = gameObject;
    }
    void Start()
    {
        if (Globals.PlayerAudioListener == null)
            Globals.PlayerAudioListener = GetComponentInChildren<AudioListener>();

        healthManager = GetComponent<HealthManager>();
        healthManager?.OnTakeDamageEvent.AddListener(OnTakeDamage);
        healthManager?.OnBreakEvent.AddListener(OnAllHealthLost);

    }

    
    void Update()
    {
        
    }

    // let ai know that player exists
    void RegisterPlayerVisibility()
    {
        // TODO: redo this
        
        /*Vector3 vPos = transform.position;

        StimulusRecordStruct scs = new StimulusRecordStruct(StimulusType.CharacterVisible, vPos, gameObject );
        scs.m_dwDynamicPosFlags |= (int)StimulusRecord.DynamicPos_Flag.TrackSource;
        AI_Globals.g_StimulusManager.RegisterStimulus(scs);*/
    }

    public void OnTakeDamage(DamageInfo damageInfo)
    {
        //Debug.Log(gameObject.name + " called take damage event with args "+ damageInfo.iAmount);
    }

    public void OnAllHealthLost(DamageInfo damageInfo)
    {
        //Debug.Log(gameObject.name + " called all health lost event with args "+ damageInfo.iAmount);
    }
}

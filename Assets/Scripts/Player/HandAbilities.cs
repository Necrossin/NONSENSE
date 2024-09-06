using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
using Valve.VR;

public class HandAbilities : MonoBehaviour
{

    [SerializeField]
    private ControllerInput inp;
    private HandAnimationsShared anim;

    [SerializeField]
    private GameObject handObject;
    [SerializeField]
    private GameObject playerObject;

    [SerializeField]
    private VisualEffect leftHandVFX;

    public Transform palmAttachment;

    [SerializeField]
    SteamVR_Action_Vibration hapticAction;

    private string activeAbility;

    void Start()
    {
        anim = GetComponentInParent<HandAnimationsShared>();
    }


    void Update()
    {

    }

    public GameObject GetHandObject() => handObject;

    public GameObject GetPlayerObject() => playerObject;

    public VisualEffect GetHandVFX() => leftHandVFX;

    public HandAnimationsShared GetHandAnimations => anim;

    public ControllerInput GetControllerInput => inp;

    public void SetActiveAbility(string ability) => activeAbility = ability;

    public void ClearActiveAbility() => activeAbility = null;

    public string GetActiveAbility() => activeAbility;
}

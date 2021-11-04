using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class HandAbilities : MonoBehaviour
{

    [SerializeField]
    private ControllerInput inp;
    private HandAnimations anim;

    [SerializeField]
    private GameObject handObject;
    [SerializeField]
    private GameObject playerObject;

    public Transform palmAttachment;

    [SerializeField]
    SteamVR_Action_Vibration hapticAction;

    private string activeAbility;

    void Start()
    {
        anim = GetComponentInParent<HandAnimations>();
    }


    void Update()
    {

    }

    public GameObject GetHandObject()
    {
        return handObject;
    }

    public GameObject GetPlayerObject()
    {
        return playerObject;
    }

    public HandAnimations GetHandAnimations 
    { 
        get
        {
            return anim;
        }
    }

    public ControllerInput GetControllerInput
    {
        get
        {
            return inp;
        }
    }

    public void SetActiveAbility( string ability )
    {
        activeAbility = ability;
    }

    public void ClearActiveAbility()
    {
        activeAbility = null;
    }

    public string GetActiveAbility()
    {
        return activeAbility;
    }    
}

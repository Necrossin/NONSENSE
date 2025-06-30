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

    [Header("Finger Bones")]
    [SerializeField]
    List<Transform> fingerBones;

    [SerializeField]
    Transform abilitySyncTrandform;

    [SerializeField]
    private List<AbilitySlotHandler> abilitySlots;

    private string activeAbility;

    void Start()
    {
        anim = GetComponentInParent<HandAnimationsShared>();
    }


    void Update()
    {

    }

    public bool TryToAddAbility(Ability_Template abilityClass)
    {
        int slot = abilityClass.GetSlotIndex();
        AbilitySlotHandler matchingSlot = abilitySlots[slot];

        if (matchingSlot == null) return false;
        if (matchingSlot.IsOccupied()) return false;

        return matchingSlot.TryAttachAbility(abilityClass);
    }

    public GameObject GetHandObject() => handObject;

    public GameObject GetPlayerObject() => playerObject;

    public VisualEffect GetHandVFX() => leftHandVFX;

    public HandAnimationsShared GetHandAnimations => anim;

    public ControllerInput GetControllerInput => inp;

    public void SetActiveAbility(string ability) => activeAbility = ability;

    public void ClearActiveAbility() => activeAbility = null;

    public string GetActiveAbility() => activeAbility;
    public List<Transform> GetFingerBones() => fingerBones;

    public Transform GetAbilitySyncTransform() => abilitySyncTrandform;

    public List<AbilitySlotHandler> GetAbilitySlots() => abilitySlots;
}

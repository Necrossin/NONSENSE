using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class Ability_Template : MonoBehaviour, IAbility
{
    public string abilityName = null;
    public int slotIndex = (int)AbilitySlot.Open;
    public int abilityHoldtype { get; set; } = 0;
    protected bool isAttached = false;
    

    protected HandAbilities abilityManager;
    protected ControllerInput inp;
    protected HandAnimationsShared anim;

    protected GameObject handObject;
    protected GameObject playerObject;
    protected CharacterController playerController;
    protected PlayerMovement playerMovement;

    // local velocity estimator (for movement relative to player)
    protected VelocityEstimatorParent velEstimatorLocal;

    // normal velocity estimator (for throwing etc)
    protected VelocityEstimator velEstimator;

    [SerializeField]
    protected SteamVR_Action_Vibration hapticAction;
    protected enum AbilitySlot
    {
        Point,
        Open,
        Hold
    }

    void Start()
    {
        
    }

    
    void Update()
    {
        
    }

    protected virtual void CheckGesture()
    {

    }

    public string GetName() => abilityName;

    public void AttachAbility( HandAbilities parent )
    {
        OnAttach(parent);

        abilityManager = parent;
        handObject = parent.GetHandObject();
        playerObject = parent.GetPlayerObject();
        playerController = parent.GetPlayerObject().GetComponent<CharacterController>();
        playerMovement = parent.GetPlayerObject().GetComponent<PlayerMovement>();
        velEstimatorLocal = parent.GetComponent<VelocityEstimatorParent>();
        velEstimator = parent.GetComponent<VelocityEstimator>();

        anim = parent.GetHandAnimations;
        inp = parent.GetControllerInput;

        isAttached = true;
    }

    public void DetachAbility()
    {
        OnDetach();

        abilityManager = null;
        handObject = null;
        playerObject = null;
        playerController = null;
        playerMovement = null;
        anim = null;
        inp = null;

        isAttached = false;
    }

    public bool IsAttached() => isAttached;

    public int GetSlotIndex() => slotIndex;

    protected virtual void OnAttach(HandAbilities parent)
    {
    }

    protected virtual void OnDetach()
    {
    }

    protected virtual void OnActivate()
    {
    }

    protected virtual void OnDeactivate()
    {
    }

    protected void TryActivateAbility()
    {
        if (abilityManager.GetActiveAbility() == null)
        {
            abilityManager.SetActiveAbility(abilityName);
            anim.SetAbilityHoldtype(abilityHoldtype);
            OnActivate();
            //Debug.Log("Activated");
        }    
    }

    protected void DeactivateThisAbility()
    {
        if (IsActive())
        {
            abilityManager.ClearActiveAbility();
            anim.SetAbilityHoldtype(0);
            OnDeactivate();
            //Debug.Log("Dectivated");
        }   
    }

    protected bool IsActive() => abilityName != null && abilityManager.GetActiveAbility() == abilityName;
}

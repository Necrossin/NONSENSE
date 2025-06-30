using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class Ability_Template : MonoBehaviour, IAbility
{
    public string abilityName = null;
    public int slotIndex = (int)AbilitySlot.Open;
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

    protected ItemAbilityTemplate itemTemplate;
    protected Animator animController;

    private int activeLayer;
    private float activeLayerWeight = 0f;

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
        itemTemplate = GetComponent<ItemAbilityTemplate>();
        animController = itemTemplate.GetAnimatorController();

        if (animController != null)
            activeLayer = animController.GetLayerIndex("Active Layer");

        OnStart();
    }

    protected virtual void OnStart() 
    { 
    }


    void Update()
    {
        HandleAnimationWeight();

        OnUpdate();
    }

    protected virtual void OnUpdate()
    {
    }
    // Ability anims are in weighted layer, just so we can have some smooth hand transitions
    protected void HandleAnimationWeight()
    {
        if (!IsAttached()) return;
        if (animController == null) return;

        float activeWeightGoal = IsActive() ? 1 : 0;

        //activeLayerWeight = Mathf.Lerp(activeLayerWeight, activeWeightGoal, Time.deltaTime * (IsActive() ? AnimationBlendInRate() : AnimationBlendOutRate()));
        //TODO: maybe hook this up to the animation time instead?
        if (IsActive())
            activeLayerWeight = Mathf.Clamp01(activeLayerWeight + Time.deltaTime * AnimationBlendInRate());
        else
            activeLayerWeight = Mathf.Clamp01(activeLayerWeight - Time.deltaTime * AnimationBlendOutRate());

        animController?.SetLayerWeight(activeLayer, activeLayerWeight);
    }

    void LateUpdate()
    {
        if (abilityManager != null && (activeLayerWeight > 0) && itemTemplate.GetFingerBones().Count > 0 && abilityManager.GetFingerBones().Count > 0)
        {
            for (int i = 0; i < itemTemplate.GetFingerBones().Count; i++)
            {
                var goalTransform = itemTemplate.GetFingerBones()[i];
                var curTransform = abilityManager.GetFingerBones()[i];

                if (curTransform == null || goalTransform == null)
                    continue;

                curTransform.localPosition = goalTransform.localPosition;
                curTransform.localRotation = goalTransform.localRotation;
            }
        }

        OnLateUpdate();
    }

    protected virtual void OnLateUpdate()
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
            //anim.SetAbilityHoldtype(abilityHoldtype);
            OnActivate();
            //Debug.Log("Activated");
        }    
    }

    protected void DeactivateThisAbility()
    {
        if (IsActive())
        {
            abilityManager.ClearActiveAbility();
            //anim.SetAbilityHoldtype(0);
            OnDeactivate();
            //Debug.Log("Dectivated");
        }   
    }

    protected bool IsActive() => abilityName != null && abilityManager.GetActiveAbility() == abilityName;

    protected virtual float AnimationBlendInRate() => 20;
    protected virtual float AnimationBlendOutRate() => 5;
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandAnimations : MonoBehaviour
{

    [SerializeField]
    private ControllerInput inp;

    [SerializeField]
    private Animator rightHandAnimator;

    [SerializeField]
    private Animator leftHandAnimator;

    private float lerpRightGrip = 0f;
    private float lerpRightTrigger = 0f;
    private float lerpRightThumb = 0f;
    private float lerpRightHoldtype = 0f;

    private float lerpLeftGrip = 0f;
    private float lerpLeftTrigger = 0f;
    private float lerpLeftThumb = 0f;
    private float lerpLeftHoldtype = 0f;

    void Start()
    {
        
    }

    
    void Update()
    {
        
        // Right Hand
        
        // index layer
        lerpRightTrigger = Mathf.Lerp(lerpRightTrigger, inp.RightTriggerDelta(), Time.deltaTime * 15);
        rightHandAnimator.SetFloat("Right Hand Point Closed", lerpRightTrigger);

        // thumb layer
        lerpRightThumb = Mathf.Lerp(lerpRightThumb, inp.RightThumbDelta(), Time.deltaTime * 10);
        rightHandAnimator.SetFloat("Right Hand Thumb Closed", lerpRightThumb);

        // main grab
        lerpRightGrip = Mathf.Lerp(lerpRightGrip, inp.RightGripDelta(), Time.deltaTime * 15);
        rightHandAnimator.SetFloat("Right Hand Closed", lerpRightGrip);

        // adjust finger weight for holdtypes
        lerpRightHoldtype = Mathf.Lerp(lerpRightHoldtype, GetHoldtype(true) == 0 ? 0 : 1, Time.deltaTime * 15);
        rightHandAnimator.SetLayerWeight(rightHandAnimator.GetLayerIndex("Index Layer"), 1 - lerpRightHoldtype);// * lerpRightTrigger);
        rightHandAnimator.SetLayerWeight(rightHandAnimator.GetLayerIndex("Thumb Layer"), 1 - lerpRightHoldtype);// * lerpRightThumb);


        // Left Hand

        // index layer
        lerpLeftTrigger = Mathf.Lerp(lerpLeftTrigger, inp.LeftTriggerDelta(), Time.deltaTime * 15);
        leftHandAnimator.SetFloat("Left Hand Point Closed", lerpLeftTrigger);

        // thumb layer
        lerpLeftThumb = Mathf.Lerp(lerpLeftThumb, inp.LeftThumbDelta(), Time.deltaTime * 10);
        leftHandAnimator.SetFloat("Left Hand Thumb Closed", lerpLeftThumb);

        // main grab
        lerpLeftGrip = Mathf.Lerp(lerpLeftGrip, inp.LeftGripDelta(), Time.deltaTime * 15);
        leftHandAnimator.SetFloat("Left Hand Closed", lerpLeftGrip);

        //todo: finger weight for special left hand anims
        lerpLeftHoldtype = Mathf.Lerp(lerpLeftHoldtype, GetAbilityHoldtype() == 0 ? 0 : 1, Time.deltaTime * 15);
        leftHandAnimator.SetLayerWeight(leftHandAnimator.GetLayerIndex("Index Layer"), 1 - lerpLeftHoldtype);// * lerpRightTrigger);
        leftHandAnimator.SetLayerWeight(leftHandAnimator.GetLayerIndex("Thumb Layer"), 1 - lerpLeftHoldtype);// * lerpRightThumb);

    }

    public void SetHoldtype( bool primaryHand, int holdtype )
    {
        if (primaryHand)
            rightHandAnimator.SetInteger("Right Hand Holdtype", holdtype);
        else
            leftHandAnimator.SetInteger("Left Hand Active Ability", holdtype);
    }

    public int GetHoldtype( bool primaryHand )
    {
        if (primaryHand)
            return rightHandAnimator.GetInteger("Right Hand Holdtype");
        else
            return leftHandAnimator.GetInteger("Left Hand Active Ability");
    }

    public void SetAbilityHoldtype(int holdtype) => SetHoldtype(false, holdtype);

    public int GetAbilityHoldtype() => GetHoldtype(false);

    public void DoTriggerInteraction(bool primaryHand)
    {
        if (primaryHand)
            rightHandAnimator.SetTrigger("Right Hand Trigger Gesture");
        else
            leftHandAnimator.SetTrigger("Left Hand Trigger Gesture");
    }

    public void DoCustomAnimationTrigger(bool primaryHand, string name)
    {
        if (primaryHand)
            rightHandAnimator.SetTrigger( name );
        else
            leftHandAnimator.SetTrigger( name );
    }
}

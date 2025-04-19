using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandAnimationsShared : MonoBehaviour
{
    [SerializeField]
    private ControllerInput inp;

    [SerializeField]
    private Animator animator;

    private float lerpRightGrip = 0f;
    private float lerpRightTrigger = 0f;
    private float lerpRightThumb = 0f;
    private float lerpRightHoldtype = 0f;

    private float lerpLeftGrip = 0f;
    private float lerpLeftTrigger = 0f;
    private float lerpLeftThumb = 0f;
    private float lerpLeftHoldtype = 0f;

    void Update()
    {
        if (animator == null ) return;

        // Right Hand

        // index layer
        lerpRightTrigger = Mathf.Lerp(lerpRightTrigger, inp.RightTriggerDelta(), Time.deltaTime * 15);
        animator.SetFloat("Right Hand Point Closed", lerpRightTrigger);

        // thumb layer
        lerpRightThumb = Mathf.Lerp(lerpRightThumb, inp.RightThumbDelta(), Time.deltaTime * 10);
        animator.SetFloat("Right Hand Thumb Closed", lerpRightThumb);

        // main grab
        lerpRightGrip = Mathf.Lerp(lerpRightGrip, inp.RightGripDelta(), Time.deltaTime * 15);
        animator.SetFloat("Right Hand Closed", lerpRightGrip);

        // adjust finger weight for holdtypes
        lerpRightHoldtype = Mathf.Lerp(lerpRightHoldtype, GetHoldtype(true) == 0 ? 0 : 1, Time.deltaTime * 15);
        animator.SetLayerWeight(animator.GetLayerIndex("R Index Layer"), 1 - lerpRightHoldtype);// * lerpRightTrigger);
        animator.SetLayerWeight(animator.GetLayerIndex("R Thumb Layer"), 1 - lerpRightHoldtype);// * lerpRightThumb);


        // Left Hand

        // index layer
        lerpLeftTrigger = Mathf.Lerp(lerpLeftTrigger, inp.LeftTriggerDelta(), Time.deltaTime * 15);
        animator.SetFloat("Left Hand Point Closed", lerpLeftTrigger);

        // thumb layer
        lerpLeftThumb = Mathf.Lerp(lerpLeftThumb, inp.LeftThumbDelta(), Time.deltaTime * 10);
        animator.SetFloat("Left Hand Thumb Closed", lerpLeftThumb);

        // main grab
        lerpLeftGrip = Mathf.Lerp(lerpLeftGrip, inp.LeftGripDelta(), Time.deltaTime * 15);
        animator.SetFloat("Left Hand Closed", lerpLeftGrip);

        //todo: finger weight for special left hand anims
        lerpLeftHoldtype = Mathf.Lerp(lerpLeftHoldtype, GetAbilityHoldtype() == 0 ? 0 : 1, Time.deltaTime * 15);
        animator.SetLayerWeight(animator.GetLayerIndex("L Index Layer"), 1 - lerpLeftHoldtype);
        animator.SetLayerWeight(animator.GetLayerIndex("L Thumb Layer"), 1 - lerpLeftHoldtype);
    }

    public void SetHoldtype(bool primaryHand, int holdtype)
    {
        if (animator == null ) return;

        if (primaryHand)
            //animator.SetInteger("Right Hand Holdtype", holdtype);
            animator.SetBool("Right Hand Item", holdtype == 0 ? false : true);
        else
            animator.SetInteger("Left Hand Active Ability", holdtype);
    }

    public int GetHoldtype(bool primaryHand)
    {
        if (animator == null ) return 0;

        if (primaryHand)
            //return animator.GetInteger("Right Hand Holdtype");
            return animator.GetBool("Right Hand Item") == true ? 1 : 0;
        else
            return animator.GetInteger("Left Hand Active Ability");
    }

    public void SetAbilityHoldtype(int holdtype) => SetHoldtype(false, holdtype);

    public int GetAbilityHoldtype() => GetHoldtype(false);

    public void DoTriggerInteraction(bool primaryHand)
    {
        if (animator == null ) return;

        if (primaryHand)
            animator.SetTrigger("Right Hand Trigger Gesture");
        else
            animator.SetTrigger("Left Hand Trigger Gesture");
    }

    public void DoCustomAnimationTrigger(bool primaryHand, string name)
    {
        if (animator == null ) return;

        animator.SetTrigger(name);
    }
}

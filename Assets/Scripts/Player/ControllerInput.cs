using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Valve.VR;

public class ControllerInput : MonoBehaviour
{

    float rightTriggerPress, rightTriggerTouch, rightTriggerMove, rightGripPress, rightGripMove, rightThumbTouch;
    float leftTriggerPress, leftTriggerTouch, leftTriggerMove, leftGripPress, leftGripMove, leftThumbTouch;

    [SerializeField]
    SteamVR_Action_Boolean inputTriggerPress;
    [SerializeField]
    SteamVR_Action_Boolean inputTriggerTouch;
    [SerializeField]
    SteamVR_Action_Single inputTriggerMove;

    [SerializeField]
    SteamVR_Action_Boolean inputGripPress;
    [SerializeField]
    SteamVR_Action_Single inputGripMove;

    [SerializeField]
    SteamVR_Action_Boolean inputThumbTouch;

    [SerializeField]
    private HandCollision rightHandItemHandler;

    [SerializeField]
    private HandAnimations anim;


    private void Update()
    {
        HandleLeftInputs();
        HandleRightInputs();
    }

    // left hand

    public void HandleLeftInputs()
    {
        // trigger (without pressing)
        leftTriggerPress = inputTriggerPress.GetState(SteamVR_Input_Sources.LeftHand) ? 1 : 0;
        leftTriggerTouch = inputTriggerTouch.GetState(SteamVR_Input_Sources.LeftHand) ? 1 : 0;
        leftTriggerMove = inputTriggerMove.GetAxis(SteamVR_Input_Sources.LeftHand);

        // grip
        leftGripPress = inputGripPress.GetState(SteamVR_Input_Sources.LeftHand) ? 1 : 0;
        leftGripMove = inputGripMove.GetAxis(SteamVR_Input_Sources.LeftHand);

        // thumb
        leftThumbTouch = inputThumbTouch.GetState(SteamVR_Input_Sources.LeftHand) ? 1 : 0;

    }

    public void HandleRightInputs()
    {
        // trigger (without pressing)
        rightTriggerPress = inputTriggerPress.GetState(SteamVR_Input_Sources.RightHand) ? 1 : 0;
        rightTriggerTouch = inputTriggerTouch.GetState(SteamVR_Input_Sources.RightHand) ? 1 : 0;
        rightTriggerMove = inputTriggerMove.GetAxis(SteamVR_Input_Sources.RightHand);

        // grip
        rightGripPress = inputGripPress.GetState(SteamVR_Input_Sources.RightHand) ? 1 : 0;
        rightGripMove = inputGripMove.GetAxis(SteamVR_Input_Sources.RightHand);

        // thumb
        rightThumbTouch = inputThumbTouch.GetState(SteamVR_Input_Sources.RightHand) ? 1 : 0;

        // trigger press stuff
        if (rightHandItemHandler.GetHeldObject() != null)
        {
            if (rightHandItemHandler.GetHeldObject().IsAutomatic())
            {
                if (inputTriggerPress.GetLastStateDown(SteamVR_Input_Sources.RightHand))
                    rightHandItemHandler.GetHeldObject().DoTriggerInteractionHold(true);
                else if (inputTriggerPress.GetLastStateUp(SteamVR_Input_Sources.RightHand))
                    rightHandItemHandler.GetHeldObject().DoTriggerInteractionHold(false);
            }

            if (!rightHandItemHandler.GetHeldObject().IsAutomatic() && inputTriggerPress.GetLastStateDown(SteamVR_Input_Sources.RightHand))
                rightHandItemHandler.GetHeldObject().DoTriggerInteraction();

        }
        if (inputTriggerPress.GetLastStateDown(SteamVR_Input_Sources.RightHand))
            anim.DoTriggerInteraction(true);

    }

    public void OnLeftTriggerPress(InputAction.CallbackContext context)
    {
        leftTriggerPress = context.ReadValue<Single>();
        // Debug.Log("Left Trigger press " + context.valueType);
    }

    public void OnLeftTriggerTouch(InputAction.CallbackContext context)
    {
        leftTriggerTouch = context.ReadValue<Single>();
        // Debug.Log("Left Trigger touch " + context.valueType);
    }

    public void OnLeftTriggerMove(InputAction.CallbackContext context)
    {
        leftTriggerMove = context.ReadValue<Single>();
        //Debug.Log("Left Trigger move " + context.valueType);
    }


    public void OnLeftGripPress(InputAction.CallbackContext context)
    {
        leftGripPress = context.ReadValue<Single>();
        // Debug.Log("Left Grip press " + context.valueType);
    }

    public void OnLeftGripMove(InputAction.CallbackContext context)
    {
        leftGripMove = context.ReadValue<Single>();
        //Debug.Log("Left Grip move " + context.valueType);
    }


    public void OnLeftThumbTouch(InputAction.CallbackContext context)
    {
        leftThumbTouch = context.ReadValue<Single>();
        // Debug.Log("Left Thumb touch " + context.valueType);
    }

    public float LeftTriggerDelta()
    {
        if (leftTriggerMove > 0 && leftTriggerTouch < 1)
            return leftTriggerMove;

        return (leftTriggerTouch + leftTriggerMove) / 2;
    }

    public float LeftGripDelta()
    {
        if (leftGripMove > 0 && leftGripPress < 1)
            return leftGripMove;

        return (leftGripPress + leftGripMove) / 2;
    }

    public float LeftThumbDelta() => leftThumbTouch;


    // right hand

    public void OnRightTriggerPress(InputAction.CallbackContext context)
    {
        rightTriggerPress = context.ReadValue<Single>();
        //if (context.performed)
            HandleRightTriggerPress(context);

    }

    public void OnRightTriggerTouch(InputAction.CallbackContext context)
    {
        rightTriggerTouch = context.ReadValue<Single>();
        //Debug.Log("Right Trigger touch " + rightTriggerTouch);
    }

    public void OnRightTriggerMove(InputAction.CallbackContext context)
    {
        rightTriggerMove = context.ReadValue<Single>();
        //Debug.Log("Right Trigger move " + rightTriggerMove);
    }


    public void OnRightGripPress(InputAction.CallbackContext context)
    {
        rightGripPress = context.ReadValue<Single>();
        //Debug.Log("Right Grip press " + rightGripPress);
    }

    public void OnRightGripMove(InputAction.CallbackContext context)
    {
        rightGripMove = context.ReadValue<Single>();
       // Debug.Log("Right Grip move " + rightGripMove);
    }


    public void OnRightThumbTouch(InputAction.CallbackContext context)
    {
        //Debug.Log("Right Thumb touch " + context.valueType);
        rightThumbTouch = context.ReadValue<Single>();
    }


    public float RightTriggerDelta()
    {
        if (rightTriggerMove > 0 && rightTriggerTouch < 1)
            return rightTriggerMove;

        return (rightTriggerTouch + rightTriggerMove) / 2;
    }

    public float RightGripDelta()
    {
        if (rightGripMove > 0 && rightGripPress < 1)
            return rightGripMove;

        return (rightGripPress + rightGripMove) / 2;
    }

    public float RightThumbDelta() => rightThumbTouch;


    private void HandleRightTriggerPress(InputAction.CallbackContext context)
    {
        if (rightHandItemHandler.GetHeldObject() != null )
        {
            if (rightHandItemHandler.GetHeldObject().IsAutomatic())
                if (context.started)
                    rightHandItemHandler.GetHeldObject().DoTriggerInteractionHold(true);
                else if (context.canceled)
                    rightHandItemHandler.GetHeldObject().DoTriggerInteractionHold(false);

            if (!rightHandItemHandler.GetHeldObject().IsAutomatic() && context.performed)
                rightHandItemHandler.GetHeldObject().DoTriggerInteraction();

        }
        if (context.performed)
            anim.DoTriggerInteraction(true);
    }


    public SteamVR_Action_Boolean GetInputTriggerPress() => inputTriggerPress;

}


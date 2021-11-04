using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR;
using Valve.VR;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private CharacterController playerController;

    [SerializeField]
    private GameObject trackingObject;

    [SerializeField]
    private Camera headCamera;

    [SerializeField]
    SteamVR_Action_Vector2 moveInput;
    [SerializeField]
    SteamVR_Action_Vector2 lookInput;

    private Vector3 lastHeadPos;

    private Vector2 moveDir;
    private Vector3 moveVelocity;
    private float lookAxis;
    private float nextTurn = 0;

    private float maxSpeed = 4;

    void Start()
    {
        lastHeadPos = headCamera.transform.position;
        trackingObject.transform.localPosition = new Vector3(-headCamera.transform.localPosition.x, 0, -headCamera.transform.localPosition.z);
    }

    
    void Update()
    {

        UpdateMovementAndLookInput();
        UpdateMovementAndLook();


    }

    public void FixedUpdate()
    {
        float dist = Vector3.Dot(headCamera.transform.localPosition, Vector3.up);
        playerController.height = Mathf.Max(.5f, dist);
        playerController.center = new Vector3(playerController.center.x, (headCamera.transform.localPosition - 0.5f * dist * Vector3.up).y, playerController.center.z );
        //playerController.transform.localPosition = headCamera.transform.localPosition - 0.5f * dist * Vector3.up;
    }

    private void UpdateMovementAndLook()
    {
        float limit = playerController.radius*2.5f;
        float limit2 = 0.05f;
        
        Vector3 campos = headCamera.transform.localPosition;
        
        Vector3 headMove = headCamera.transform.position - lastHeadPos;
        headMove.y = 0;

        Vector3 offset = headCamera.transform.position - playerController.transform.position;
        offset.y = 0;

        Vector3 offset_x = offset * 1;
        offset_x.z = 0;

        Vector3 offset_z = offset * 1;
        offset_z.x = 0;

        float dist_x = offset_x.magnitude;
        float dist_z = offset_z.magnitude;

        Vector3 dir_forward = offset.normalized;

        Vector3 new_localPos = trackingObject.transform.localPosition;
        float view_dot = Vector3.Dot(headCamera.transform.forward, dir_forward);
        bool changed = false;

        if (view_dot > 0)
        {
            if (dist_x > limit)
            {
                new_localPos.x = -campos.x + limit * (view_dot > 0 ? 1 : -1);
                changed = true;
            }
            if (dist_z > limit)
            {
                new_localPos.z = -campos.z + limit * (view_dot > 0 ? 1 : -1);
                changed = true;
            }
        }
        else
        {
            new_localPos.z = -campos.z;
            new_localPos.x = -campos.x;
            changed = true;
        }
          
        if (changed)
            trackingObject.transform.localPosition = new_localPos;


        float x = moveDir.x;
        float z = moveDir.y;

        Vector3 camForw = headCamera.transform.forward;
        camForw.y = 0;
        camForw.Normalize();

        Vector3 camRight = headCamera.transform.right;
        camRight.y = 0;
        camRight.Normalize();

        Vector3 moveInput = camRight * x + camForw * z;

        moveVelocity.x *= 0.8f;
        moveVelocity.z *= 0.8f;

        moveVelocity += moveInput * (maxSpeed / 3);

        Vector3 moveClamp = moveVelocity;
        moveClamp.y = 0;
        moveClamp = moveClamp.normalized * Mathf.Min(moveClamp.magnitude, maxSpeed);

        moveVelocity.x = moveClamp.x;
        moveVelocity.z = moveClamp.z;

        moveVelocity.y += playerController.isGrounded ? 0 : -1;
        moveVelocity.y = playerController.isGrounded ? 0 : Mathf.Clamp(moveVelocity.y, -9, 10);

        playerController.Move(moveVelocity * Time.deltaTime + headMove);

        // smooth
        /*float yaw = 100.0f * lookAxis;
        transform.Rotate(0, yaw * Time.deltaTime, 0);*/

        if (nextTurn <= Time.time && Mathf.Abs(lookAxis) > 0.75f)
        {
            float yaw = 35f * (lookAxis > 0 ? 1 : -1);
            transform.Rotate(0, yaw, 0);
            nextTurn = Time.time + 0.4f;
        }

        lastHeadPos = headCamera.transform.position;
    }

    public void ResetCameraMove()
    {
        lastHeadPos = headCamera.transform.position;
    }

    private void UpdateMovementAndLookInput()
    {
        moveDir = moveInput.GetAxis(SteamVR_Input_Sources.Any);
        lookAxis = lookInput.GetAxis(SteamVR_Input_Sources.Any).x;
    }

    public void OnMovement( InputAction.CallbackContext context )
    {
        //moveDir = context.ReadValue<Vector2>();
    }

    public void OnLook(InputAction.CallbackContext context)
    {
       //lookAxis = context.ReadValue<Vector2>().x;
    }
}

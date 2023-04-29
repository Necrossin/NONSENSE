using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR;
using Valve.VR;

public class PlayerMovement : MonoBehaviour
{
    [Header("Player Controller")]
    [SerializeField]
    private CharacterController playerController;

    [Header("Camera Management")]
    [SerializeField]
    private GameObject trackingObject;

    [SerializeField]
    private Camera headCamera;

    [SerializeField]
    private Transform headTracker;

    [SerializeField]
    private Transform cameraAnchor;

    [Header("Movement Input Management")]
    [SerializeField]
    SteamVR_Action_Vector2 moveInput;
    [SerializeField]
    SteamVR_Action_Vector2 lookInput;
    [SerializeField]
    SteamVR_Action_Boolean jumpInput;

    private Vector3 lastHeadPos;

    private Vector2 moveDir;
    private Vector3 moveVelocity;

    private float lookAxis;
    private float nextTurn = 0;
    private float maxSpeed = 4;

    private float jumpHeight = 1f;
    private float gravityValue = -9.81f;

    void Start()
    {
        lastHeadPos = cameraAnchor.position;
        trackingObject.transform.localPosition = new Vector3(-headTracker.localPosition.x, 0, -headTracker.localPosition.z);
    }

    
    void Update()
    {
        UpdateMovementAndLookInput();
        UpdateMovementAndLook();
    }

    public void FixedUpdate()
    {
        float dist = Vector3.Dot(headTracker.localPosition, Vector3.up);

        playerController.height = Mathf.Max(.5f, dist);
        playerController.center = new Vector3(playerController.center.x, (headTracker.localPosition - 0.5f * dist * Vector3.up).y, playerController.center.z);
    }

    private void UpdateMovementAndLook()
    {
        float limit = playerController.radius * 2f;

        Vector3 camForw = headTracker.forward;
        camForw.y = 0;
        camForw.Normalize();

        Vector3 localCamForw = transform.InverseTransformDirection(camForw);

        Vector3 camRight = headTracker.right;
        camRight.y = 0;
        camRight.Normalize();

        Vector3 localCamRight = transform.InverseTransformDirection(camRight);

        float view_pitch = headTracker.rotation.eulerAngles.x;
        if (view_pitch >= 230)
            view_pitch = (360 - view_pitch) * -1;
        
        // when looking up - lock character controller under the camera, instead of pushing forward
        float shift_z = Mathf.Clamp(view_pitch / 30, 0, 1);

        float view_roll = headTracker.rotation.eulerAngles.z;
        if (view_roll >= 180)
            view_roll = (360 - view_roll) * -1;

        float shift_x = Mathf.Clamp(view_roll / 30, -1, 1);

        // camera uses world coordinates
        Vector3 new_anchorPos = new Vector3(0, 0, 0);
        new_anchorPos -= camForw * (limit * shift_z + 0.0f);
        new_anchorPos += camRight * limit * shift_x;

        cameraAnchor.position = headTracker.position + new_anchorPos;

        Vector3 headMove = cameraAnchor.position - lastHeadPos;
        headMove.y = 0;

        Vector3 campos = headTracker.localPosition;

        // tracking thingy uses local coordinates
        Vector3 new_localPos = new Vector3(-campos.x, 0, -campos.z);
        new_localPos += localCamForw * (limit * shift_z + 0.0f);
        new_localPos -= localCamRight * limit * shift_x;

        trackingObject.transform.localPosition = new_localPos;

        float x = moveDir.x;
        float z = moveDir.y;

        Vector3 moveInput = camRight * x + camForw * z;

        moveVelocity.x *= 0.8f;
        moveVelocity.z *= 0.8f;

        moveVelocity += moveInput * (maxSpeed / 3);

        Vector3 moveClamp = moveVelocity;
        moveClamp.y = 0;
        moveClamp = moveClamp.normalized * Mathf.Min(moveClamp.magnitude, maxSpeed);

        moveVelocity.x = moveClamp.x;
        moveVelocity.z = moveClamp.z;

        bool isGrounded = playerController.isGrounded;

        //moveVelocity.y += isGrounded ? 0 : -1;
        //moveVelocity.y = isGrounded ? 0 : Mathf.Clamp(moveVelocity.y, -9, 10);

        if (isGrounded)
            moveVelocity.y = 0;

        // jump
        if (isGrounded && jumpInput.GetStateDown(SteamVR_Input_Sources.Any))
        {
            moveVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
        }

        moveVelocity.y += gravityValue * Time.deltaTime * 2;

        playerController.Move(moveVelocity * Time.deltaTime + headMove);

        //TODO: add an option for smooth turning
        /*float yaw = 100.0f * lookAxis;
        transform.Rotate(0, yaw * Time.deltaTime, 0);*/

        if (nextTurn <= Time.time && Mathf.Abs(lookAxis) > 0.75f)
        {
            float yaw = 35f * (lookAxis > 0 ? 1 : -1);
            transform.Rotate(0, yaw, 0);
            nextTurn = Time.time + 0.2f;
        }

        lastHeadPos = cameraAnchor.position;
    }

    public void ResetCameraMove()
    {
        lastHeadPos = cameraAnchor.position;
    }

    private void UpdateMovementAndLookInput()
    {
        moveDir = moveInput.GetAxis(SteamVR_Input_Sources.Any);
        lookAxis = lookInput.GetAxis(SteamVR_Input_Sources.Any).x;
    }


}

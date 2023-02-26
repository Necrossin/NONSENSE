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
    private Transform headTracker;

    [SerializeField]
    private Transform cameraAnchor;

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

    /*public Vector3 HandleCameraClip()
    {
        bool hasCollision = Physics.CheckSphere(headCamera.transform.position, cameraCollisionRadius, 1 << 9, QueryTriggerInteraction.Ignore);

        if (!hasCollision)
        {
            return Vector3.zero;
        }
            

        Vector3 collisionOffset = Vector3.zero;

        RaycastHit hitInfo;
        Vector3 closestPoint;
        Vector3 diff;

        //bool hasCollision = Physics.SphereCast(headCamera.transform.position, cameraCollisionRadius, headCamera.transform.forward, out hitInfo, 0.1f, 1 << 9, QueryTriggerInteraction.Ignore);

        // forward
        hasCollision = Physics.Raycast(headCamera.transform.position, headCamera.transform.forward, out hitInfo, cameraCollisionRadius, 1 << 9, QueryTriggerInteraction.Ignore);

        if (hasCollision)
        {
            closestPoint = hitInfo.point;
            diff = headCamera.transform.position - closestPoint;
            collisionOffset += diff.normalized * Mathf.Clamp(cameraCollisionRadius - diff.magnitude, 0, cameraCollisionRadius);
        }

        // left and right
        for (int i = 0; i < 2; i++)
        {
            hasCollision = Physics.Raycast(headCamera.transform.position, headCamera.transform.right * ( i == 0 ? 1 : -1 ), out hitInfo, cameraCollisionRadius, 1 << 9, QueryTriggerInteraction.Ignore);

            if (hasCollision)
            {
                closestPoint = hitInfo.point;
                diff = headCamera.transform.position - closestPoint;
                collisionOffset += diff.normalized * Mathf.Clamp(cameraCollisionRadius - diff.magnitude, 0, cameraCollisionRadius);
            }

        }

        // up and down

        for (int i = 0; i < 2; i++)
        {
            hasCollision = Physics.Raycast(headCamera.transform.position, headCamera.transform.up * (i == 0 ? 1 : -1), out hitInfo, cameraCollisionRadius * 1.2f, 1 << 9, QueryTriggerInteraction.Ignore);

            if (hasCollision)
            {
                closestPoint = hitInfo.point;
                diff = headCamera.transform.position - closestPoint;
                collisionOffset += diff.normalized * Mathf.Clamp(cameraCollisionRadius - diff.magnitude, 0, cameraCollisionRadius);
            }

        }

        //Debug.Log(collisionOffset);

        return collisionOffset;
    }*/

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

        // old
        /*Vector3 headMove = headCamera.transform.position - lastHeadPos;
        headMove.y = 0;

        Vector3 campos = headCamera.transform.localPosition;
        trackingObject.transform.localPosition = new Vector3(-campos.x, 0, -campos.z);*/

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

        moveVelocity.y += playerController.isGrounded ? 0 : -1;
        moveVelocity.y = playerController.isGrounded ? 0 : Mathf.Clamp(moveVelocity.y, -9, 10);

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

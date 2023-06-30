using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShoulderAdjustment : MonoBehaviour
{
    [SerializeField]
    Transform cameraTransform;

    [SerializeField]
    CharacterController playerController;

    Transform playerTransform;

    [SerializeField]
    Transform leftHandTransform;

    [SerializeField]
    Transform rightHandTransform;

    [SerializeField]
    float yOffset = 0.22f;

    void Start()
    {
        playerTransform = playerController.transform;
    }

    
    void Update()
    {

        float view_pitch = cameraTransform.rotation.eulerAngles.x;
        if (view_pitch >= 230)
            view_pitch = (360 - view_pitch);

        float shift_z = Mathf.Clamp(view_pitch / 30, 0, 1);

        Vector3 newPos = cameraTransform.position;
        newPos -= cameraTransform.up * yOffset;

        //Vector3 forward = cameraTransform.forward;
        //forward.y = 0;

        //newPos -= forward * (shift_z) * yOffset;

        //newPos -= cameraTransform.forward * yOffset * shift_z * 0.5f;
        //newPos.y = cameraTransform.position.y + yOffset;
        // newPos.x = playerTransform.position.x;
        //newPos.z = playerTransform.position.z;

        //newPos -= GetAverageDirection() * yOffset;

        transform.position = newPos;

        transform.LookAt(transform.position + GetAverageDirection() * 1);
    }

    private Vector3 GetAverageDirection()
    {
        Vector3 leftDir = (leftHandTransform.position - transform.position).normalized;
        Vector3 rightDir = (rightHandTransform.position - transform.position).normalized;
        Vector3 camDir = cameraTransform.forward;

        Vector3 result = (leftDir + rightDir + camDir).normalized;
        result.y = 0;

        return result;
    }
}

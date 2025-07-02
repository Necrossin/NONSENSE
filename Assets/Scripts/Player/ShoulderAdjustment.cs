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
    Transform leftHandPole;

    [SerializeField]
    Transform leftHandWrist;

    [SerializeField]
    Transform rightHandTransform;

    [SerializeField]
    Transform rightHandPole;

    [SerializeField]
    Transform rightHandWrist;

    [SerializeField]
    float yOffset = 0.22f;

    Vector3 originalLeftPolePos, originalRightPolePos;

    float limitX = 0f;
    float limitY = 0;


    void Start()
    {
        playerTransform = playerController.transform;

        if (rightHandPole != null)
            originalRightPolePos = rightHandPole.localPosition;

        if (leftHandPole != null)
            originalLeftPolePos = leftHandPole.localPosition;
    }

    
    void Update()
    {

        float view_pitch = cameraTransform.rotation.eulerAngles.x;
        if (view_pitch >= 230)
            view_pitch = (360 - view_pitch);

        float shift_z = Mathf.Clamp(view_pitch / 30, 0, 1);

        Vector3 newPos = cameraTransform.position;
        newPos -= cameraTransform.up * yOffset;

        transform.position = newPos;

        transform.LookAt(transform.position + GetAverageDirection() * 1);
    }

    private void LateUpdate()
    {
       
    }

    void PoleUpdate()
    {
        if (rightHandPole != null && rightHandWrist != null)
        {
            rightHandPole.localPosition = originalRightPolePos;

            Vector3 new_pos = rightHandWrist.transform.up * -3;
            rightHandPole.transform.position = new_pos;

            rightHandPole.transform.localPosition = new Vector3(Mathf.Clamp(rightHandPole.transform.localPosition.x, originalRightPolePos.x - limitX, originalRightPolePos.x + limitX), Mathf.Clamp(rightHandPole.transform.localPosition.x, originalRightPolePos.y - limitY, originalRightPolePos.y + limitY), originalRightPolePos.z);

            //Debug.DrawLine(rightHandWrist.transform.position, rightHandPole.transform.position, Color.red, 0.1f, true);
        }

        if (leftHandPole != null && leftHandWrist != null)
        {
            leftHandPole.localPosition = originalLeftPolePos;

            Vector3 new_pos = leftHandWrist.transform.up * -3;
            leftHandPole.transform.position = new_pos;

            leftHandPole.transform.localPosition = new Vector3(Mathf.Clamp(leftHandPole.transform.localPosition.x, originalLeftPolePos.x - limitX, originalLeftPolePos.x + limitX), Mathf.Clamp(leftHandPole.transform.localPosition.x, originalLeftPolePos.y - limitY, originalLeftPolePos.y + limitY), originalLeftPolePos.z);

            //Debug.DrawLine(leftHandWrist.transform.position, leftHandPole.transform.position, Color.green, 0.1f, true);
        }
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

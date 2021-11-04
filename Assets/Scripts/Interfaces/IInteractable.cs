using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable
{
    Transform GetRelativeTransform();
    void SetKinematic(bool km);
    void OnGrab();
    void OnDrop();
    void MoveWithChild(Transform transform, Vector3 childPosition, Quaternion childRotation, Vector3 childTargetPosition, Quaternion childTargetRotation);
    void GetMoveWithChildPositionAndRotation(Transform transform, Vector3 childPosition, Quaternion childRotation, Vector3 childTargetPosition, Quaternion childTargetRotation, out Vector3 targetPosition, out Quaternion targetRotation);

    GameObject GetGameObject();
    Rigidbody GetRigidbody();

    int GetHoldtype();

    void DoTriggerInteraction();
    void DoTriggerInteractionHold( bool start );

    void PullTowards(Transform goal, float power);

    bool IsAutomatic();

    void SetHandObject(GameObject hand);
    void ClearHandObject();

    void SetOwnerObject(GameObject player);
    void ClearOwnerObject();
    bool CanBeDropped();

    bool IsGrabbable();

}

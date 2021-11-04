using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseInteractable : MonoBehaviour, IInteractable
{

    [SerializeField]
    private Transform relativeTransform;
    private Rigidbody rb;

    protected int itemHoldtype = (int)Holdtype.TestItem;

    private Vector3 lastPullPos;
    private Transform lastPullTransform;
    private float pullTime = 0f;
    private float pullPower = 0f;

    private float lastDist = 1f;

    protected bool auto = false;
    protected bool isHeld = false;

    protected bool isGrabbable = true;

    private GameObject handObj;
    private GameObject playerObj;

    public enum Holdtype
    {
        None,
        TestItem,
        Pistol,
        Shotgun
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        OnStart();
    }

    protected virtual void OnStart()
    {
    }

    void Update()
    {
        OnUpdate();
    }

    protected virtual void OnUpdate()
    {
    }

    private void FixedUpdate()
    {
        if (pullTime > Time.time && lastPullTransform != null)
        {
            
            Vector3 dir = (lastPullTransform.position - transform.position).normalized;

            rb.MovePosition(transform.position + dir * pullPower * Time.fixedDeltaTime);

        }
        else
            if (rb.isKinematic && !isHeld && IsGrabbable())
                rb.isKinematic = false;
        
        OnFixedUpdate();
    }

    protected virtual void OnFixedUpdate()
    {
    }

    public virtual void OnGrab()
    {
        pullTime = 0;
        isHeld = true;
    }

    public virtual void OnDrop()
    {
        isHeld = false;
    }

    public void SetKinematic( bool km )
    {
        rb.isKinematic = km;
    }

    public Transform GetRelativeTransform()
    {
        return relativeTransform;
    }


    public void MoveWithChild( Transform transform, Vector3 childPosition, Quaternion childRotation, Vector3 childTargetPosition, Quaternion childTargetRotation )
    {
        GetMoveWithChildPositionAndRotation(transform, childPosition, childRotation, childTargetPosition, childTargetRotation, out Vector3 position, out Quaternion rotation);
        transform.SetPositionAndRotation(position, rotation);
    }

    public void GetMoveWithChildPositionAndRotation( Transform transform, Vector3 childPosition, Quaternion childRotation, Vector3 childTargetPosition, Quaternion childTargetRotation, out Vector3 targetPosition, out Quaternion targetRotation )
    {
        Quaternion rotation = childTargetRotation * Quaternion.Inverse(childRotation);
        targetPosition = childTargetPosition + rotation * (transform.position - childPosition);
        targetRotation = rotation * transform.rotation;
    }

    public GameObject GetGameObject()
    {
        return this.gameObject;
    }

    public Rigidbody GetRigidbody()
    {
        return rb;
    }

    public int GetHoldtype()
    {
        return itemHoldtype;
    }

    public virtual void DoTriggerInteraction()
    {

    }

    public virtual void DoTriggerInteractionHold( bool start )
    {

    }

    public void PullTowards(Transform goal, float power)
    {
        //lastPullPos = goal.position;
        lastPullTransform = goal;
        pullPower = power;
        pullTime = Time.time + 0.4f;

        rb.isKinematic = true;

        //lastDist = (lastPullPos - transform.position).magnitude;
        //lastDist = Mathf.Max(0.001f, lastDist);
    }

    public bool IsAutomatic()
    {
        return auto;
    }

    public void SetHandObject(GameObject hand)
    {
        handObj = hand;
    }

    protected GameObject GetHandObject()
    {
        return handObj;
    }

    public void ClearHandObject()
    {
        handObj = null;
    }

    public void SetOwnerObject(GameObject player)
    {
        playerObj = player;
    }

    public void ClearOwnerObject()
    {
        playerObj = null;
    }

    protected GameObject GetOwnerObject()
    {
        return playerObj;
    }

    public virtual bool CanBeDropped()
    {
        return true;
    }

    public virtual bool IsGrabbable()
    {
        return isGrabbable;
    }
}

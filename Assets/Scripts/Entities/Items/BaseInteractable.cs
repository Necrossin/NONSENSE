using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseInteractable : MonoBehaviour, IInteractable
{

    [SerializeField]
    private Transform relativeTransform;
    private Rigidbody rb;

    [SerializeField]
    protected Animator animController;

    protected int itemHoldtype = (int)Holdtype.TestItem;

    private Vector3 lastPullPos;
    private Transform lastPullTransform;
    private float pullTime = 0f;
    private float pullPower = 0f;

    private float lastDist = 1f;

    protected bool auto = false;
    protected bool isHeld = false;

    protected bool isHeldByEnemy = false;

    protected bool isGrabbable = true;

    [SerializeField]
    protected List<Transform> fingerBones;

    private GameObject handObj;
    private GameObject playerObj;

    //protected AmmoCounter 

    public enum Holdtype
    {
        None,
        TestItem,
        Pistol,
        Shotgun
    }

    protected void Start()
    {
        rb = GetComponent<Rigidbody>();
        OnStart();

        Globals.TryResolvingAudioProbes(gameObject);
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
        // just to be sure
        SetHeldByEnemy(false);
        isHeld = false;
    }

    public void SetKinematic(bool km) => rb.isKinematic = km;

    public Transform GetRelativeTransform() => relativeTransform;


    public void MoveWithChild( Transform transform, Vector3 childPosition, Quaternion childRotation, Vector3 childTargetPosition, Quaternion childTargetRotation )
    {
        GetMoveWithChildPositionAndRotation(transform, childPosition, childRotation, childTargetPosition, childTargetRotation, out Vector3 position, out Quaternion rotation);
        transform.SetPositionAndRotation(position, rotation);
    }

    public void MoveWithChild(Vector3 childTargetPosition, Quaternion childTargetRotation)
    {
        GetMoveWithChildPositionAndRotation(transform, relativeTransform.position, relativeTransform.rotation, childTargetPosition, childTargetRotation, out Vector3 position, out Quaternion rotation);
        transform.SetPositionAndRotation(position, rotation);
    }

    public void GetMoveWithChildPositionAndRotation( Transform transform, Vector3 childPosition, Quaternion childRotation, Vector3 childTargetPosition, Quaternion childTargetRotation, out Vector3 targetPosition, out Quaternion targetRotation )
    {
        Quaternion rotation = childTargetRotation * Quaternion.Inverse(childRotation);
        targetPosition = childTargetPosition + rotation * (transform.position - childPosition);
        targetRotation = rotation * transform.rotation;
    }

    public List<Transform> GetFingerBones() => fingerBones;
    public void SetFingerBones(int ind, Transform bone) => fingerBones.Insert(ind, bone);

    public GameObject GetGameObject() => gameObject;

    public Rigidbody GetRigidbody() => (rb != null) ? rb : GetComponent<Rigidbody>();

    public int GetHoldtype() => itemHoldtype;

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

    public bool IsAutomatic() => auto;

    public void SetHandObject(GameObject hand) => handObj = hand;

    protected GameObject GetHandObject() => handObj;

    public void ClearHandObject() => handObj = null;

    public void SetOwnerObject(GameObject player) => playerObj = player;

    public void ClearOwnerObject() => playerObj = null;

    protected GameObject GetOwnerObject() => playerObj;

    public virtual bool CanBeDropped() => true;

    public virtual bool IsGrabbable() => isGrabbable;

    public void SetHeldByEnemy(bool enemy) => isHeldByEnemy = enemy;
    public bool IsHeldByEnemy() => isHeldByEnemy;
}

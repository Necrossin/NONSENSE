using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class HandCollision : MonoBehaviour
{

    [SerializeField]
    private ControllerInput inp;
    private HandAnimations anim;

    [SerializeField]
    private bool primaryHand = true;

    [SerializeField]
    private VisualEffect pullVFX;
    [SerializeField]
    private VFX_ItemPull pullVFXScript;

    [SerializeField]
    private GameObject handObject;
    [SerializeField]
    private GameObject playerObject;

    private List<IInteractable> touchedObjects = new List<IInteractable>();

    private IInteractable heldObject;
    [SerializeField]
    private ConfigurableJoint joint;

    [SerializeField]
    private Transform parentTransform;

    [SerializeField]
    private Transform grabDotTransform;

    [SerializeField]
    private LayerMask grabMask;

    [SerializeField]
    private BoxCollider itemHullCollider;

    [SerializeField]
    SteamVR_Action_Vibration hapticAction;

    private Vector3 defHullCenter;
    private Vector3 defHullSize;
    private Quaternion defHullRotation;
    private Vector3 defHullPosition;

    // local velocity estimator (for movement relative to player)
    private VelocityEstimatorParent velEstimatorLocal;

    // normal velocity estimator (for throwing etc)
    private VelocityEstimator velEstimator;



    private float nextPull = 0f;
    private bool playPull = false;

    void Start()
    {
        velEstimator = GetComponent<VelocityEstimator>();
        velEstimatorLocal = GetComponent<VelocityEstimatorParent>();
        anim = GetComponentInParent<HandAnimations>();

        defHullCenter = itemHullCollider.center * 1;
        defHullSize = itemHullCollider.size * 1;
        defHullRotation = itemHullCollider.transform.localRotation;
        defHullPosition = itemHullCollider.transform.localPosition;
 
    }

    // TODO: separate grab and pull cooldowns, because this is janky
    void FixedUpdate()
    {
        // pick the object
        if (heldObject == null)
        {
            //if (inp.RightGripDelta() >= 0.33 )
            //{
                if ( touchedObjects.Count > 0 )
                {
                    int closestObject = 0;

                    for ( int i = 0; i < touchedObjects.Count; i++ )
                    {
                        //if (!touchedObjects[i].IsGrabbable())
                            //continue;

                        float dot1 = Vector3.Dot((touchedObjects[i].GetRigidbody().transform.position - grabDotTransform.position).normalized, grabDotTransform.forward);
                        float dot2 = Vector3.Dot((touchedObjects[closestObject].GetRigidbody().transform.position - grabDotTransform.position).normalized, grabDotTransform.forward);

                        if( dot1 > dot2 )
                        {
                            closestObject = i;
                        }
                    }

                    float dot = Vector3.Dot((touchedObjects[closestObject].GetRigidbody().transform.position - grabDotTransform.position).normalized, grabDotTransform.forward);
                    float dist = (touchedObjects[closestObject].GetRigidbody().transform.position - grabDotTransform.position).magnitude;

                    if (!Physics.Raycast(grabDotTransform.position, touchedObjects[closestObject].GetRigidbody().transform.position - grabDotTransform.position, dist, grabMask) && dot >= 0.65)
                    {

                        if (dist > 0.2 && nextPull <= Time.time)
                            DoPullFX(touchedObjects[closestObject].GetGameObject().transform);
                        else
                           StopPullFX();

                        if (inp.RightGripDelta() >= 0.2)
                        {
                            // it is close, grab it
                            if (dist < ( 0.18 ) )
                            {
                                GrabObject(touchedObjects[closestObject]);
                            }
                            // its closest - pull it
                            else
                            {
                                if (nextPull <= Time.time)
                                {
                                    touchedObjects[closestObject].PullTowards(transform, 6);

                                    if (dist > ( 0.5 ) )
                                    {
                                        //DoPullFX(touchedObjects[closestObject].GetGameObject().transform);
                                    }

                                    hapticAction.Execute(0, 0.07f, 20, 5, primaryHand ? SteamVR_Input_Sources.RightHand : SteamVR_Input_Sources.LeftHand);

                                    nextPull = Time.time + 0.8f;
                                }
                            }
                        }
                    }
                }
                else
                {
                    StopPullFX();
                }
            //}           
        }
        else
        {

            if (inp.RightGripDelta() < 0.2)
            {
                ReleaseObject();
            }
        }
    }

    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        var otherClass = other.attachedRigidbody.GetComponentInParent<IInteractable>();

        AddHeldObject(otherClass);
    }

    private void OnTriggerStay(Collider other)
    {
        var otherClass = other.attachedRigidbody.GetComponentInParent<IInteractable>();

        AddHeldObject(otherClass);
    }

    private void OnTriggerExit(Collider other)
    {
        var otherClass = other.attachedRigidbody.GetComponentInParent<IInteractable>();

        RemoveHeldObject(otherClass);
    }

    public void GrabObject(IInteractable obj)
    {
        touchedObjects.Remove(obj);
        heldObject = obj;
        obj.GetGameObject().transform.SetParent(transform);
        obj.MoveWithChild(obj.GetRigidbody().transform, obj.GetRelativeTransform().position, obj.GetRelativeTransform().rotation, transform.position, transform.rotation);
        joint.connectedBody = obj.GetRigidbody();
        obj.GetRigidbody().isKinematic = true;
        SetItemHull(obj.GetGameObject());
        obj.SetHandObject(handObject);
        obj.SetOwnerObject(playerObject);
        obj.OnGrab();
        nextPull = Time.time + 1f;

        anim.SetHoldtype(primaryHand, obj.GetHoldtype());

        velEstimator.BeginEstimatingVelocity();
        velEstimatorLocal.BeginEstimatingVelocity();

        hapticAction.Execute(0, 0.1f, 20, 50, primaryHand ? SteamVR_Input_Sources.RightHand : SteamVR_Input_Sources.LeftHand);

        StopPullFX();
    }

    public void ReleaseObject()
    {
        if (!heldObject.CanBeDropped()) 
            return;
        
        nextPull = Time.time + 1f;

        velEstimator.FinishEstimatingVelocity();
        velEstimatorLocal.FinishEstimatingVelocity();
        heldObject.GetGameObject().transform.SetParent(null);
        joint.connectedBody = null;
        heldObject.OnDrop();
        heldObject.GetRigidbody().isKinematic = false;
        heldObject.GetRigidbody().AddForce(velEstimator.GetVelocityEstimate(), ForceMode.Impulse);
        heldObject.GetRigidbody().AddTorque(velEstimator.GetAngularVelocityEstimate(), ForceMode.Impulse);
        RemoveHeldObject(heldObject);
        heldObject.ClearHandObject();
        heldObject.ClearOwnerObject();
        heldObject = null;
        
        DisableItemHull();

        anim.SetHoldtype(primaryHand, 0);
    }

    private void AddHeldObject(IInteractable obj)
    {
        if (obj != null && heldObject == null && !touchedObjects.Contains(obj) && obj.IsGrabbable())
        {
            touchedObjects.Add(obj);
        }
    }

    private void RemoveHeldObject( IInteractable obj )
    {
        if (obj != null && touchedObjects.Contains(obj))
        {
            touchedObjects.Remove(obj);
        }
    }

    public IInteractable GetHeldObject()
    {
        return heldObject;
    }

    private void SetItemHull( GameObject item )
    {
        itemHullCollider.enabled = true;

        itemHullCollider.transform.localRotation = item.transform.localRotation;
        itemHullCollider.transform.localPosition = item.transform.localPosition;

        var colliders = item.GetComponentsInChildren<BoxCollider>();

        foreach (var collider in colliders)
        {
            if (collider.isTrigger)
            {
                itemHullCollider.size = Vector3.Scale(collider.size, collider.transform.localScale);
                itemHullCollider.center = collider.center;
                break;
            }
        }

    }

    private void DisableItemHull()
    {
        itemHullCollider.enabled = false;

        itemHullCollider.size = defHullSize;
        itemHullCollider.center = defHullCenter;
        itemHullCollider.transform.localRotation = defHullRotation;
        itemHullCollider.transform.localPosition = defHullPosition;

    }

    private void DoPullFX(Transform obj)
    {
        if ( pullVFX != null && pullVFXScript != null )
        {
            if (!playPull)
                pullVFXScript.SetTransforms(transform, transform);

            pullVFXScript.SetTransforms(transform, obj);

            if (!playPull && ( nextPull < Time.time ) )
            {
                pullVFX.Play();
                playPull = true;
            }
                
        }
    }

    private void StopPullFX()
    {
        if (pullVFX != null && playPull && pullVFXScript != null)
        {
            pullVFX.Stop();
            playPull = false;
        }
    }
}

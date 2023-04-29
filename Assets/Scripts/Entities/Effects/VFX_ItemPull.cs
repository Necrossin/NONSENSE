using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
using UnityEngine.VFX.Utility;

public class VFX_ItemPull : MonoBehaviour
{

    private VisualEffect effect;

    [SerializeField]
    private Transform startTransformSelf;
    [SerializeField]
    private Transform endTransformSelf;

    private Transform startTransform;
    private Transform endTransform;

    private GameObject lastObj;
    private BoxCollider objCollider;

    ExposedProperty startProp, endProp, boundsCenterProp, boundsSizeProp;

    void Start()
    {
        effect = GetComponent<VisualEffect>();

        startProp = "LineZStart";
        endProp = "LineZEnd";
        boundsCenterProp = "Bounds Center";
        boundsSizeProp = "Bounds Size";
    }

    void Update()
    {
        if (effect != null && startTransform != null && endTransform != null)
        {
            if (objCollider != null)
            {

                //startTransformSelf.position = objCollider.bounds.ClosestPoint(endTransform.position);
                startTransformSelf.position = startTransform.position;

                effect.SetVector3(startProp, objCollider.transform.position + objCollider.transform.forward * objCollider.size.z * 0.5f);
                effect.SetVector3(endProp, objCollider.transform.position - objCollider.transform.forward * objCollider.size.z * 0.5f);

            }
            else
            {
                startTransformSelf.position = startTransform.position;
            }

            if (lastObj != null)
            {
                
                //gameObject.transform.localRotation = lastObj.transform.rotation;
            }

            //effect.SetVector3("Item Angle", startTransform.eulerAngles);
            //startTransformSelf.localRotation = startTransform.rotation;    

            endTransformSelf.position = endTransform.position;
        }
    }

    public void SetTransforms(Transform start, Transform end)
    {
        startTransform = start;
        endTransform = end;
    }

    public void SetObject( GameObject obj )
    {
        if (obj == null)
        {
            lastObj = null;
            objCollider = null;
            return;
        }

        if (lastObj == obj)
            return;

        lastObj = obj;

        var colliders = obj.GetComponentsInChildren<BoxCollider>();

        foreach (var collider in colliders)
        {
            if (collider.isTrigger)
            {
                objCollider = collider;

                effect.SetVector3(boundsCenterProp, objCollider.center);
                effect.SetVector3(boundsSizeProp, objCollider.size);
                break;
            }
        }
    }
}

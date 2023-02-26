using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

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

    void Start()
    {
        effect = GetComponent<VisualEffect>();
    }

    void Update()
    {
        if (effect != null && startTransform != null && endTransform != null)
        {
            if (objCollider != null)
            {

                //startTransformSelf.position = objCollider.bounds.ClosestPoint(endTransform.position);
                startTransformSelf.position = startTransform.position;

                effect.SetVector3("LineZStart", objCollider.transform.position + objCollider.transform.forward * objCollider.size.z * 0.5f);
                effect.SetVector3("LineZEnd", objCollider.transform.position - objCollider.transform.forward * objCollider.size.z * 0.5f);

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

                effect.SetVector3("Bounds Center", objCollider.center);
                effect.SetVector3("Bounds Size", objCollider.size);
                break;
            }
        }
    }
}

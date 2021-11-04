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

    void Start()
    {
        effect = GetComponent<VisualEffect>();
    }

    void Update()
    {
        if (effect != null && startTransform != null && endTransform != null)
        {
            startTransformSelf.position = startTransform.position;
            endTransformSelf.position = endTransform.position;
        }
    }

    public void SetTransforms(Transform start, Transform end)
    {
        startTransform = start;
        endTransform = end;
    }

}

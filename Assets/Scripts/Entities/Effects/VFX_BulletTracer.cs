using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class VFX_BulletTracer : MonoBehaviour
{
    [SerializeField]
    private Transform endTransformSelf;

    public void SetHitPos(Vector3 end)
    {
        endTransformSelf.position = end;
    }
}

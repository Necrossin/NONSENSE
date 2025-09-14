using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TriggerCollisionEventMelee : MonoBehaviour
{

    //public UnityEvent OnTriggerCollision;
    private BaseMeleeWeapon parentWeapon;
    private BoxCollider thisCollider;
    void Start()
    {
        parentWeapon = GetComponentInParent<BaseMeleeWeapon>();
        thisCollider = GetComponent<BoxCollider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (parentWeapon != null && thisCollider != null)
        {
            Vector3 hitWorld = other.ClosestPointOnBounds(this.transform.position);
            Vector3 hitPos = thisCollider.ClosestPointOnBounds(hitWorld);
            Vector3 hitNormal = (this.transform.position - hitWorld).normalized;

            parentWeapon.OnEdgeTrigger(thisCollider, other, hitWorld, hitNormal);
        }    
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilitySlotHandler : MonoBehaviour
{
    public int slotIndex = 0;
    private HandAbilities abilityManager;

    void Start()
    {
        abilityManager = GetComponentInParent<HandAbilities>();
    }

    
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {    
        var otherClass = other.attachedRigidbody.GetComponent<IAbility>();

        if ( otherClass != null && !otherClass.IsAttached() && otherClass.GetSlotIndex() == slotIndex )
        {

            var hand = other.attachedRigidbody.GetComponentInParent<HandCollision>();

            if (hand != null)
                hand.ReleaseObject();

            otherClass.AttachAbility(abilityManager);

            other.transform.SetParent(transform);

            other.transform.position = transform.position;
            other.transform.rotation = transform.rotation;

            

            other.attachedRigidbody.isKinematic = true;

            //Debug.Log("enter");

        }

    }

    private void OnTriggerExit(Collider other)
    {
        
    }
}

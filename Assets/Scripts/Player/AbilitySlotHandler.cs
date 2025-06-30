using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilitySlotHandler : MonoBehaviour
{
    public int slotIndex = 0;
    private HandAbilities abilityManager;
    private bool isOccupied = false;

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

            var itemClass = other.attachedRigidbody.GetComponent<IInteractable>();

            if (itemClass != null)
            {
                itemClass.MoveWithChild(abilityManager.GetAbilitySyncTransform().position, abilityManager.GetAbilitySyncTransform().rotation);
            }
            else
            {
                other.transform.position = transform.position;
                other.transform.rotation = transform.rotation;
            }
            

            other.attachedRigidbody.isKinematic = true;

            isOccupied = true;
        }

    }

    private void OnTriggerExit(Collider other)
    {
        
    }

    public bool TryAttachAbility( Ability_Template abilityClass )
    {
        if (abilityClass != null && !abilityClass.IsAttached() && abilityClass.GetSlotIndex() == slotIndex)
        {
            var rb = abilityClass.gameObject.GetComponent<Rigidbody>();
            if (rb == null) return false;

            abilityClass.AttachAbility(abilityManager);

            abilityClass.gameObject.transform.SetParent(transform);

            var itemClass = abilityClass.gameObject.GetComponent<IInteractable>();

            if (itemClass != null)
            {
                itemClass.MoveWithChild(abilityManager.GetAbilitySyncTransform().position, abilityManager.GetAbilitySyncTransform().rotation);
            }
            else
            {
                abilityClass.gameObject.transform.position = transform.position;
                abilityClass.gameObject.transform.rotation = transform.rotation;
            }


            rb.isKinematic = true;

            isOccupied = true;

            return true;
        }
        return false;
    }

    public bool IsOccupied() => isOccupied;
}

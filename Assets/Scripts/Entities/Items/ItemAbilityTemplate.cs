using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemAbilityTemplate : BaseInteractable
{
    private IAbility abilityBase;
    protected override void OnStart()
    {
        abilityBase = GetComponent<IAbility>();
        //Debug.Log(abilityBase);
    }
    protected override void OnUpdate()
    {

    }

    public override bool IsGrabbable()
    {
        if (abilityBase != null && abilityBase.IsAttached())
            return false;

        return base.IsGrabbable();
    }
}


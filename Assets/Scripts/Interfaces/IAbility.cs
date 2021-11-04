using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAbility
{
    string GetName();

    void AttachAbility(HandAbilities parent);
    void DetachAbility();

    bool IsAttached();
    int GetSlotIndex();
}


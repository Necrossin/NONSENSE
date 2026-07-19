using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBlade : BaseMeleeWeapon
{
    // store these to animate blade bits
    private SkinnedMeshRenderer rend;
    private MaterialPropertyBlock prop;

    // todo, maybe use an override in case if I use Awake in the base code
    private void Awake()
    {
        rend = GetComponent<SkinnedMeshRenderer>();
        prop = new MaterialPropertyBlock();
    }


    public override bool IsGrabbable() => false;

}

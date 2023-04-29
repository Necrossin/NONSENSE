using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
using UnityEngine.VFX.Utility;

//[ExecuteInEditMode]
public class AmmoCounter : MonoBehaviour
{
    //[SerializeField]
    //Vector3 bulletScale = new Vector3(0.8f, 0.8f, 2);
    
    VisualEffect ammoVFX;
    BaseRangedWeapon weaponScript;

    ExposedProperty curAmmoProp, maxAmmoProp, shootDeltaProp;
    private int fireEvent;

    private float shootDelta = 1;
    private float shootTime = 0;
    private float shootDelay = 0.2f;


    void Start()
    {
        ammoVFX = GetComponent<VisualEffect>();
        weaponScript = GetComponentInParent<BaseRangedWeapon>();

        curAmmoProp = "Current Ammo";
        maxAmmoProp = "Max Ammo";
        shootDeltaProp = "Shoot Delta";
        //bulletScaleProp = "Bullet Scale";

        fireEvent = Shader.PropertyToID("OnFire");
    }
    
    void Update()
    {
        //ammoVFX.SetVector3(bulletScaleProp, bulletScale);
        
        if (ammoVFX == null || weaponScript == null)
            return;

        int test = Random.Range(2, 6);

        ammoVFX.SetInt(curAmmoProp, weaponScript.GetClip());
        ammoVFX.SetInt(maxAmmoProp, weaponScript.GetMaxClip());

        if (shootTime != 0)
        {
            shootDelta = Mathf.Clamp01(1 - (shootTime - Time.time) / shootDelay);
            ammoVFX.SetFloat(shootDeltaProp, shootDelta);
        }
    }

    public void PlayFireEffects()
    {
        if (ammoVFX == null)
            return;

        ammoVFX.SendEvent(fireEvent);
        shootTime = Time.time + shootDelay;
    }

    public VisualEffect GetAmmoVFX() => ammoVFX;
}

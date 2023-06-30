using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class SurfaceMaterial : MonoBehaviour
{

    public SurfaceMaterialTemplate materialInfo;

    private int objectLayer = 9;

    private void Start()
    {
        objectLayer = gameObject.layer;
        AddSurfaceMaterialToChildren();
    }



    void AddSurfaceMaterialToChildren()
    {

        Transform childTransform;
        GameObject child;
        for (int i=0; i < transform.childCount; i++ )
        {
            childTransform = transform.GetChild(i);
            child = childTransform.gameObject;

            if (child != null && child.layer == objectLayer && child.GetComponent<SurfaceMaterial>() == null)
            {
                var childComponent = child.AddComponent(typeof(SurfaceMaterial)) as SurfaceMaterial;
                childComponent.materialInfo = materialInfo;
            }
        }
    }

    public void PlaceDecal(RaycastHit hitInfo, Vector3 dir, bool isSilent = false )
    {
        if (materialInfo == null) return;

        GameObject decalPrefab = materialInfo.decal;
        GameObject impactEffectPrefab = materialInfo.impactEffect;
        Material decalMat = materialInfo.decalMaterial;

        GameObject decal = Pool.Instance.InstantiateFromPool(decalPrefab, hitInfo.point + hitInfo.normal * 0.1f, Quaternion.FromToRotation(Vector3.forward, dir));
        if (decal != null)
        {
            float rand = Random.Range(0.8f, 1f);
            decal.transform.localScale = new Vector3(rand, rand, 1);
            decal.transform.rotation *= Quaternion.AngleAxis(Random.Range(-180, 180), Vector3.forward);

            DecalAtlasHelper decalScript = decal.GetComponent<DecalAtlasHelper>();

            if (decalScript != null && decalMat != null)
                decalScript.UpdateDecal(decalMat);

        }

        if (impactEffectPrefab != null)
        {
            Quaternion rotation = Quaternion.FromToRotation(Vector3.right * -1, Vector3.Reflect(dir, hitInfo.normal * -1));
            rotation.eulerAngles += new Vector3(Random.Range(-0.05f, 0.05f), Random.Range(-0.05f, 0.05f), Random.Range(-0.05f, 0.05f));

            GameObject impactEffect = Pool.Instance.InstantiateFromPool(impactEffectPrefab, hitInfo.point + hitInfo.normal * 0.008f, rotation, true);

            var impactScript = impactEffect.GetComponentInChildren<FX_ImpactDefault>();
            if (impactScript != null)
            {
                impactScript.isSilent = isSilent;
            }

            impactEffect.SetActive(true);
        }
    }
}

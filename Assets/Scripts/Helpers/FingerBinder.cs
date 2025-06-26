using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class FingerBinder : MonoBehaviour
{
    void Start()
    {
        var handObject = gameObject.transform.Find("RightHand");
        var weaponScript = GetComponentInParent<BaseInteractable>();

        if (handObject == null || weaponScript == null) return;

        int count = 0;

        for ( int i = 0; i < handObject.childCount; i++ )
        {
            if (!handObject.GetChild(i).name.Contains("Right")) continue;

            foreach (Transform g in handObject.GetChild(i).GetComponentsInChildren<Transform>())
            {
                if (!g.name.Contains("Right")) continue;

                weaponScript.SetFingerBones(count, g);
                Debug.Log(count + " -- " + g.name);
                count++;
            }
        }

        Debug.Log("Finished");
        //DestroyImmediate(this);
    }

}

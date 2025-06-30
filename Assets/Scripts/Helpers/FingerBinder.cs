using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// NOTE TO SELF: Put this on an object that is a parent of Right/Left hand!
[ExecuteInEditMode]
public class FingerBinder : MonoBehaviour
{
    void Start()
    {
        string orientation = "Right";
        var handObject = gameObject.transform.Find(orientation+"Hand");
        var weaponScript = GetComponentInParent<BaseInteractable>();

        if (handObject == null)
        {
            Debug.Log("ERROR! No hand objects of " + orientation + "Hand found! Switching hands.");
            orientation = "Left";
            handObject = gameObject.transform.Find(orientation+"Hand");
        }

        if (handObject == null)
        {
            Debug.Log("ERROR! No hand objects of "+ orientation + "Hand found!");
            return;
        }

        if (weaponScript == null)
        {
            Debug.Log("ERROR! No object script found!");
            return;
        }

        int count = 0;

        for ( int i = 0; i < handObject.childCount; i++ )
        {
            if (!handObject.GetChild(i).name.Contains(orientation)) continue;

            foreach (Transform g in handObject.GetChild(i).GetComponentsInChildren<Transform>())
            {
                if (!g.name.Contains(orientation)) continue;

                weaponScript.SetFingerBones(count, g);
                Debug.Log(count + " -- " + g.name);
                count++;
            }
        }

        Debug.Log("Finished "+ orientation+" Hand!");
        //DestroyImmediate(this);
    }

}

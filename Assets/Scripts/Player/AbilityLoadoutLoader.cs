using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityLoadoutLoader : MonoBehaviour
{
    [SerializeField]
    private HandAbilities abilityManager;

    [SerializeField]
    private List<GameObject> defaultLoadout;

    private bool tryDefault = false;

    void Start()
    {
        //TryDefaultLoadout();
    }

    void Update()
    {
        
    }

    private void LateUpdate()
    {
        if (!tryDefault)
        {
            TryDefaultLoadout();
            tryDefault = true;
        }
    }

    void TryDefaultLoadout()
    {
        if (defaultLoadout.Count <= 0) return;

        for (int i = 0; i < defaultLoadout.Count; i++)
        {
            var abilityObj = defaultLoadout[i];

            if (abilityObj == null) continue;

            Ability_Template abilityClass = abilityObj.GetComponent<Ability_Template>();

            if (abilityClass == null) continue;

            GameObject abilityInstance = Instantiate(abilityObj);

            if (abilityInstance != null)
            {
                abilityClass = abilityInstance.GetComponent<Ability_Template>();

                bool success = abilityManager.TryToAddAbility(abilityClass);

                if (success)
                {
                    //Debug.Log("Added ability " + abilityClass.ToString());
                } 
                else
                    Destroy(abilityInstance);

            }
        }
    }
}

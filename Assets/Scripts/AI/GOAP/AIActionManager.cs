using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static partial class AIGlobals
{
    public static AIActionManager ACTION_MANAGER = null;
}

public class AIActionManager : MonoBehaviour
{
    Object[] AssetsToLoad;
    List<AIAction> allActions = new List<AIAction>();
    Dictionary<AIAction, int> allActionsReferences = new Dictionary<AIAction, int>();

    GameObject actionContainer;

    void Awake()
    {
        AIGlobals.ACTION_MANAGER = this;

        CollectAllActions();
    }

    void CollectAllActions()
    {
        actionContainer = BlankCategory("Actions");

        AssetsToLoad = Resources.LoadAll("Action Sets", typeof(GameObject));

        foreach (var set in AssetsToLoad)
        {
            var components = ((GameObject)set).GetComponents<AIAction>();

            foreach (AIAction action in components)
            {
                var actionType = action.GetType();

                if ( actionContainer.GetComponent(actionType) == null )
                    actionContainer.AddComponent(actionType);
            }  
        }

        foreach (AIAction action in actionContainer.GetComponents<AIAction>())
        {
            action.InitAction();
            allActionsReferences.Add(action, allActions.Count);
            allActions.Add(action);
        }
    }

    public AIAction GetAIAction(int id)
    {
        if (id < 0 || id > allActions.Count)
            return null;
        
        if (allActions[id] != null)
            return allActions[id];

        return null;
    }
        
    public int GetAIActionID(AIAction action)
    {
        if (allActionsReferences.ContainsKey(action))
            return allActionsReferences[action];
        return -1;
    }
    public int GetNumAIActions() => allActions.Count;

    public List<AIAction> GetAllActions() => allActions;

    GameObject BlankCategory(string name = "")
    {
        var Empty = new GameObject(name);

        Empty.transform.parent = gameObject.transform;
        Empty.transform.localPosition = Vector3.zero;
        Empty.transform.rotation = Quaternion.identity;

        Empty.layer = gameObject.layer;

        return Empty;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class AIAStarGoal
{
    AIAStarMap map;
    AIGoal goal;
    AICharacter AI = null;

    public void Init(AICharacter AI, AIAStarMap map, AIGoal goal)
    {
        this.AI = AI;
        this.map = map;
        this.goal = goal;
    }

    public float GetHeuristicDistance(AStarNode node)
    {
        AIAction action = map.GetAIAction(node.ID);
        if (action != null)
        {
            action.SolvePlanWSVariable(AI, node.worldStateCur, node.worldStateGoal);
            action.SetPlanWSPreconditions(AI, node.worldStateGoal);

            AI.GetAIPlanner().MergeWorldStates(node.worldStateCur, node.worldStateGoal);
        }

        else if (goal != null)
        {
            goal.SetWSSatisfaction(node.worldStateGoal);

            AI.GetAIPlanner().MergeWorldStates(node.worldStateCur, node.worldStateGoal);
        }

        return (node.worldStateCur.GetNumWorldStateDifferences(node.worldStateGoal));
    }

    public float GetActualCost(AStarNode nodeA, AStarNode nodeB)
    {
        nodeB.worldStateCur.CopyWorldState(nodeA.worldStateCur);
        nodeB.worldStateGoal.CopyWorldState(nodeA.worldStateGoal);

        AIAction action = map.GetAIAction(nodeB.ID);
        if (action != null)
        {
            return action.GetActionCost();
        }

        return 1;
    }

    public bool IsAStarFinished(AStarNode node)
    {
        if (node == null)
        {
            //Debug.Log("ASTAR PLANNER: NODE IS NULL, FINISHED ");
            return true;
        }

        if (IsPlanValid(node))
        {
            //Debug.Log("ASTAR PLANNER: PLAN IS VALID, FINISHED ");
            return true;
        }

        //Debug.Log("ASTAR PLANNER: IS NOT FINISHED "+pAStarNode?.iNodeID);

        return false;
    }

    public bool IsPlanValid(AStarNode node)
    {
        //Debug.Log("ASTAR PLANNER: Checking plan for node "+ pAStarNode?.iNodeID);

        WS worldState = new WS();
        AI.GetAIPlanner().MergeWorldStates(worldState, node.worldStateCur);

        AIAction action = map.GetAIAction(node.ID);
        if (action == null)
            return false;

        //Debug.Log("ASTAR PLANNER: Found Action "+pAction);

        AStarNode nodeParent = null;
        while (action != null)
        {
            nodeParent = node.parent;

            //Debug.Log("ASTAR PLANNER: Parent for " + pAction + "  "+ pNodeParent?.iNodeID);

            if (!action.ValidateWSEffects(AI, worldState, nodeParent.worldStateGoal))
            {
                //Debug.Log("ASTAR PLANNER: cant validate effects for " + pAction + "  " + pNodeParent?.iNodeID);
                //break;
                return false;
            }

            //Debug.Log("ASTAR PLANNER: Can validate WS effects for " + pAction + " and " + pNodeParent?.iNodeID);

            if (!action.ValidateWSPreconditions(AI, worldState, nodeParent.worldStateGoal))
            {
                //break;
                return false;
            }

            //Debug.Log("ASTAR PLANNER: Can validate WS preconditions for " + pAction + " and " + pNodeParent?.iNodeID);

            if (!action.ValidateContextPreconditions(AI, nodeParent.worldStateGoal, true))
            {
                //break;
                return false;
            }

            //Debug.Log("ASTAR PLANNER: Can validate context effects for " + pAction + " and " + pNodeParent?.iNodeID);

            if (action.GetActionProbability() < 1 && action.GetActionProbability() < UnityEngine.Random.Range(0.0f, 1.0f))
            {
                action.FailActionProbability(AI);
                return false;
            }

            //Debug.Log("ASTAR PLANNER: Passed probability for " + pAction + " and " + pNodeParent?.iNodeID);

            //Debug.Log("ASTAR PLANNER: Applying final worldstate ");

            action.ApplyWSEffect(AI, worldState, nodeParent.worldStateGoal);

            node = node.parent;
            if (node != null)
                action = map.GetAIAction(node.ID);

        }

        if (goal != null && nodeParent != null)
        {
            //Debug.Log("ASTAR PLANNER: Found goal " + m_pAIGoal + " and parent " + pNodeParent?.iNodeID);

            if (nodeParent.worldStateGoal.GetNumUnsatisfiedWorldStateProps(worldState) > 0)
            {
                //Debug.Log("ASTAR PLANNER: NOT SATISFIED " + pNodeParent?.iNodeID+"  NUM UNSATISFIED "+ pNodeParent.wsWorldStateGoal.GetNumUnsatisfiedWorldStateProps(wsWorldState));
                return false;
            }
            //Debug.Log("ASTAR PLANNER: IS SATISFIED " + pNodeParent?.iNodeID);
            return true;
        }

        return false;
    }
}


public class ActionPrecedenceComparer : IComparer
{
    public int Compare(object arg1, object arg2)
    {

        int actionId1 = (int)arg1;
        int actionId2 = (int)arg2;

        AIAction action1 = AIGlobals.ACTION_MANAGER.GetAIAction(actionId1);
        AIAction action2 = AIGlobals.ACTION_MANAGER.GetAIAction(actionId2);

        if (!(action1 != null && action2 != null))
            return 0;

        if (action1.GetActionPrecedence() < action2.GetActionPrecedence())
            return 1;

        if (action1.GetActionPrecedence() > action2.GetActionPrecedence())
            return -1;

        return 0;
    }
}

public class AIAStarMap
{
    AICharacter AI;
    Dictionary<string,List<AIAction>> effectActions = new Dictionary<string,List<AIAction>>();
    int[] neighborActions;
    int numNeighborActions = 0;
    bool effectTableBuilt = false;
    IComparer actionComparer = new ActionPrecedenceComparer();

    public AIAStarMap()
    {
        neighborActions = new int[AIGlobals.ACTION_MANAGER.GetNumAIActions()];
    }

    public void BuildEffectActionsTable()
    {
        if (effectTableBuilt)
            return;

        WS effects;
        HashSet<string> keys;

        var allActions = AIGlobals.ACTION_MANAGER.GetAllActions();

        foreach (AIAction action in allActions)
        {
            effects = action.GetActionEffects();
            if (effects == null)
                continue;

            keys = effects.GetPropertyKeys();
            if (keys == null)
                continue;

            foreach (string keyProperty in keys)
            {
                if (!effectActions.ContainsKey(keyProperty))
                    effectActions.Add(keyProperty, new List<AIAction>());

                effectActions[keyProperty].Add(action);
                //Debug.Log("Adding action " + action.ToString() + "  to key property " + keyProperty);
            }
        }
        
        effectTableBuilt = true;
    }

    public void Init(AICharacter AI)
    {
        if (this.AI == AI)
            return;

        this.AI = AI;
    }

    public int GetNumNeighbors(AICharacter AI, AStarNode node)
    {
        if (node == null)
            return 0;

        numNeighborActions = 0;
        AIAction action;

        HashSet<string> keysCur = node.worldStateCur.GetPropertyKeys();
        HashSet<string> keysGoal = node.worldStateGoal.GetPropertyKeys();

        HashSet<string> sharedKeys = new HashSet<string>(keysCur);
        sharedKeys.UnionWith(keysGoal);


        foreach (string keyProperty in sharedKeys)
        {
            if (!(node.worldStateCur.HasProperty(keyProperty) && node.worldStateGoal.HasProperty(keyProperty)))
            {
                continue;
            }
            
            var curProperty = node.worldStateCur.GetProperty(keyProperty);
            var goalProperty = node.worldStateGoal.GetProperty(keyProperty);

            if (curProperty != null && goalProperty != null)
            {
                //Debug.Log("Both worldstates exist   " + keyProperty);

                if (!curProperty.Equals(goalProperty))// && effectActions.ContainsKey(keyProperty))
                {
                    for (int i = 0; i < effectActions[keyProperty].Count; i++)
                    {
                        action = effectActions[keyProperty][i];
                        int actionID = AIGlobals.ACTION_MANAGER.GetAIActionID(action);

                        //TODO: check action set

                        ////
                        if (!(action != null && action.ValidateContextPreconditions(AI, node.worldStateGoal, true)))
                            continue;

                        //Debug.Log(AI.gameObject.name + " Confirmed Action   " + action);

                        neighborActions[numNeighborActions] = actionID;
                        ++numNeighborActions;

                        if (numNeighborActions >= AIGlobals.ACTION_MANAGER.GetNumAIActions())
                            break;
                    }
                }
            }

            if (numNeighborActions >= AIGlobals.ACTION_MANAGER.GetNumAIActions())
                break;
        }

        if (numNeighborActions > 1)
            System.Array.Sort(neighborActions, actionComparer);

        return numNeighborActions;
    }

    public int GetNeighbor(int neighbor) => neighborActions[neighbor];

    public AIAction GetAIAction(int node) => AIGlobals.ACTION_MANAGER.GetAIAction(node);

}

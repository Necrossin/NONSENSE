using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIPlanner : MonoBehaviour
{
    AICharacter AI;

    AIAStar astar = new AIAStar();
    AIAStarMap astarMap = new AIAStarMap();
    AIAStarNodeStorage nodeStorage = new AIAStarNodeStorage();
    AIAStarGoal astarGoal = new AIAStarGoal();

    void Start()
    {
        AI = GetComponentInParent<AICharacter>();

        nodeStorage.Init(astar);
        astar.Init(nodeStorage, astarGoal, astarMap);
        astarMap.BuildEffectActionsTable();
    }

	public bool BuildPlan(AIGoal goal)
	{
		astarMap.Init(AI);
		astarGoal.Init(AI, astarMap, goal);

		astar.SetAStarSource(-1);
		astar.RunAStar(AI);

		AStarNode node = astar.GetNodeCur();
		if (node == null)
		{
			//Debug.Log("AI PLANNER: Can not build plan");
			return false;
		}

		var plan = AI.GetAIPlan();
		plan.Init();

		KeyValuePair<int,WS> planStep;

		while (node != null)
		{
			if (node.ID == -1)
				break;

			planStep = new KeyValuePair<int, WS>(node.ID, new WS());

			node = node.parent;

			planStep.Value.CopyWorldState(node.worldStateGoal);

			plan.planSteps.Add(planStep);
		}

		return true;
	}

	public void MergeWorldStates(WS worldStateCur, WS worldStateGoal)
	{
		var aiWorldState = AI.GetAIWorldState();

		HashSet<string> keysCur = worldStateCur.GetPropertyKeys();
		HashSet<string> keysGoal = worldStateGoal.GetPropertyKeys();
		HashSet<string> keysAI = aiWorldState.GetPropertyKeys();

		HashSet<string> sharedKeys = new HashSet<string>(keysCur);
		sharedKeys.UnionWith(keysGoal);
		sharedKeys.UnionWith(keysAI);

		foreach (string keyProperty in sharedKeys)
		{
			var property = worldStateGoal.GetProperty(keyProperty);

			if (property == null)
				continue;

			var aiProperty = aiWorldState.GetProperty(keyProperty);

			if (aiProperty != null)
				worldStateCur.SetProperty(keyProperty, aiProperty);
		}
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIGoalManager : MonoBehaviour
{
    AICharacter AI;
    AIGoal curGoal;

    List<AIGoal> allGoals = new List<AIGoal>();

    void Start()
    {
        AI = GetComponentInParent<AICharacter>();
	}

    public void SetGoalSet( GameObject goalSet )
    {
        if (goalSet == null)
            return;

		if (AI == null)
			AI = GetComponentInParent<AICharacter>();

		var Goals = Instantiate(goalSet, transform.position, Quaternion.identity);

		Goals.name = "Goals (" + goalSet.name + ")";
		Goals.transform.parent = gameObject.transform;
        Goals.transform.localPosition = Vector3.zero;
        Goals.transform.rotation = Quaternion.identity;
        Goals.layer = gameObject.layer;

        foreach ( AIGoal goal in Goals.GetComponents<AIGoal>() )
        {
			goal.InitGoal(AI);
			allGoals.Add(goal);
		}
            
    }

	public void UpdateGoalRelevances(bool replan)
	{
		bool requiresHigherInterruptPriority = false;
		bool goalChanged = false;

		AIGoal relevantGoal = FindMostRelevantGoal(true);

		while (relevantGoal != null)
		{
			if ((curGoal != null && relevantGoal != null) && (!curGoal.Equals(relevantGoal)) && (curGoal.GetGoalRelevance() == relevantGoal.GetGoalRelevance()))
			{
				relevantGoal = curGoal;
			}

			if ((relevantGoal.Equals(curGoal)) && (!replan) && (!curGoal.ReplanRequired()))
			{
				break;
			}

			if (requiresHigherInterruptPriority && relevantGoal != null && curGoal != null && (!relevantGoal.Equals(curGoal)) && (relevantGoal.GetInterruptPriority() <= curGoal.GetInterruptPriority()))
			{
				relevantGoal.ClearGoalRelevance();
			}

			else if ((relevantGoal.GetActivateChance() < 1) && (relevantGoal.GetActivateChance() < Random.Range(0.0f, 1.0f)))
			{
				relevantGoal.ClearGoalRelevance();
			}

			else if (relevantGoal.BuildPlan())
			{
				//Debug.Log("[GOAL MANAGER] Building plan");
				if (curGoal != null)
				{
					curGoal.DeactivateGoal();
				}

				curGoal = relevantGoal;
				curGoal.ActivateGoal();
				curGoal.ActivatePlan();
				goalChanged = true;
				break;
			}

			else
			{
				//Debug.Log("[GOAL MANAGER] Plan failed");
				relevantGoal.HandleBuildPlanFailure();
			}

			relevantGoal = FindMostRelevantGoal(false);
		}

		if (replan && relevantGoal == null && !requiresHigherInterruptPriority)
		{
			curGoal = null;
			AI.ClearState();
		}

		if (requiresHigherInterruptPriority && !goalChanged)
		{
			return;
		}
		else
		{
			AI.SetShouldSelectAction(false);
		}
		//Debug.Log("[GOAL MANAGER]"+ m_pAI.gameObject + " Cur Goal is: " + m_pCurGoal);

		if (AI.testText != null && curGoal != null)
			AI.testText.text = curGoal.GetType().ToString();
	}

	public AIGoal FindMostRelevantGoal(bool recalculate)
	{
		float relevance;
		float maxRelevance = 0;

		float nextRecalcTime;
		AIGoal pGoalMax = null;

		foreach ( AIGoal goal in allGoals )
		{
			if (recalculate)
			{
				nextRecalcTime = goal.GetNextRecalcTime();
				if ((goal != curGoal) && (nextRecalcTime > 0) && (nextRecalcTime > Time.time))
				{
					goal.ClearGoalRelevance();
				}

				else if ((!goal.GetReEvalOnSatisfaction()) && goal.IsWSSatisfied(AI.GetAIWorldState()))
				{
					goal.ClearGoalRelevance();
				}
				//else if (!goal.IsAwarenessValid())
				//{
				//	pGoal.ClearGoalRelevance();
				//}
				else
				{
					goal.CalculateGoalRelevance();

					if (nextRecalcTime > 0)
						goal.SetNextRecalcTime();
				}
			}

			relevance = goal.GetGoalRelevance();

			if (relevance > maxRelevance)
			{
				maxRelevance = relevance;
				pGoalMax = goal;
			}
		}

		return pGoalMax;
	}

	public void SelectRelevantGoal()
	{
		if (AI == null)
			return;

		bool updateGoals = false;
		bool forceReplanning = false;

		if (curGoal != null)
		{
			if (!curGoal.IsPlanValid())
			{
				//Debug.Log("[GOAL MANAGER] Goal exists, but plan is not valid");
				updateGoals = true;
				forceReplanning = true;
			}


			else if (curGoal.IsWSSatisfied(AI.GetAIWorldState()))
			{
				//Debug.Log("[GOAL MANAGER] Goal exists, but worldstate is satisfied");
				updateGoals = true;
				forceReplanning = true;
			}
		}


		if (AI.GetShouldSelectAction())
		{
			updateGoals = true;
		}

		if (AI.GetShouldUpdatePlan())
		{
			updateGoals = true;
			forceReplanning = true;
		}

		if (updateGoals)
		{
			UpdateGoalRelevances(forceReplanning);
		}

		AI.SetShouldUpdatePlan(false);
	}

	public void UpdateGoal()
	{
		if (curGoal != null)
		{
			if (!curGoal.UpdateGoal())
			{
				AI.SetShouldSelectAction(true);
				curGoal.ClearPlan();
			}
		}
	}

	public bool IsCurGoal(AIGoal goal) => goal == curGoal;

	void Update()
    {
		UpdateGoal();
		SelectRelevantGoal();
	}
}

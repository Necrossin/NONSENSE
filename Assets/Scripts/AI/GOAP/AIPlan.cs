using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIPlan : MonoBehaviour
{
    AICharacter AI;
    public List<KeyValuePair<int, WS>> planSteps = new List<KeyValuePair<int, WS>>();

    int curStep;
    double planActivationTime;

    void Start()
    {
        AI = GetComponentInParent<AICharacter>();
    }

    public void Init()
    {
        planSteps.Clear();

        curStep = 0;
        planActivationTime = 0;
    }

	public void Clear() => Init();

	public void ActivatePlan()
	{
		if (AI == null)
			return;

		planActivationTime = Time.time;

		if (planSteps.Count < 1)
			return;

		var planStep = planSteps[0];

		AIAction action = AIGlobals.ACTION_MANAGER.GetAIAction(planStep.Key);
		if (action != null)
		{
			if (!action.ValidateContextPreconditions(AI, planStep.Value, false))
			{
				AI.SetShouldUpdatePlan(true);
				return;
			}

			action.ActivateAction(AI, planStep.Value);

			if (action.IsActionComplete(AI))
			{
				AdvancePlan();
			}
		}
	}

	public void DeactivatePlan()
	{
		AIAction action = GetCurrentPlanStepAction();
		if (action != null)
			action.DeactivateAction(AI);
	}

	public bool PlanStepIsComplete()
	{
		if (curStep >= planSteps.Count)
			return false;

		var planStep = planSteps[curStep];

		AIAction action = AIGlobals.ACTION_MANAGER.GetAIAction(planStep.Key);
		if (action != null)
		{
			return action.IsActionComplete(AI);
		}

		return false;
	}

	public bool PlanStepIsInterruptible()
	{
		if (curStep >= planSteps.Count)
			return true;

		var planStep = planSteps[curStep];

		AIAction action = AIGlobals.ACTION_MANAGER.GetAIAction(planStep.Key);
		if (action != null)
		{
			return action.IsActionInterruptible();
		}

		return true;
	}

	public bool AdvancePlan()
	{
		AIAction action;
		KeyValuePair<int,WS> planStep;

		while (true)
		{
			planStep = planSteps[curStep];
			action = AIGlobals.ACTION_MANAGER.GetAIAction(planStep.Key);
			if (action != null)
			{
				action.ApplyContextEffect(AI, AI.GetAIWorldState(), planStep.Value);
				action.DeactivateAction(AI);
			}

			curStep++;

			if (curStep >=planSteps.Count)
			{
				return false;
			}

			planStep = planSteps[curStep];
			action = AIGlobals.ACTION_MANAGER.GetAIAction(planStep.Key);
			if (action != null)
			{

				if (!action.ValidateContextPreconditions(AI, planStep.Value, false))
				{
					return false;
				}

				action.ActivateAction(AI, planStep.Value);
				if (!action.IsActionComplete(AI))
				{
					return true;
				}
				action.DeactivateAction(AI);
			}
		}

		return false;
	}

	public bool Exists() => planSteps.Count > 0;

	public bool IsPlanValid()
	{
		if (planSteps.Count < 1)
        {
			return false;
        }
			
		if (curStep >= planSteps.Count)
		{
			return false;
		}

		var planStep = planSteps[curStep];
		AIAction action = AIGlobals.ACTION_MANAGER.GetAIAction(planStep.Key);
		if (action != null)
		{
			return action.ValidateAction(AI);
		}

		return false;
	}

	public AIAction GetCurrentPlanStepAction()
	{
		if (curStep >= planSteps.Count)
		{
			return null;
		}

		return AIGlobals.ACTION_MANAGER.GetAIAction(planSteps[curStep].Key);
	}

	public double GetPlanActivationTime() => planActivationTime;

}

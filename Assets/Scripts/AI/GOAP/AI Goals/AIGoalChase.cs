using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIGoalChase : AIGoal
{
	protected override void ApplySettings()
	{
		settings.relevance = 24;
		settings.interruptPriority = 2;
		settings.reEvalOnSatisfaction = true;
	}

	protected override void AddRequiredSensors()
	{
		requiredSensors.Add(typeof(AISensorVisual));
	}

	public override void CalculateGoalRelevance()
	{
		var target = AI.GetTarget();

		if (!AI.HasPlayerTarget())
		{
			goalRelevance = 0;
			return;
		}

		/*Vector3 dest = target.transform.position;

		if (AI.GetAIMovement().IsAIRoughlyThere(dest))
		{
			goalRelevance = 0;
			return;
		}*/

		goalRelevance = settings.relevance;
	}

	public override void SetWSSatisfaction(WS worldState)
	{
		worldState.SetProperty("AtTargetPosition", true);
	}

	public override bool IsWSSatisfied(WS worldState)
	{
		var property = worldState.GetProperty("AtTargetPosition");
		if (property == null || (property != null && !(bool)property))
		{
			return false;
		}

		return true;
	}

}

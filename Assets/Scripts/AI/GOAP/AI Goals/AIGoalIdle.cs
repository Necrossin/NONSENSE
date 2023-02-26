using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIGoalIdle : AIGoal
{
	protected override void ApplySettings()
	{
		settings.relevance = 0.1f;
	}

	protected override void AddRequiredSensors()
	{
		requiredSensors.Add(typeof(AISensorIdle));
	}

	public override void CalculateGoalRelevance() => goalRelevance = 0.1f;

    public override void SetWSSatisfaction(WS worldState)
	{
		worldState.SetProperty("Idling", true);
	}

	public override bool IsWSSatisfied(WS worldState)
	{
		return false;
	}
}

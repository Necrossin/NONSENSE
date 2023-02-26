using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIGoalKill : AIGoal
{
    protected override void ApplySettings()
    {
        settings.relevance = 26;
        settings.interruptPriority = 2;
        settings.reEvalOnSatisfaction = false;
    }

    public override void CalculateGoalRelevance()
    {
        if (AI.HasPlayerTarget())
        {
            goalRelevance = settings.relevance;
            return;
        }

        goalRelevance = 0;
    }

    public override void SetWSSatisfaction(WS worldState)
    {
        worldState.SetProperty("TargetIsDead", true);
    }

    public override bool IsWSSatisfied(WS worldState)
    {
        var property = worldState.GetProperty("TargetIsDead");
        if (property == null || (property != null && !(bool)property))
        {
            return false;
        }

        return true;
    }
}

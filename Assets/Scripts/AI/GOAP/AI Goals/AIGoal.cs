using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AIGoal : MonoBehaviour
{
    protected struct GoalSettings
    {
        public float activationChance;
        public float interruptPriority;
        public bool reEvalOnSatisfaction;
        public float recalcRateMax;
        public float recalcRateMin;
        public float frequency;
        public float relevance;

    }
    
    protected AICharacter AI;

    protected float goalRelevance;
    protected float nextRecalcTime;
    protected float activationTime;

    protected List<System.Type> requiredSensors = new List<System.Type>();

    protected GoalSettings settings = new GoalSettings();

    protected void ApplyDefaultSettings()
    {
        settings.activationChance = 1;
        settings.interruptPriority = 0;
        settings.reEvalOnSatisfaction = false;
        settings.recalcRateMax = 0;
        settings.recalcRateMin = 0;
        settings.frequency = 0;
        settings.relevance = 0;
    }

    protected virtual void ApplySettings()
    {

    }

    protected virtual void AddRequiredSensors()
    {

    }

    private void Awake()
    {
        goalRelevance = 0;
        nextRecalcTime = 0;
        activationTime = 0;

        ApplyDefaultSettings();
        ApplySettings();
    }

    public void InitGoal( AICharacter AI )
    {
        this.AI = AI;

        AddRequiredSensors();
        InitializeRequiredSensors();
    }

    void InitializeRequiredSensors()
    {
        if (requiredSensors == null) 
            return;

        if (AI.GetSensorContainer() == null)
            return;

        for ( int i=0; i < requiredSensors.Count; i++ )
        {
            System.Type sensor = requiredSensors[i];

            if (AI.GetSensorContainer().GetComponent(sensor) == null)
                AI.GetSensorContainer().AddComponent(sensor);
        }
        
    }

    public virtual void ActivateGoal()
    {
        activationTime = Time.time;
    }

    public virtual void DeactivateGoal()
    {
        if (settings.frequency > 0)
        {
            nextRecalcTime = Time.time + settings.frequency;
        }

        //AI.GetAIWorldState().SetWSProp(WORLDSTATE_PROPERTY_KEY.ReactedToWorldStateEvent, WORLDSTATE_PROPERTY_TYPE.WorldStateEvent, (int)WorldStateEvent.Invalid);
    }
    public void ActivatePlan()
    {
        AIPlan plan = AI.GetAIPlan();
        if (plan != null && plan.Exists())
        {
            plan.ActivatePlan();
        }
    }

    public bool BuildPlan()
    {
        return AI.GetAIPlanner().BuildPlan(this);
    }

    public virtual bool IsPlanValid()
    {
        AIPlan plan = AI.GetAIPlan();
        if (plan == null)
        {
            return false;
        }

        if (!plan.Exists())
        {
            return false;
        }

        return plan.IsPlanValid();
    }

    public void ClearPlan()
    {
        AIPlan plan = AI.GetAIPlan();
        if (plan != null)
        {
            plan.Clear();
        }
    }

    public virtual bool UpdateGoal()
    {
        AIPlan plan = AI.GetAIPlan();
        if (plan == null)
        {
            return false;
        }

        if (!plan.Exists())
        {
            return false;
        }

        if (plan.PlanStepIsComplete())
        {
            return plan.AdvancePlan();
        }

        return true;
    }

    public virtual void SetNextRecalcTime()
    {
        if (settings.recalcRateMax > 0f)
        {
            nextRecalcTime = Time.time + Random.Range(settings.recalcRateMin, settings.recalcRateMax);
        }
    }

    public virtual void SetWSSatisfaction(WS worldState) { }
    public virtual bool IsWSSatisfied(WS worldState) => false;
    public virtual void HandleBuildPlanFailure() { ClearGoalRelevance(); }
    public virtual bool ReplanRequired() => false;
    public virtual float GetNextRecalcTime() => nextRecalcTime;
    public virtual void CalculateGoalRelevance() { }
    public float GetActivateChance() => settings.activationChance;
    public float GetInterruptPriority() => settings.interruptPriority;
    public bool GetReEvalOnSatisfaction() => settings.reEvalOnSatisfaction;
    public float GetGoalRelevance() => goalRelevance;
    public void ClearGoalRelevance() => goalRelevance = 0;

}

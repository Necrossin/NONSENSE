using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class AIAction : MonoBehaviour
{

    protected struct ActionSettings
    {
        public float cost;
        public float precedence;
        public bool isInterruptible;
        public float probability;
    }

    protected WS worldStatePreconditions = new WS();
    protected WS worldStateEffects = new WS();

    protected ActionSettings settings = new ActionSettings();

    protected void ApplyDefaultSettings()
    {
        settings.cost = 2;
        settings.precedence = 1;
        settings.isInterruptible = true;
        settings.probability = 1;
    }

    protected virtual void ApplySettings()
    {
        
    }


    private void Awake()
    {
        ApplyDefaultSettings();
        ApplySettings();
    }

    public virtual void InitAction()
    {
        
    }

    public virtual void ActivateAction(AICharacter AI, WS worldStateGoal)
    {
        
    }

    public virtual void SetPlanWSPreconditions(AICharacter AI, WS worldStateGoal)
    {
        HashSet<string> keysPreconditions = worldStatePreconditions.GetPropertyKeys();
        HashSet<string> keysGoal = worldStateGoal.GetPropertyKeys();

        HashSet<string> sharedKeys = new HashSet<string>(keysPreconditions);
        sharedKeys.UnionWith(keysGoal);

        foreach (string keyProperty in sharedKeys)
        {
            var precondition = worldStatePreconditions.GetProperty(keyProperty);

            if (precondition == null)
                continue;

            worldStateGoal.SetProperty(keyProperty, precondition);
        }
    }

    public virtual void SolvePlanWSVariable(AICharacter AI, WS worldStateCur, WS worldStateGoal)
    {
        HashSet<string> keysEffects = worldStateEffects.GetPropertyKeys();
        HashSet<string> keysCur = worldStateCur.GetPropertyKeys();
        HashSet<string> keysGoal = worldStateGoal.GetPropertyKeys();

        HashSet<string> sharedKeys = new HashSet<string>(keysCur);
        sharedKeys.UnionWith(keysGoal);
        sharedKeys.UnionWith(keysEffects);

        foreach (string keyProperty in sharedKeys)
        {
            var effect = worldStateEffects.GetProperty(keyProperty);

            if (effect == null)
                continue;

            var property = worldStateGoal.GetProperty(keyProperty);

            if (property != null)
                worldStateCur.SetProperty(keyProperty, property);
        }
    }

    public virtual bool ValidateWSPreconditions(AICharacter AI, WS worldStateCur, WS worldStateGoal)
    {
        HashSet<string> keysPreconditions = worldStatePreconditions.GetPropertyKeys();
        HashSet<string> keysCur = worldStateCur.GetPropertyKeys();
        HashSet<string> keysGoal = worldStateGoal.GetPropertyKeys();

        HashSet<string> sharedKeys = new HashSet<string>(keysCur);
        sharedKeys.UnionWith(keysGoal);
        sharedKeys.UnionWith(keysPreconditions);

        foreach (string keyProperty in sharedKeys)
        {
            var precondition = worldStatePreconditions.GetProperty(keyProperty);
            var property = worldStateCur.GetProperty(keyProperty);

            if (precondition == null)
                continue;

            if (property != null && property != precondition)
                return false;
        }

        return true;
    }

    public virtual bool ValidateWSEffects(AICharacter AI, WS worldStateCur, WS worldStateGoal)
    {
        HashSet<string> keysEffects = worldStateEffects.GetPropertyKeys();
        HashSet<string> keysCur = worldStateCur.GetPropertyKeys();
        HashSet<string> keysGoal = worldStateGoal.GetPropertyKeys();

        HashSet<string> sharedKeys = new HashSet<string>(keysCur);
        sharedKeys.UnionWith(keysGoal);
        sharedKeys.UnionWith(keysEffects);

        foreach (string keyProperty in sharedKeys)
        {
            var effect = worldStateEffects.GetProperty(keyProperty);

            if (effect == null)
                continue;

            var property = worldStateGoal.GetProperty(keyProperty);

            if (property == effect)
                continue;

            return true;
        }

        return false;
    }

    public virtual void ApplyWSEffect(AICharacter AI, WS worldStateCur, WS worldStateGoal)
    {
        if (!(worldStateCur != null && worldStateGoal != null))
            return;

        HashSet<string> keysEffects = worldStateEffects.GetPropertyKeys();
        HashSet<string> keysCur = worldStateCur.GetPropertyKeys();
        HashSet<string> keysGoal = worldStateGoal.GetPropertyKeys();

        HashSet<string> sharedKeys = new HashSet<string>(keysCur);
        sharedKeys.UnionWith(keysGoal);
        sharedKeys.UnionWith(keysEffects);

        foreach (string keyProperty in sharedKeys)
        {
            var effect = worldStateEffects.GetProperty(keyProperty);

            if (effect == null)
                continue;

            worldStateCur.SetProperty(keyProperty, effect);

        }
    }

    public virtual bool ValidateAction(AICharacter AI)
    {
        if (AI == null)
            return false;

        if (AI.GetState() != null && AI.GetState().GetStateStatus() == AIStateStatus.Failed)
        {
            return false;
        }

        return true;
    }

    public virtual bool ValidateContextPreconditions(AICharacter AI, WS worldStateGoal, bool isPlanning) => true;
    public float GetActionCost() => settings.cost;
    public float GetActionPrecedence() => settings.precedence;
    public bool IsActionInterruptible() => settings.isInterruptible;
    public virtual void DeactivateAction(AICharacter AI) { }
    public virtual bool IsActionComplete(AICharacter AI) => false;
    public float GetActionProbability() => settings.probability;
    public WS GetActionEffects() => worldStateEffects;
    public virtual bool ValidatePreconditions(AICharacter AI, WS worldStateGoal, bool isPlanning) => true;
    public virtual void ApplyContextEffect(AICharacter AI, WS worldStateCur, WS worldStateGoal) { }
    public virtual void FailActionProbability(AICharacter AI) { }
    public System.Type GetActionType() => GetType();

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AISensor : MonoBehaviour
{

    protected AICharacter AI;
    //protected float rateModifier = 1;
    protected float stimulationThreshold = 1;
    protected float stimulationMax = 1;
    //protected float stimulationCur = 0;

    void Start()
    {
        AI = GetComponentInParent<AICharacter>();
        StartCoroutine(SensorCoroutine());

    }

    void Update()
    {
        
    }

    protected virtual IEnumerator SensorCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(GetUpdateRate());

            UpdateSensor();
        }
    }

    public virtual void UpdateSensor()
    {
        bool stimulated = StimulateSensor();
        if (!stimulated)
            DestimulateSensor();
    }

	public virtual bool StimulateSensor()
    {        
        if (AI == null)
            return false;

        float rateModifier = 1;
        if (!ComplexCheck( ref rateModifier ))
            return false;

        return true;
    }

    public virtual float IncreaseStimulation( float curStimulation, float rateModifier )
    {
        Awareness awareness = AI.GetAwareness();
        float stimulationIncreaseRate;
        if (awareness == Awareness.Alert)
        {
            stimulationIncreaseRate = 0.95f;
        }
        else if (awareness == Awareness.Suspicious)
        {
            stimulationIncreaseRate = 0.8f;
        }
        else
        {
            stimulationIncreaseRate = 0.5f;
        }

        curStimulation = Mathf.Min(stimulationMax, curStimulation + (Mathf.Max(Time.deltaTime, GetUpdateRate()) * stimulationIncreaseRate * rateModifier));

        return curStimulation;
    }

    public virtual void DestimulateSensor()
    {

    }

    public virtual float DecreaseStimulation( float curStimulation )
    {
        Awareness awareness = AI.GetAwareness();

        float stimulationDecreaseRate;
        if (awareness == Awareness.Alert)
        {
            stimulationDecreaseRate = 0.125f;
        }
        else if (awareness == Awareness.Suspicious)
        {
            stimulationDecreaseRate = 0.125f;
        }
        else
        {
            stimulationDecreaseRate = 0.5f;
        }

        if (stimulationDecreaseRate > 0 && curStimulation > 0)
        {
            curStimulation = Mathf.Max(0.0f, curStimulation - (Mathf.Max(Time.deltaTime, GetUpdateRate()) * stimulationDecreaseRate));
        }
        return curStimulation;
    }

    public virtual float GetSensorDistSqr() => 0;
    public virtual bool ComplexCheck( ref float rateModifier ) => true;

    public virtual float GetUpdateRate() => 0f;
}

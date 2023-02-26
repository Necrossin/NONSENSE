using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIStateActivity : AIState
{
    float actDuration = 0.5f;
    AI_Action action = AI_Action.None;

    void Start()
    {
        StartCoroutine(UpdateCoroutine());
    }

    public void SetAnimation()
    {
        stateStatus = AIStateStatus.Initialized;
    }

    public void SetAction(AI_Action newAction)
    {
        action = newAction;
    }

    public AI_Action GetAction() => action;

    public void SetDuration(float dur)
    {
        actDuration = dur;
        stateStatus = AIStateStatus.Initialized;
    }

    private IEnumerator UpdateCoroutine()
    {
        while (true)
        {
            if (stateStatus == AIStateStatus.Uninitialized)
                yield return new WaitForSeconds(0.05f);

            yield return new WaitForSeconds(actDuration);

            stateStatus = stateStatus = AIStateStatus.Complete;
        }
    }
}

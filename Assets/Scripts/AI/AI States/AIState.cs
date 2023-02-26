using UnityEngine;

public enum AIStateType
{
    Activity,
    Goto,
};

public enum AIStateStatus
{
    Initialized,
    Complete,
    Failed,
    Uninitialized,
};

public class AIState : MonoBehaviour
{
    protected AICharacter AI;
    protected AIStateStatus stateStatus = AIStateStatus.Uninitialized;
    public virtual bool Init(AICharacter AI)
    {
        this.AI = AI;
        stateStatus = AIStateStatus.Initialized;

        return true;
    }

    public AIStateStatus GetStateStatus() => stateStatus;

    protected virtual void Update()
    {

    }

    public virtual void OnRemove()
    {

    }
}

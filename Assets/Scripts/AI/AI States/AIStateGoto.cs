using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIStateGoto : AIState
{
    AIMovement movement;

    Vector3 destination;
    GameObject destObject;

    void Start()
    {
        AI = GetComponent<AICharacter>();
        movement = AI.GetAIMovement();
    }

    public override bool Init(AICharacter AI)
    {
        if (!base.Init(AI))
        {
            return false;
        }

        movement = AI.GetAIMovement();

        return true;
    }

    public void SetDestination(Vector3 dest)
    {
        if (movement == null)
            movement = AI.GetAIMovement();

        destination = dest;
        stateStatus = AIStateStatus.Initialized;

        movement.SetDestination(dest);
    }

    public void SetDestinationObject(GameObject obj)
    {
        if (movement == null)
            movement = AI.GetAIMovement();

        destObject = obj;
        stateStatus = AIStateStatus.Initialized;

        GetObjectPosition(destObject, ref destination);
        movement.SetDestination(destination);
    }

    void GetObjectPosition(GameObject obj, ref Vector3 pos)
    {
        if (pos == null)
        {
            return;
        }

        if (AI.GetTarget().Equals(obj))
        {
            pos = obj.transform.position; // TODO: replace this
        }
        else
        {
            pos = obj.transform.position;
        }
    }

    public void SetDynamicRepathDistance(float dist)
    {
        movement.SetDynamicDestination(true);
        movement.SetRepathDistanceSqr(dist);
    }

    public override void OnRemove()
    {
        movement.SetStatus(MOVEMENT_STATUS.None);

        movement.SetDynamicDestination(false);
        movement.SetRepathDistanceSqr(0);
    }

    void UpdateMoveLogic()
    {
        if (movement == null)
            return;

        if (stateStatus != AIStateStatus.Complete)
        {
            switch (movement.GetStatus())
            {
                case MOVEMENT_STATUS.Done:
                    {
                        stateStatus = AIStateStatus.Complete;
                        break;
                    }
                case MOVEMENT_STATUS.Failed:
                    {
                        stateStatus = AIStateStatus.Failed;
                        break;
                    }
                    
                case MOVEMENT_STATUS.Set:
                    {
                        if (destObject != null && movement.IsDestinationDynamic())
                        {
                            GetObjectPosition(destObject, ref destination);

                            if (!movement.IsAIRoughlyThere(movement.GetDestination(), destination))
                            {
                                movement.SetDestination(destination);
                            }
                        }
                        break;
                    } 
                default:
                    break;
            }
        }
    }

    protected override void Update()
    {
        UpdateMoveLogic();
    }
}

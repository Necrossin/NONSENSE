using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum MOVEMENT_STATUS
{
    None,
    Set,
    Failed,
    Done
}

public class AIMovement : MonoBehaviour
{
    AICharacter AI;
    NavMeshAgent agent;
    NavMeshPath path;

    Vector3 destination = Vector3.zero;

    MOVEMENT_STATUS status = MOVEMENT_STATUS.None;

    bool pathSet = false;
    bool shouldUpdatePath = false;
    bool destIsDynamic = false;

    float repathDistSqr = 0;


    void Start()
    {
        AI = GetComponent<AICharacter>();
        agent = GetComponent<NavMeshAgent>();

        path = new NavMeshPath();
    }

    
    void Update()
    {
        if (AI == null)
            return;

        agent.updateRotation = !AI.IsFacingTarget();

        if (AI.GetTarget() != null && AI.IsFacingTarget())
        {
            var target = AI.GetTarget();

            Vector3 vDir = (target.transform.position - transform.position).normalized;
            vDir.y = 0;
            Quaternion qRotation = Quaternion.LookRotation(vDir);
            transform.rotation = Quaternion.Slerp(transform.rotation, qRotation, Time.deltaTime * 5);
        }

        if (GetStatus() != MOVEMENT_STATUS.Set)
        {
            pathSet = false;
            return;
        }

        if ((!pathSet || !IsPathValid(destination)) && !SetPath(destination))
        {
            return;
        }

        if (IsAIRoughlyThere(destination))
        {
            FinishPath();
            return;
        }

    }

    bool SetPath(Vector3 dest)
    {
        bool hasPath = HasPathToDestination(dest);
        if (!hasPath)
        {
            SetStatus(MOVEMENT_STATUS.Failed);
            pathSet = false;
            return false;
        }

        SetShouldUpdatePath(false);
        ResetPath();

        agent.destination = dest;
        pathSet = true;

        //fPathCreationTime = Time.time;

        return true;
    }

    bool IsPathValid(Vector3 dest)
    {
        if (GetShouldUpdatePath())
        {
            SetShouldUpdatePath(false);
            ResetPath();
            return false;
        }

        if (IsDestinationDynamic())
        {
            float flDistanceDiffSqr = (agent.destination - dest).sqrMagnitude;
            if (flDistanceDiffSqr >= GetRepathDistanceSqr())
            {
                return false;
            }
        }
        else if (agent.destination != dest)
        {
            return false;
        }

        return true;
    }

    void FinishPath()
    {
        SetStatus(MOVEMENT_STATUS.Done);
        pathSet = false;
        ResetPath();
    }

    public bool IsAIRoughlyThere(Vector3 cur, Vector3 dest, float radiusMul = 2f)
    {
        float fDistanceDiffSqr = (cur - dest).sqrMagnitude;
        float fTolerance = agent.radius * radiusMul;
        float fToleranceSqr = fTolerance * fTolerance;

        return fDistanceDiffSqr <= fToleranceSqr;
    }

    public bool IsAIRoughlyThere(Vector3 dest, float radiusMul = 2f)
    {
        float fDistanceDiffSqr = (AI.GetPos() - dest).sqrMagnitude;
        float fTolerance = agent.radius * radiusMul;
        float fToleranceSqr = fTolerance * fTolerance;

        return fDistanceDiffSqr <= fToleranceSqr;
    }


    public void SetDestination(Vector3 dest)
    {
        destination = dest;
        SetStatus(MOVEMENT_STATUS.Set);
    }

    public void SetShouldUpdatePath(bool b) => shouldUpdatePath = b;
    public bool GetShouldUpdatePath() => shouldUpdatePath;
    public Vector3 GetDestination() => destination;
    public void SetDynamicDestination(bool b) => destIsDynamic = b;
    public bool IsDestinationDynamic() => destIsDynamic;
    public void SetRepathDistanceSqr(float dist) => repathDistSqr = dist * dist;
    public float GetRepathDistanceSqr() => repathDistSqr;
    public void ResetPath() => agent.ResetPath();
    public void SetStatus(MOVEMENT_STATUS newStatus) => status = newStatus;
    public MOVEMENT_STATUS GetStatus() => status;
    public bool HasPathToDestination(Vector3 dest) => agent.CalculatePath(dest, path);
}

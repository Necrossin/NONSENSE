using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public enum Awareness
{
    Invalid = -1,
    Relaxed,
    Suspicious,
    Alert,
    Count,
    Any = Count,
};

//TODO: change this from enum to something else
public enum AI_Action
{
    None = 0,

    FireWeapon,
}

public static partial class AIGlobals
{
    public static List<AICharacter> allAI = new List<AICharacter>();
    public static void ADD_AI(AICharacter AI) => allAI?.Add(AI);
    public static void REMOVE_AI(AICharacter AI) => allAI?.Remove(AI);
}

public class AICharacter : MonoBehaviour
{

    [SerializeField]
    GameObject testWeapon;

    [SerializeField]
    public TextMeshPro testText;

    public GameObject target;

    public LayerMask visibilityBlockMask;

    GameObject curWeapon;
    BaseRangedWeapon curWeaponScript;

    [SerializeField]
    Transform eyeTransform;
    [SerializeField]
    Transform rightHandTransform;

    [SerializeField]
    GameObject goalSet;

    [SerializeField]
    float viewDistance = 15;

    HealthManager healthManager;

    AIState state;
    WS worldState = new WS();
    Awareness awareness;

    AIGoalManager goalManager;
    AIPlan plan;
    AIPlanner planner;
    AIMovement movement;

    GameObject sensorContainer;

    // temporarily public for debugging
    public bool shouldUpdatePlan;
    public bool shouldSelectAction;
    bool shouldFaceTarget;

    public float enemyConfidence = 0;

    void Start()
    {
        movement = gameObject.AddComponent<AIMovement>();

        plan = gameObject.AddComponent<AIPlan>();
        planner = gameObject.AddComponent<AIPlanner>();

        sensorContainer = BlankGameObject("Sensors");

        goalManager = gameObject.AddComponent<AIGoalManager>();
        if (goalSet != null)
            goalManager.SetGoalSet(goalSet);
        else
            Debug.LogWarning(gameObject.name + " has no Goal Set!");

        healthManager = GetComponent<HealthManager>();
        healthManager?.OnTakeDamageEvent.AddListener(OnTakeDamage);
        healthManager?.OnBreakEvent.AddListener(OnAllHealthLost);

        SpawnAndGiveWeapon(testWeapon);

        SetDefaultWorldState();

        //SetShouldSelectAction(true);

        AIGlobals.ADD_AI(this);
    }

    
    void Update()
    {
        HandleWeapons();

        //FireWeapon();
    }

    public bool IsCurWeaponInRange()
    {
        if (curWeaponScript == null)
            return false;

        var fRange = curWeaponScript.IsAIInRange();
        float fSqrDist = (GetTargetPosition() - GetPosition()).sqrMagnitude;

        if ((fRange * fRange) > fSqrDist)
            return true;

        return false;
    }

    public void GiveWeapon(GameObject Weapon)
    {
        curWeapon = Weapon;
        curWeaponScript = Weapon.GetComponent<BaseRangedWeapon>();

        //curWeapon.transform.position = m_tRightHandTransform.position;
        //curWeapon.transform.rotation = m_tRightHandTransform.rotation;

        curWeaponScript.MoveWithChild(curWeaponScript.GetRigidbody().transform, curWeaponScript.GetRelativeTransform().position, Quaternion.identity, rightHandTransform.position, rightHandTransform.rotation);

        curWeapon.transform.SetParent(rightHandTransform.transform);

        curWeaponScript.SetOwnerObject(gameObject);
        curWeaponScript.SetHeldByEnemy(true);

        var rb = curWeaponScript.GetRigidbody();
        if (rb != null)
            rb.isKinematic = true;

        curWeaponScript.OnGrab();
    }

    public void SpawnAndGiveWeapon(GameObject Weapon)
    {
        var SpawnedWeapon = Instantiate(Weapon, transform.position, Quaternion.identity);

        if (SpawnedWeapon != null)
            GiveWeapon(SpawnedWeapon);
    }

    public void FireWeapon()
    {
        if (GetActiveWeapon() == null)
            return;

        if (GetActiveWeaponInfo() == null)
            return;

        var pWeapon = GetActiveWeaponInfo();

        if (pWeapon.IsAutomatic())
            pWeapon.DoTriggerInteractionHold(true);
        else
            pWeapon.DoTriggerInteraction();
    }

    public void StopFiringWeapon()
    {
        if (GetActiveWeapon() == null)
            return;

        if (GetActiveWeaponInfo() == null)
            return;

        var pWeapon = GetActiveWeaponInfo();

        if (pWeapon.IsAutomatic())
            pWeapon.DoTriggerInteractionHold(false);
    }

    void HandleWeapons()
    {
        if (GetActiveWeapon() == null)
            return;

        if (GetActiveWeaponInfo() == null)
            return;

        // fire weapon is we have to
        if (GetState() != null && GetState().GetType() == typeof(AIStateActivity))
        {
            var curState = (AIStateActivity)GetState();
            if (curState.GetAction() == AI_Action.FireWeapon)
                FireWeapon();
        }
    }


    GameObject BlankGameObject(string name = "")
    {
        var Empty = new GameObject(name);

        Empty.transform.parent = gameObject.transform;
        Empty.transform.localPosition = Vector3.zero;
        Empty.transform.rotation = Quaternion.identity;

        Empty.layer = gameObject.layer;

        return Empty;
    }

    public void OnTakeDamage(DamageInfo damageInfo)
    {
        //Debug.Log(gameObject.name + " called take damage event with args "+ damageInfo.iAmount);
    }

    public void OnAllHealthLost(DamageInfo damageInfo)
    {
        //Debug.Log(gameObject.name + " called all health lost event with args "+ damageInfo.iAmount);
    }

    void OnDestroy()
    {
        healthManager?.OnTakeDamageEvent.RemoveListener(OnTakeDamage);
        healthManager?.OnBreakEvent.RemoveListener(OnAllHealthLost);

        AIGlobals.REMOVE_AI(this);
    }

    private void OnCollisionEnter(Collision collision)
    {
        // its a player
        if (collision.gameObject != null && collision.gameObject.layer == 8)
        {
           
        }
    }

    void SetDefaultWorldState()
    {
        worldState.SetProperty("Idling", false);
        worldState.SetProperty("TargetIsDead", false);
        worldState.SetProperty("AtTargetPosition", false);
    }

    public void SetState(AIStateType newState)
    {
        if (state != null)
        {
            state.OnRemove();
            Destroy(state);
            state = null;
        }

        switch ((int)newState)
        {
            case (int)AIStateType.Activity:
                {
                    state = gameObject.AddComponent<AIStateActivity>();
                    break;
                }
            case (int)AIStateType.Goto:
                {
                    state = gameObject.AddComponent<AIStateGoto>();
                    break;
                }
            default:
                break;
        }

        if (state == null)
            return;

        if (!state.Init(this))
        {
            state.OnRemove();
            Destroy(state);
            state = null;
            return;
        }

    }

    public AIState GetState() => state;

    public void ClearState()
    {
        if (state == null)
            return;
        state.OnRemove();
        Destroy(state);
        state = null;

        //BlackBoard.SetStateChangeTime(Time.time);
    }

    public bool IsInsideFOV(float distanceSqr, Vector3 dirNorm)
    {
        float fHorizontalDp = Vector3.Dot(dirNorm, GetEyeForward());

        float fFOV = Globals.fFOV180;

        if (distanceSqr >= viewDistance * viewDistance)
        {
            switch (GetAwareness())
            {
                case Awareness.Alert:
                    fFOV = Globals.fFOV140;
                    break;
                case Awareness.Suspicious:
                    fFOV = Globals.fFOV140;
                    break;
                case Awareness.Relaxed:
                    fFOV = Globals.fFOV140;
                    break;
            }
        }

        if (fHorizontalDp <= fFOV)
            return false;

        if (fFOV != Globals.fFOV180)
        {
            float fVerticalDp = Vector3.Dot(dirNorm, transform.up);

            if (fVerticalDp >= Globals.fFOV120)
                return false;
        }

        return true;
    }

    public bool IsPositionVisible(LayerMask blockFilter, Vector3 startPosition, Vector3 endPosition, float maxDistanceSqr, bool useFOV, out float refDistanceSqr)
    {
        Vector3 dir = endPosition - startPosition;

        float distance = dir.magnitude;
        float distanceSqr = dir.sqrMagnitude;

        refDistanceSqr = distanceSqr;

        if (distanceSqr >= maxDistanceSqr)
        {
            return false;
        }

        Vector3 dirNorm = dir.normalized;

        if (useFOV)
        {
            if (!IsInsideFOV(distanceSqr, dirNorm))
            {
                return false;
            }
        }

        RaycastHit hitInfo;

        bool hit = Physics.Raycast(startPosition, dir, out hitInfo, distance, blockFilter);

        return !hit;
    }

    // TODO: redo target visibility logic 
    public bool GetTargetVisibleFromWeapon()
    {
        if (GetTarget() == null)
            return false;

        /*

        float distanceSqr = GetViewDistance() + fTargetRadius;
        fSeeEnemyDistanceSqr *= fSeeEnemyDistanceSqr;

        bool bIsAlert = (m_pAI.GetBlackBoard().GetAwareness() == Awareness.Alert) || (m_pAI.GetBlackBoard().GetAwareness() == Awareness.Suspicious);

        if (m_pAI.IsObjectPositionVisible(m_pAI.eVisibilityBlockMask, vCheckPos, hTarget, vTargetPos, fSeeEnemyDistanceSqr, !bIsAlert, out _))*/

        return true;

    }

    public GameObject GetActiveWeapon() => curWeapon;
    public BaseRangedWeapon GetActiveWeaponInfo() => curWeaponScript;
    public HealthManager GetHealthManager() => healthManager;
    public AIPlan GetAIPlan() => plan;
    public AIPlanner GetAIPlanner() => planner;
    public AIMovement GetAIMovement() => movement;
    public void SetTarget(GameObject newTarget) => target = newTarget;
    public GameObject GetTarget() => target;
    public Vector3 GetTargetPosition() => GetTarget() != null ? GetTarget().transform.position : Vector3.zero;
    public void FaceTarget(bool b) => shouldFaceTarget = b;
    public bool IsFacingTarget() => shouldFaceTarget;
    public void SetPos(Vector3 vPos) => transform.position = vPos;
    public Vector3 GetEyePosition() => eyeTransform.position;
    public Vector3 GetEyeForward() => eyeTransform.forward;
    public void SetShouldUpdatePlan(bool b) => shouldUpdatePlan = b;
    public bool GetShouldUpdatePlan() => shouldUpdatePlan;
    public bool GetShouldSelectAction() => shouldSelectAction;
    public void SetShouldSelectAction(bool b) => shouldSelectAction = b;
    public Vector3 GetPos() => transform.position;
    public Vector3 GetPosition() => GetPos();
    public WS GetAIWorldState() => worldState;
    public bool HasPlayerTarget() => target != null && Globals.IsPlayer(target);
    public void SetAwareness(Awareness newAwareness) => awareness = newAwareness;
    public Awareness GetAwareness() => awareness;
    public float GetViewDistance() => viewDistance;
    public float GetViewDistanceSqr() => viewDistance * viewDistance;
    public GameObject GetSensorContainer() => sensorContainer;

    public float GetEnemyConfidence() => enemyConfidence;
    public void SetEnemyConfidence(float conf) => enemyConfidence = conf;
}

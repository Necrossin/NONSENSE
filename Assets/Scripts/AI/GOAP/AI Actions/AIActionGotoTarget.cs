using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIActionGotoTarget : AIAction
{
	protected override void ApplySettings()
	{
		settings.cost = 4;
		settings.precedence = 1;
		settings.isInterruptible = false;
	}

	public override void InitAction()
	{
		base.InitAction();
		worldStateEffects.SetProperty("AtTargetPosition", true);
	}

	public override bool ValidateContextPreconditions(AICharacter AI, WS worldStateGoal, bool isPlanning)
	{
		if (!isPlanning)
		{
			return true;
		}

		var target = AI.GetTarget();

		if (target == null)
		{
			return false;
		}

		Vector3 dest = target.transform.position;

		if (AI.GetAIMovement().IsAIRoughlyThere(dest, 4))
		{
			return false;
		}

		bool hasPath = AI.GetAIMovement().HasPathToDestination(dest);

		return hasPath;
	}

	public override void ActivateAction(AICharacter AI, WS worldStateGoal)
	{
		base.ActivateAction(AI, worldStateGoal);

		var target = AI.GetTarget();

		if (target == null)//  || pAI.HasTarget((uint)TARGET_TYPE.All)&& pAI.GetBlackBoard().GetTargetPosTrackingFlags() == (uint)TARGET_POS_TRACKING_FLAGS.None)
		{
			return;
		}

		AI.SetState(AIStateType.Goto);
		AIStateGoto stateGoto = (AIStateGoto)AI.GetState();

		if (target != null)
		{
			//stateGoto.SetDestination(target.transform.position);
			stateGoto.SetDestinationObject(target);
			stateGoto.SetDynamicRepathDistance(1f);

			AI.FaceTarget(false);
		}

		else
		{
			//stateGoto.SetDestination(pAI.GetBlackBoard().GetTargetReachableNavMeshPosition());

			//pAI->GetAIBlackBoard()->SetBBTargetTrackerFlags(kTrackerFlag_LookAt);
			//pAI.GetBlackBoard().SetFaceTarget(false);
		}
	}

	public override bool IsActionComplete(AICharacter AI)
	{
		if ((AI.GetState() != null) && (AI.GetState().GetStateStatus() == AIStateStatus.Complete))
			return true;

		return false;
	}
}

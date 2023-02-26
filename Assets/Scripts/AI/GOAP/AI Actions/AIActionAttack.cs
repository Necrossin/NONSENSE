using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIActionAttack : AIAction
{
	protected override void ApplySettings()
	{
		settings.cost = 6;
		settings.precedence = 1;
		settings.isInterruptible = true;
	}

	public override void InitAction()
	{
		base.InitAction();
		worldStateEffects.SetProperty("TargetIsDead", true);
	}

	public override bool ValidateContextPreconditions(AICharacter AI, WS worldStateGoal, bool isPlanning)
	{
		if (AI.GetTarget() == null)
			return false;

		// temporary
		if (AI.GetEnemyConfidence() < 0.2f)
			return false;

		if (!AI.GetTargetVisibleFromWeapon())
			return false;

		if (!AI.IsCurWeaponInRange())
			return false;

		return true;
	}

	public override void ActivateAction(AICharacter AI, WS worldStateGoal)
	{
		base.ActivateAction(AI, worldStateGoal);

		AI.SetState(AIStateType.Activity);
		AIStateActivity state = (AIStateActivity)AI.GetState();

		state.SetAction(AI_Action.FireWeapon);
		state.SetDuration(AI.GetActiveWeaponInfo().GetFireDelay());

		AI.FaceTarget(true);
	}

	public override bool ValidateAction(AICharacter AI)
	{
		if (!base.ValidateAction(AI))
			return false;

		// temporary
		if (AI.GetEnemyConfidence() < 0.2f)
			return false;

		if (!AI.GetTargetVisibleFromWeapon())
			return false;

		if (!AI.IsCurWeaponInRange())
			return false;

		return true;
	}

	public override bool IsActionComplete(AICharacter AI)
	{
		if (AI.GetState() != null && AI.GetState().GetStateStatus() == AIStateStatus.Complete)
		{
			return true;
		}

		return false;
	}

}

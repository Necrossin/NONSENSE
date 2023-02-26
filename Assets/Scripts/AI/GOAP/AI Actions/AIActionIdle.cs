using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AIActionIdle : AIAction
{
	protected override void ApplySettings()
	{
		settings.cost = 2;
		settings.precedence = 1;
		settings.isInterruptible = true;
	}

	public override void InitAction()
	{
		base.InitAction();
		worldStateEffects.SetProperty("Idling", true);
	}

	public override void ActivateAction(AICharacter AI, WS worldStateGoal)
	{
		base.ActivateAction(AI, worldStateGoal);

		if (AI.GetState() == null || AI.GetAIMovement().GetStatus() != MOVEMENT_STATUS.None)
		{
			AI.SetState(AIStateType.Activity);
		}

		if (AI.GetTarget() != null)
		{
			AI.FaceTarget(true);
		}
		else
		{
			AI.FaceTarget(false);
		}
	}
}

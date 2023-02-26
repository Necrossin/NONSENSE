using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AISensorVisual : AISensor
{
	Vector2 viewGrid = new Vector2(-1, 0);
	GameObject lastTarget;

	public override float GetUpdateRate() => 0.33f; //0.15

	/*public override void UpdateSensor()
    {
		base.UpdateSensor();
	}*/


	public override bool StimulateSensor()
	{
		if (AI == null)
			return false;

		float rateModifier = 1;
		if (!ComplexCheck(ref rateModifier))
			return false;

		float confidence = IncreaseStimulation(AI.GetEnemyConfidence(), rateModifier);
		AI.SetEnemyConfidence(confidence);

		return true;
	}

	public override float IncreaseStimulation(float curStimulation, float rateModifier)
	{
		float stimulation = base.IncreaseStimulation(curStimulation, rateModifier);

		if (curStimulation < 1 && stimulation > 1)
		{
			AI.SetAwareness(Awareness.Alert);

			GameObject hCurTarget = AI.GetTarget();
		}

		if (stimulation >= 1 && !AI.HasPlayerTarget() && Globals.Player != null)
        {
			AI.SetTarget(Globals.Player);
			AI.SetShouldSelectAction(true);
        }

		return stimulation;
	}

	public override void DestimulateSensor()
	{
		float confidence = DecreaseStimulation(AI.GetEnemyConfidence());
		AI.SetEnemyConfidence(confidence);
	}

	public override bool ComplexCheck( ref float rateModifier )
	{
		float distanceSqr = AI.GetViewDistanceSqr();

		GameObject target = Globals.Player;

		if (target == null)
			return false;

		float xIncr = 0.5f;
		float yIncr = 0.25f;

		Vector3 position = target.transform.position;
		Vector3 dims = Globals.GetObjectDims(target);

		Vector3 dirToTarget = position - AI.GetEyePosition();
		
		if (Vector3.Dot(dirToTarget, dirToTarget) <= 0.1f * 0.1f)
		{
			return true;
		}

		dirToTarget.Normalize();

		Vector3 up;
		Vector3 right;
		if (Mathf.Abs(Vector3.Dot(Vector3.up, dirToTarget)) > 0.98)
		{
			right = Vector3.Cross(Globals.BuildOrthonormalVector3(dirToTarget), dirToTarget);
			right.Normalize();

			up = Vector3.Cross(right, dirToTarget);
			up.Normalize();
		}
		else
		{
			right = Vector3.Cross(Vector3.up, dirToTarget);
			right.Normalize();

			up = Vector3.up;
		}


		float x = dims.x * viewGrid.x * 0.5f;
		float y = dims.y * viewGrid.y;

		position += right * x;
		position += up * y;

		float refDistanceSqr = 0;
		bool visible = AI.IsPositionVisible(AI.visibilityBlockMask, AI.GetEyePosition(), position, distanceSqr, true, out refDistanceSqr);

		Debug.DrawLine(AI.GetEyePosition(), position, visible ? Color.red : Color.white, GetUpdateRate());

		if (visible)
		{
			float instantSeeDistSqr;
			if ((AI.GetAwareness() == Awareness.Alert))
				{
					instantSeeDistSqr = distanceSqr * distanceSqr * 100;
				}
				else
				{
					instantSeeDistSqr = distanceSqr * distanceSqr * 0.3f;
				}

				if (refDistanceSqr > instantSeeDistSqr)
				{
					rateModifier = 1.0f - (refDistanceSqr / distanceSqr);
				}
				else
				{
					rateModifier = 99999999;
				}
			}
		else
		{
			if (lastTarget != target)
			{
				lastTarget = target;
				viewGrid.y = Random.Range(-1, 1);
				viewGrid.x = Random.Range(-1, 1);
			}
			else
            {
				viewGrid.y += yIncr;
				if (viewGrid.y > 1)
				{
					viewGrid.y = 0;
					viewGrid.x += xIncr * 2;

					if (viewGrid.x > 1)
					{
						viewGrid.x = -1;
					}
				}
			}
		}

		//Debug.Log("See target  "+bVisible);

		return visible;
	}
}

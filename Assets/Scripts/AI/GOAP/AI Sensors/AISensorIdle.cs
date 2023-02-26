using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AISensorIdle : AISensor
{
	public override float GetUpdateRate() => 0.5f;

	public override void UpdateSensor()
    {
		if (AI == null)
			return;

		AI.SetShouldSelectAction(true);
	}

}

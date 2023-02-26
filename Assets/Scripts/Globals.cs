using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Globals
{
	public static float fFOV180 = 0f;
	public static float fFOV140 = 0.3420201433257f;
	public static float fFOV120 = 0.5f;


	public static Vector3 BuildOrthonormalVector3( Vector3 inp )
	{

		Vector3 rv = new Vector3();
		Vector3 vAbs = new Vector3((inp.x < 0)? -inp.x : inp.x, (inp.y < 0) ? -inp.y : inp.y, (inp.z < 0) ? -inp.z : inp.z);

		if ((vAbs.x <= vAbs.y) && (vAbs.x <= vAbs.z))
			rv.Set(0, -inp.z, inp.y);
		else if ((vAbs.y <= vAbs.x) && (vAbs.y <= vAbs.z))
			rv.Set(-inp.z, 0, inp.x);
		else
			rv.Set(-inp.y, inp.x, 0);

		rv.Normalize();
		return rv;
	}


	public static Vector3 GetObjectDims(GameObject g)
	{
		Bounds b;// = new Bounds(g.transform.position, Vector3.zero);

		var charController = g.GetComponent<CharacterController>();
		if (charController != null)
        {
			b = new Bounds(g.transform.position + charController.center, new Vector3(charController.radius*2, charController.height, charController.radius*2));
			return b.size;
		}
		else
			b = new Bounds(g.transform.position, Vector3.zero);


		foreach (Collider r in g.GetComponentsInChildren<Collider>())
		{
			if (r.isTrigger)
				continue;

			b.Encapsulate(r.bounds);
		}
		return b.size;
	}

	public static bool IsWorld( GameObject hObj )
    {
		if (hObj != null)
			return hObj.layer == 9;
		return false;
    }

	public static bool IsDeadAI( GameObject obj )
    {
		return false;
    }

	public static bool IsCharacter(GameObject hObj)
	{
		return IsPlayer(hObj) || IsAI(hObj);
	}

	public static bool IsPlayer(GameObject hObj)
	{
		return hObj != null && hObj.GetComponent<CharacterController>() != null;
	}

	public static bool IsAI(GameObject hObj)
    {
		return hObj != null && hObj.GetComponent<AICharacter>() != null;
	}
}

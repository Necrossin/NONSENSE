using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RigToIK : MonoBehaviour
{
    public bool IKEnabled = true;

    [SerializeField]
    List<Rigidbody> bodies;

    [SerializeField]
    List<Transform> goal;

    [SerializeField]
    List<ConfigurableJoint> joints;

    [SerializeField]
    Rigidbody handBody;

    [SerializeField]
    Transform handIK;

    private JointDrive defDrive, defSlerp, zeroDrive;

    private void Awake()
    {
        defDrive = new JointDrive() { maximumForce = Mathf.Infinity, positionDamper = 0, positionSpring = 5000f };
        defSlerp = new JointDrive() { maximumForce = Mathf.Infinity, positionDamper = 0, positionSpring = 1000f };

        zeroDrive = new JointDrive() { maximumForce = 0, positionDamper = 0, positionSpring = 0 };
    }

    void FixedUpdate()
    {
        if (!IKEnabled) return;

        if (handBody == null || handIK == null) return;

        float maxDist = 0.05f;
        var distSqr = (handIK.position - handBody.transform.position).sqrMagnitude;

        for (int i = 0; i < joints.Count; i++)
        {
            var curJoint = joints[i];

            if (curJoint != null)
            {
                if (distSqr <= maxDist * maxDist)
                {
                    if (curJoint.xDrive.maximumForce < 1)
                    {
                        curJoint.xDrive = defDrive;
                        curJoint.yDrive = defDrive;
                        curJoint.zDrive = defDrive;

                        curJoint.slerpDrive = defSlerp;
                    }
                }
                else
                {
                    if (curJoint.xDrive.maximumForce > 1)
                    {
                        curJoint.xDrive = zeroDrive;
                        curJoint.yDrive = zeroDrive;
                        curJoint.zDrive = zeroDrive;

                        curJoint.slerpDrive = zeroDrive;
                    }
                }
            }
        }
    }
}

//
// Jiggle Bone (Simple version)
//   -  by WarpZone
//
//  TO USE:
//  Simply attach this script to any Transform in your model's hierarchy.
//
//  FEATURES:
//    -  It doesn't matter what the forward normal of your bone is, it just works.
//    -  Bounce and sway can be configured independently
//    -  Simple script is Simple.
//
// If you want to call the bounce code using some sort of manager script, comment out the JiggleBonesUpdate call and SendMessage JiggleBonesUpdate from your Manager instead.
// This is useful if you want to control which bones update first, or strategically layer other procedural animation effects at runtime.
// Modified in order to clamp angle components

using UnityEngine;

public class JiggleBone : MonoBehaviour
{

    public float bounceFactor = 20;
    public float wobbleFactor = 10;

    public float maxTranslation = 0.05f;

    public bool useWeight = false;
    public Transform baseBody;

    public Transform weightPoint;

    public Vector2 xLimit = new Vector2( 0, 0 );
    public Vector2 yLimit = new Vector2( 0, 0 );
    public Vector2 zLimit = new Vector2( 0, 0 );

    private Vector3 oldBoneWorldPosition;
    private Quaternion oldBoneWorldRotation;
    private Vector3 animatedBoneWorldPosition;
    private Quaternion animatedBoneWorldRotation;
    private Quaternion goalRotation;
    private Vector3 goalPosition;
    private Vector3 localAngles;
    private float maxRotation;
    private float weightScale;
    private Vector3 weightDir;

    void Awake()
    {
        oldBoneWorldPosition = transform.position;
        oldBoneWorldRotation = transform.rotation;
    }

    void LateUpdate()
    {
        JiggleBonesUpdate();
    }

    void JiggleBonesUpdate()
    {
        weightScale = 0;

        animatedBoneWorldPosition = transform.position;
        animatedBoneWorldRotation = transform.rotation;

        var slerpRot = transform.rotation;

        if (useWeight && weightPoint != null)
        {
            weightScale = Vector3.Dot((baseBody != null) ? baseBody.up : transform.up, Vector3.up ) > 0 ? 0 : 1 ;
            weightDir = (weightPoint.position - transform.position).normalized;

            //if (weightScale == 1)
                slerpRot = Quaternion.FromToRotation(weightDir, Vector3.up * -1);
        }


        goalPosition = Vector3.Slerp(oldBoneWorldPosition, transform.position, Time.deltaTime * bounceFactor);
        goalRotation = Quaternion.Slerp(oldBoneWorldRotation, slerpRot, Time.deltaTime * wobbleFactor * ( 1 - weightScale ) );

        maxRotation = Mathf.Max(xLimit.x, xLimit.y, yLimit.x, yLimit.y, zLimit.x, zLimit.y);

        transform.rotation = Quaternion.RotateTowards(animatedBoneWorldRotation, goalRotation, maxRotation);
        transform.position = Vector3.MoveTowards(animatedBoneWorldPosition, goalPosition, maxTranslation);

        localAngles = transform.localEulerAngles;

        transform.localEulerAngles = new Vector3(Mathf.Clamp(localAngles.x, xLimit.x, xLimit.y), Mathf.Clamp(localAngles.y, yLimit.x, yLimit.y), Mathf.Clamp(localAngles.z, zLimit.x, zLimit.y));

        oldBoneWorldPosition = transform.position;
        oldBoneWorldRotation = transform.rotation;
    }

}
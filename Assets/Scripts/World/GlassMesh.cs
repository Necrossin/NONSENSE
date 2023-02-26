using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(Rigidbody))]
public class GlassMesh : MonoBehaviour
{
    [SerializeField]
    [Range(1, 6)]
    int minChunksPerSide = 1;

    [SerializeField]
    [Range(1, 6)]
    int maxChunksPerSide = 1;

    [SerializeField]
    GameObject ChunkPrefab;

    [SerializeField]
    AudioSource glassSfx;

    Rigidbody rb;
    BoxCollider boxCollider;
    MeshRenderer meshRenderer;
    Collider[] edgeColliders;

    List<Vector3> CornerVertices = new List<Vector3>();

    Vector3[] DefaultCorners = {
        new Vector3(-0.5f, -0.5f,0),
        new Vector3(-0.5f, 0.5f,0),
        new Vector3(0.5f, 0.5f,0),
        new Vector3(0.5f, -0.5f,0),
    };


    int[] chunkTriangles3D =
    {
        0, 1, 2, // face 1
        3, 5, 4, // face 2

        0, 3, 4,
        0, 4, 1,

        2, 1, 4,
        2, 4, 5,

        2, 3, 0,
        2, 5, 3,
    };


    int[] chunkTrianglesFractured3D =
    {
        //face 1
        0, 1, 2,
        0, 2, 3,

        //face 2
        4, 6, 5,
        4, 7, 6,

        4, 1, 0,
        4, 5, 1,

        4, 0, 7,
        0, 3, 7,

        1, 6, 2,
        1, 5, 6,

        2, 6, 7,
        2, 7, 3,
    };

    struct GlassJointData
    {
        public GlassJointData( Vector3 vPos, Vector3 vAxisStart, Vector3 vAxisEnd )
        {
            vJointPos = vPos;
            vJointAxisStart = vAxisStart;
            vJointAxisEnd = vAxisEnd;
        }
        
        public Vector3 vJointPos;
        public Vector3 vJointAxisStart;
        public Vector3 vJointAxisEnd;
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        meshRenderer = GetComponent<MeshRenderer>();
        boxCollider = GetComponent<BoxCollider>();

        GenerateCorners();

        CheckEdges();
        //Break(transform.TransformPoint(new Vector3(Random.Range( -0.4f, 0.4f) , Random.Range(-0.4f, 0.4f))), Vector3.right * -1, 3f);
        //Time.timeScale = 0.1f;
        //Debug.Break();

        Globals.TryResolvingAudioProbes(gameObject);
    }   
    
    void GenerateCorners()
    {
        int chunksPerSide = Random.Range( Mathf.Min( minChunksPerSide, maxChunksPerSide), Mathf.Max(minChunksPerSide, maxChunksPerSide));
        int angle = 45;
        int prevAngle = angle * 1;

        float sin, cos;
        float sin2, cos2;

        for (int _ = 1; _ <= 4; _++)
        {
            sin = Mathf.Sin(angle * Mathf.Deg2Rad);
            cos = Mathf.Cos(angle * Mathf.Deg2Rad);

            sin = Mathf.Clamp(sin, -0.5f, 0.5f);
            cos = Mathf.Clamp(cos, -0.5f, 0.5f);

            sin2 = sin * 1;
            cos2 = cos * 1;

            CornerVertices.Add(new Vector3(cos, sin, 0));

            for (int i=0; i < (chunksPerSide - 1); i++)
            {
                int randAngle = Random.Range(angle - 90, angle);

                sin = Mathf.Sin(randAngle * Mathf.Deg2Rad) * 0.5f;
                cos = Mathf.Cos(randAngle * Mathf.Deg2Rad) * 0.5f;

                if (_ == 1 || _ == 3)
                    cos = cos2 * 1;

                if (_ == 2 || _ == 4)
                    sin = sin2 * 1;

                if (Mathf.Abs(randAngle - prevAngle) < 5)
                    continue;

                if (randAngle > prevAngle)
                {
                    CornerVertices.Insert(CornerVertices.Count - 1, new Vector3(cos, sin, 0));
                }
                else
                {
                    CornerVertices.Add(new Vector3(cos, sin, 0));
                }
                    
                prevAngle = randAngle;
            }

            angle -= 90;
            prevAngle = angle * 1;
        }
    }

    void CheckEdges()
    {
        edgeColliders = Physics.OverlapBox(transform.position, (transform.localScale * 1.01f ) / 2, transform.rotation, 1 << 9);
    }

    public void Break(Vector3 vPoint, Vector3 vDir, float fForce)
    {
        glassSfx.pitch = Random.Range(1.1f, 1.3f);
        glassSfx.Play();

        fForce *= Random.Range(0.8f, 1.2f);

        var newVertices = new Vector3[CornerVertices.Count + 1];

        for (int i = 0; i < CornerVertices.Count; i++)
        {
            newVertices[i] = CornerVertices[i];
        }

        var localHitPos = transform.InverseTransformPoint(vPoint);
        localHitPos.z = 0;

        if (vPoint == Vector3.zero)
            localHitPos = Vector3.zero;

        //safety first
        localHitPos.x = Mathf.Clamp(localHitPos.x, -0.5f, 0.5f);
        localHitPos.y = Mathf.Clamp(localHitPos.y, -0.5f, 0.5f);

        newVertices[CornerVertices.Count] = localHitPos;

        for (int i = 0; i < CornerVertices.Count; i++)
        {
            var chunkVertices = new Vector3[3];
            var next_i = (i + 1) >= CornerVertices.Count ? 0 : i + 1;

            chunkVertices[0] = newVertices[i];
            chunkVertices[1] = newVertices[next_i];
            chunkVertices[2] = newVertices[CornerVertices.Count];

            var fDistSqr = Mathf.Max((chunkVertices[2] - chunkVertices[0]).sqrMagnitude, (chunkVertices[1] - chunkVertices[0]).sqrMagnitude);

            CreateChunk(chunkVertices, vPoint, vDir, fForce, fDistSqr > (0.25f * 0.25f), false);
        }

        GetComponent<MeshFilter>().mesh = null;
        boxCollider.enabled = false;
        rb.isKinematic = true;
    }

    void CreateChunk(Vector3[] vertices, Vector3 vPoint, Vector3 vDir, float fForce, bool bFractureAgain, bool isBigChunk)
    {
        bool bIsEdge = true;
        
        var verticesEx = new Vector3[vertices.Length * 2];

        for (int i = 0; i < vertices.Length; i++)
        {
            verticesEx[i] = vertices[i];
            verticesEx[i].z = -0.5f;
            verticesEx[i + vertices.Length] = vertices[i];
            verticesEx[i + vertices.Length].z = 0.5f;
        }

        var Chunk = Pool.Instance.InstantiateFromPool(ChunkPrefab, Vector3.zero, Quaternion.identity);

        if (Chunk == null)
            return;

        // clean up joints from reused objects, because I'm lazy and can't have a script per each chunk
        var joints = Chunk.GetComponents<HingeJoint>();

        foreach (var joint in joints)
            Destroy(joint);

        Chunk.transform.parent = gameObject.transform;
        Chunk.transform.localPosition = Vector3.zero;
        Chunk.transform.localRotation = Quaternion.identity;
        Chunk.transform.localScale = Vector3.one;

        Chunk.transform.SetParent(null, true);

        Chunk.layer = gameObject.layer;

        var meshFilter = Chunk.GetComponent<MeshFilter>();
        var meshRenderer = Chunk.GetComponent<MeshRenderer>();
        var meshCollider = Chunk.GetComponent<MeshCollider>();
        var rb = Chunk.GetComponent<Rigidbody>();

        //small inner chunks
        if (bFractureAgain && !isBigChunk)
        {
            var extraVertices = new Vector3[4];
            extraVertices[0] = verticesEx[0] * 1;
            extraVertices[1] = verticesEx[1] * 1;

            var dir1 = (verticesEx[0] - verticesEx[2]).normalized;
            var dir2 = (verticesEx[1] - verticesEx[2]).normalized;

            var dist1 = (verticesEx[0] - verticesEx[2]).magnitude;
            var dist2 = (verticesEx[1] - verticesEx[2]).magnitude;

            float rand1 = Random.Range(0.1f, 0.15f);
            float rand2 = Random.Range(0.1f, 0.15f);

            verticesEx[0] = verticesEx[2] + dir1 * dist1 * rand1;
            verticesEx[1] = verticesEx[2] + dir2 * dist2 * rand2;

            verticesEx[vertices.Length] = verticesEx[2 + vertices.Length] + dir1 * dist1 * rand1;
            verticesEx[vertices.Length + 1] = verticesEx[2 + vertices.Length] + dir2 * dist2 * rand2;

            extraVertices[2] = verticesEx[1] * 1;
            extraVertices[3] = verticesEx[0] * 1;

            var fDistSqr = Mathf.Max((extraVertices[2] - extraVertices[1]).sqrMagnitude, (extraVertices[3] - extraVertices[0]).sqrMagnitude);
            bool bSubdivide = fDistSqr > (0.45f * 0.45f);

            bIsEdge = false;

            //we only need to send 2d data, because extrusion will happen after
            CreateChunk(extraVertices, vPoint, vDir, fForce * Random.Range(0.85f, 0.95f), bSubdivide, true);
        }

        // extra division
        if (bFractureAgain && isBigChunk)
        {
            var extraVertices = new Vector3[4];
            extraVertices[0] = verticesEx[0] * 1;
            extraVertices[1] = verticesEx[1] * 1;

            var dir1 = (verticesEx[0] - verticesEx[3]).normalized;
            var dir2 = (verticesEx[1] - verticesEx[2]).normalized;

            var dist1 = (verticesEx[0] - verticesEx[3]).magnitude;
            var dist2 = (verticesEx[1] - verticesEx[2]).magnitude;

            float rand1 = Random.Range(0.25f, 0.45f);
            float rand2 = Random.Range(0.25f, 0.45f);

            verticesEx[0] = verticesEx[3] + dir1 * dist1 * rand1;
            verticesEx[1] = verticesEx[2] + dir2 * dist2 * rand2;

            verticesEx[vertices.Length] = verticesEx[3 + vertices.Length] + dir1 * dist1 * rand1;
            verticesEx[vertices.Length + 1] = verticesEx[2 + vertices.Length] + dir2 * dist2 * rand2;

            extraVertices[2] = verticesEx[1] * 1;
            extraVertices[3] = verticesEx[0] * 1;

            bIsEdge = false;

            CreateChunk(extraVertices, vPoint, vDir, fForce * Random.Range(0.45f, 0.65f), false, true);
        }

        var mesh = new Mesh
        {
            name = "Glass Mesh"
        };

        mesh.vertices = verticesEx;

        var normals = new Vector3[verticesEx.Length];
        for (int i = 0; i < verticesEx.Length; i++)
        {
            normals[i] = i >= vertices.Length ? Vector3.forward : Vector3.back;
        }

        bool bigChunk = isBigChunk;

        mesh.triangles = bigChunk ? chunkTrianglesFractured3D : chunkTriangles3D;
        mesh.normals = normals;

        mesh.RecalculateBounds();
        mesh.Optimize();

        meshFilter.mesh = mesh;
        meshCollider.sharedMesh = mesh;

        var mat = this.meshRenderer.material;
        meshRenderer.material = mat;

        if (bIsEdge)
        {
            List<GlassJointData> Joints = new List<GlassJointData>();
            int connectedPoints = 0;

            for (int i = 0; i < edgeColliders.Length; i++)
            {
                var collider = edgeColliders[i];

                var vPoint1 = transform.TransformPoint(verticesEx[0]);
                var vPoint2 = transform.TransformPoint(verticesEx[1]);

                var dist1 = (collider.ClosestPointOnBounds(vPoint1) - vPoint1).sqrMagnitude;
                var dist2 = (collider.ClosestPointOnBounds(vPoint2) - vPoint2).sqrMagnitude;

                float tolerance = 0.02f;

                if (dist1 <= tolerance * tolerance)
                {
                    connectedPoints += 1;

                    var JointData = new GlassJointData(verticesEx[0], vPoint1, vPoint2);
                    Joints.Add(JointData);
                }

                if ( dist2 <= tolerance * tolerance)
                {
                    var JointData = new GlassJointData(verticesEx[1], vPoint1, vPoint2);
                    Joints.Add(JointData);

                    connectedPoints += 1;
                }

                if (connectedPoints >= 2)
                    break;
            }

            if (connectedPoints < 2)
                bIsEdge = false;
            else
            {
                if (Chunk.GetComponent<HingeJoint>() == null)
                {
                    for (int i=0; i<Joints.Count; i++)
                    {
                        var vJointPos = Joints[i].vJointPos;
                        vJointPos.z = 0;

                        var vAxisStart = Chunk.transform.InverseTransformPoint(Joints[i].vJointAxisStart);
                        var vAxisEnd = Chunk.transform.InverseTransformPoint(Joints[i].vJointAxisEnd);

                        var vJointAxis = (vAxisEnd - vAxisStart).normalized;

                        var joint = Chunk.AddComponent<HingeJoint>();

                        joint.connectedBody = this.rb;
                        joint.enableCollision = false;
                        joint.axis = vJointAxis;
                        joint.anchor = vJointPos;
                        joint.breakForce = Mathf.Infinity;
                        joint.breakTorque = Random.Range(170,180);

                        joint.useLimits = true;

                        break;
                    }
                }
            }
        }

        var vForceDir = (Chunk.transform.TransformPoint(meshCollider.bounds.center) - vPoint).normalized;
        var rotDir = Vector3.Cross(vDir, vForceDir).normalized;

        if (bigChunk)
            rb.mass *= Random.Range(1.5f, 1.9f);
        else
            rb.mass *= Random.Range(0.5f, 0.9f);

        rb.AddForce(vDir * fForce * (bIsEdge ? 2 : 1), ForceMode.Impulse);
        rb.AddTorque(rotDir * fForce * (bigChunk ? 0.1f : 0.15f), ForceMode.Impulse);
            
        /*if (!bIsEdge)
            Destroy(Chunk, 40);*/
    }

    void HideChunks( GameObject chunk, Mesh meshRef )
    {
        if (chunk == null)
            return;
        
        var meshFilter = chunk.GetComponent<MeshFilter>();

        if (meshFilter.mesh == meshRef)
            Pool.Instance.ReturnToPool(chunk);
    }

    private void OnCollisionEnter(Collision collision)
    {
        ContactPoint contact = collision.contacts[0];
        Vector3 vHitPos = contact.point;

        if (collision.relativeVelocity.sqrMagnitude > 3 * 3)
        {
            Break(vHitPos, collision.relativeVelocity.normalized, collision.relativeVelocity.magnitude * 0.3f);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        var other_rb = other.attachedRigidbody;

        if (other_rb == null)
            return;

        var vCenter = other_rb.worldCenterOfMass;
        var vHitPos = boxCollider.ClosestPointOnBounds(vCenter);

        Break(vHitPos, (vHitPos- vCenter).normalized, other_rb.velocity.magnitude);
    }
}

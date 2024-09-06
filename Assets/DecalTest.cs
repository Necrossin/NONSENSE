using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

[ExecuteInEditMode]
public class DecalTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void DoStuff()
    {
        RaycastHit hitInfo;

        DecalProjector projector = GetComponent<DecalProjector>();

        bool hit = Physics.Raycast(transform.position, transform.forward, out hitInfo, 1000, 1 << 9);

        if (hit)
        {
            transform.position = hitInfo.point + hitInfo.normal * 0.001f;

            float angle = Vector3.Angle(transform.forward, hitInfo.normal) - 90;
            float dist = 0.3f / Mathf.Tan(Mathf.Deg2Rad * angle);

            projector.size = new Vector3(projector.size.x, projector.size.y, dist);

            Debug.Log(dist);



        }
    }

    private void OnEnable()
    {
        DoStuff();
    }
}

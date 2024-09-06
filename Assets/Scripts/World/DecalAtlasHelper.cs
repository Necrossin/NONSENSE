using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class DecalAtlasHelper : MonoBehaviour
{
    private float matWidth, matHeight;

    DecalProjector projector;

    private int wProp = Shader.PropertyToID("_Width");
    private int hProp = Shader.PropertyToID("_Height");
    private int dirProp = Shader.PropertyToID("_Direction");

    void Awake()
    {
        projector = GetComponent<DecalProjector>();
    }

    public void UpdateDecal( Material decalMat )
    {
        if (decalMat == null) return;
        if (projector == null) return;

        projector.material = decalMat;

        var matProjector = projector.material;

        matWidth = matProjector.GetFloat(wProp);
        matHeight = matProjector.GetFloat(hProp);

        int offX = Random.Range(1, (int)matWidth + 1) - 1;
        int offY = Random.Range(1, (int)matHeight + 1) - 1;

        projector.uvBias = new Vector2(offX, offY);
    }

    public void UpdateDecal(Material decalMat, Vector3 hitNormal)
    {
        if (decalMat == null) return;
        if (projector == null) return;

        projector.material = decalMat;

        var matProjector = projector.material;

        matWidth = matProjector.GetFloat(wProp);
        matHeight = matProjector.GetFloat(hProp);

        int offX = Random.Range(1, (int)matWidth + 1) - 1;
        int offY = Random.Range(1, (int)matHeight + 1) - 1;

        projector.uvBias = new Vector2(offX, offY);

        float angle = Vector3.Angle(transform.forward, hitNormal) - 90;
        float dist = projector.size.x / Mathf.Tan(Mathf.Deg2Rad * angle);

        projector.size = new Vector3(projector.size.x, projector.size.y, dist);
    }


    /*void UpdateDecal()
    {
        if (projector == null) return;

        int offX = Random.Range(1, (int)matWidth + 1) - 1;
        int offY = Random.Range(1, (int)matHeight + 1) - 1;

        projector.uvBias = new Vector2(offX, offY);
    }*/

    private void OnEnable()
    {
        //UpdateDecal();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class FX_ItemPull : MonoBehaviour
{

    private ParticleSystem effect;

    [SerializeField]
    private Transform startPos;
    [SerializeField]
    private Transform endPos;

    private int particleNum = 16;

    private float nextEmit = 0;

    private int curEmission = 0;


    void Start()
    {
        effect = GetComponent<ParticleSystem>();
    }

    
    void Update()
    {
        if ( startPos != null && endPos != null )
        {
            DoEmission();
        }

    }

    private void DoEmission()
    {

        if (nextEmit > Time.time)
            return;

        if (curEmission >= (particleNum + 1))
            return;

        var fxParams = new ParticleSystem.EmitParams();
        fxParams.applyShapeToPosition = true;

        Vector3 pStart = startPos.position;
        Vector3 pEnd = endPos.position;

        Vector3 dir = (pEnd - pStart).normalized;
        float dist = (pEnd - pStart).magnitude;

        float delta = curEmission / (float)particleNum;
        fxParams.position = pStart + dir * delta * dist;
        effect.Emit(fxParams, 1);

        curEmission += 1;
        nextEmit = Time.time + 0.001f * Time.deltaTime;
    }

    public void SetPositions( Transform start, Transform end )
    {
        startPos = start;
        endPos = end;
        transform.SetParent(end);
    }
}

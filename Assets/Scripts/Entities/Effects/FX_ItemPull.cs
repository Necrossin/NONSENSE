using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class FX_ItemPull : MonoBehaviour
{

    private ParticleSystem effect;
    //private ParticleSystem.Particle[] curParticles;

    //private Vector3[] offsetPos;

    [SerializeField]
    private Transform startPos;
    [SerializeField]
    private Transform endPos;

    private int particleNum = 16;

    private float nextEmit1 = 0;
    private float nextEmit = 0;

    private int curEmission = 0;


    void Start()
    {
        effect = GetComponent<ParticleSystem>();
        //curParticles = new ParticleSystem.Particle[effect.main.maxParticles];
        //offsetPos = new Vector3[effect.main.maxParticles];
    }

    
    void Update()
    {
        if ( startPos != null && endPos != null )
        {
                        
            DoEmission();
            //MaintainOffset();
        }
            



        /*if ( nextEmit1 <= Time.time )
        {
            curEmission = 0;
            nextEmit1 = Time.time + effect.main.duration * 2;
        }*/

    }

    private void DoEmission()
    {

        if (nextEmit > Time.time)
            return;

        if (curEmission >= (particleNum + 1))
            return;

        var fxParams = new ParticleSystem.EmitParams();
        fxParams.applyShapeToPosition = true;

        Vector3 pStart = startPos.position;//;transform.InverseTransformPoint(startPos.position) 
        Vector3 pEnd = endPos.position;//transform.InverseTransformPoint(endPos.position);

        //offsetPos[curEmission] = pStart;

        Vector3 dir = (pEnd - pStart).normalized;
        float dist = (pEnd - pStart).magnitude;

        float delta = curEmission / (float)particleNum;
        fxParams.position = pStart + dir * delta * dist;
        effect.Emit(fxParams, 1);

        curEmission += 1;
        nextEmit = Time.time + 0.001f * Time.deltaTime;
    }

    /*private void MaintainOffset()
    {

        int num = effect.GetParticles(curParticles);

        for (int i = 0; i < num; i++)
        {
            Vector3 pStart = transform.InverseTransformPoint(startPos.position);//startPos.position; 
            Vector3 pEnd = transform.InverseTransformPoint(endPos.position);//endPos.position;

            Vector3 dir = (pEnd - pStart).normalized;
            float dist = (pEnd - pStart).magnitude;

            float delta = i / (float)num;

            Vector3 offset = offsetPos[i] - curParticles[i].position;

            curParticles[i].position = pStart + dir * delta * dist + offset;

            offsetPos[i] = curParticles[i].position;
        }

        effect.SetParticles(curParticles, num);

    }*/

    public void SetPositions( Transform start, Transform end )
    {
        startPos = start;
        endPos = end;
        transform.SetParent(end);
    }
}

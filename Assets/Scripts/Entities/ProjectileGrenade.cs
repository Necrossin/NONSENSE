using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

// make an interface maybe?
//[ExecuteInEditMode]
public class ProjectileGrenade : MonoBehaviour
{
    [SerializeField]
    ParticleSystem fxDrip;
    //[SerializeField]
    //Renderer blobRenderer;
    [SerializeField]
    VisualEffect grenadeBodyVFX;

    [SerializeField]
    GameObject grenadeExplosionFX;
    //AudioSource explosionSnd;
    public bool isPrimed { get; set; } = false;
    public int primeTimer { get; set; } = 4;

    float primeDelta = 0;
    float primeDeltaTime = 0;
    float primeAnimDuration = 0.5f;

    float fadeDelta = 0;
    float fadeDeltaTime = 0;
    float fadeAnimDuration = 0.3f;


    MaterialPropertyBlock GrenadeMatProps;
    
    void Start()
    {
        //explosionSnd = GetComponent<AudioSource>();
    }

    
    void Update()
    {
        if (!isPrimed)
        {
            if (fadeDelta < 1)
            {
                fadeDelta = Mathf.Clamp01(1 - (fadeDeltaTime - Time.time) / fadeAnimDuration);
                grenadeBodyVFX.SetFloat("Fade Delta", fadeDelta);
            }
            return;
        }
            

        if (primeDelta < 1)
        {
            primeDelta = Mathf.Clamp01( 1 - (primeDeltaTime - Time.time) / primeAnimDuration);
            grenadeBodyVFX.SetFloat("Prime Delta", primeDelta);
        }
            
    }

    public void Prime()
    {
        fxDrip.Play();

        isPrimed = true;
        primeDeltaTime = Time.time + primeAnimDuration;
        grenadeBodyVFX.SetBool("Primed", true);
        grenadeBodyVFX.SendEvent("OnPrimed");

        //TODO: more
    }

    public void OnSpawn()
    {
        fadeDelta = 0;
        fadeDeltaTime = Time.time + fadeAnimDuration;
    }

    public void OnThrow()
    {
        var main = fxDrip.main;
        main.simulationSpace = ParticleSystemSimulationSpace.World;

        //Destroy(gameObject, 20);
        Invoke("Detonate", primeTimer);
    }

    void Detonate()
    {
        _ = Instantiate(grenadeExplosionFX, transform.position + Vector3.up * 0.3f, Quaternion.identity);

        //explosionSnd.pitch = Random.Range(0.9f, 1.1f);
        //explosionSnd.PlayOneShot(explosionSnd.clip);

        Destroy(gameObject);
    }
}

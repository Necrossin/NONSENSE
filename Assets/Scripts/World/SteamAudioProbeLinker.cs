using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SteamAudio;

public partial class Globals
{
    public static SteamAudioProbeBatch SceneAudioProbeBatch;

    public static void TryResolvingAudioProbes( GameObject obj )
    {
        if (SceneAudioProbeBatch != null)
        {
            SteamAudioSource audioSource = obj.GetComponent<SteamAudioSource>();

            if (audioSource != null)
            {
                audioSource.pathingProbeBatch = SceneAudioProbeBatch;
            }
        }
    }
}

public class SteamAudioProbeLinker : MonoBehaviour
{

    void Awake()
    {
        var ProbeBatch = GetComponent<SteamAudioProbeBatch>();

        if (ProbeBatch != null && Globals.SceneAudioProbeBatch == null)
            Globals.SceneAudioProbeBatch = ProbeBatch;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteamAudioProbeBatchBinder : MonoBehaviour
{
    void Start()
    {
        Globals.TryResolvingAudioProbes(gameObject);
    }
}

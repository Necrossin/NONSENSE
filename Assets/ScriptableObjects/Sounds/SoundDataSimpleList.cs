using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SoundDataSimpleList", menuName = "ScriptableObjects/Sounds/SimpleList", order = 1)]
public class SoundDataSimpleList : ScriptableObject
{
    public List<AudioClip> lstSoundList;

    public AudioClip GetRandomClip()
    {
        int index = Random.Range(0, lstSoundList.Count - 1);

        if (lstSoundList[index] != null)
            return lstSoundList[index];

        return null;
    }
}

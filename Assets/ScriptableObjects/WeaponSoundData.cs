using UnityEngine;

[CreateAssetMenu(fileName = "SoundData", menuName = "ScriptableObjects/WeaponSoundData", order = 1)]
public class WeaponSoundData : ScriptableObject
{
    public AudioClip fireSnd;
    public AudioClip dryFireSnd;
    public AudioClip miscSnd1;
}
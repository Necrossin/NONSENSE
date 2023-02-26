using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Pool Object", menuName = "ScriptableObjects/Pool Objects", order = 1)]

public class PoolObjectTemplate : ScriptableObject
{
    public GameObject Prefab;
    public int Capacity;
}

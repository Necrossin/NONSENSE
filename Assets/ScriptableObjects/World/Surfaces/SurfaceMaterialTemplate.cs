using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Surface Material", menuName = "ScriptableObjects/Surface Materials", order = 1)]

public class SurfaceMaterialTemplate : ScriptableObject
{
    public GameObject decal;
    public GameObject impactEffect;
    public Material decalMaterial;
}

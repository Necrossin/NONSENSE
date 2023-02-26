using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DMG_TYPE
{
    NONE,
    GENERIC,
    BULLET,
    MELEE
}

public struct DamageInfo
{
    public void Init()
    {
        iAmount = 0;
        vDamagePos = Vector3.zero;
        vDamageDir = Vector3.zero;
        vDamageForce = Vector3.zero;
        eDamageType = DMG_TYPE.GENERIC;
    }
    
    public int iAmount;
    public Vector3 vDamagePos;
    public Vector3 vDamageDir;
    public Vector3 vDamageForce;
    public DMG_TYPE eDamageType;
    public GameObject pAttacker;
}

public interface IDamageable
{
    int Health();
    int MaxHealth();
    void SetHealth(int iAmount);
    void SetMaxHealth(int iAmount);

    void TakeDamage(int iAmount);
    void TakeDamage(DamageInfo damageInfo);
    void OnTakeDamage(int iAmount);
    void OnTakeDamage(DamageInfo damageInfo);

    void Break(DamageInfo damageInfo);
    void Break(int iAmount);

    bool IsDead();
}

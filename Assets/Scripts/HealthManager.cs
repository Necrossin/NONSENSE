using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class DamageInfoEvent : UnityEvent<DamageInfo> {}

public class HealthManager : MonoBehaviour, IDamageable
{
    [SerializeField]
    int health = 100;
    [SerializeField]
    int maxHealth = 100;
    [SerializeField]
    bool invincible = false;
    [SerializeField]
    bool penetrateWhenBroken = false; //if this breaks on bullet hit, allow bullets to keep travelling (mostly for glass and other low health objects)

    public DamageInfoEvent OnTakeDamageEvent = new DamageInfoEvent();
    public DamageInfoEvent OnBreakEvent = new DamageInfoEvent();

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public int Health() => health;
    public int MaxHealth() => maxHealth;

    public void SetHealth(int iAmount) => health = iAmount;
    public void SetMaxHealth(int iAmount) => maxHealth = iAmount;

    public void TakeDamage(int iAmount)
    {
        OnTakeDamage(iAmount);
        
        if (invincible)
            return;

        SetHealth(Mathf.Clamp(health - iAmount, 0, maxHealth));
    }

    public void TakeDamage(DamageInfo damageInfo)
    {
        OnTakeDamage(damageInfo);

        if (invincible)
            return;

        SetHealth(Mathf.Clamp(health - damageInfo.iAmount, 0, maxHealth));

        //Debug.Log(Health());
    }

    public void OnTakeDamage(int iAmount)
    {
        var tempDmg = new DamageInfo();
        tempDmg.Init();

        tempDmg.iAmount = iAmount;

        OnTakeDamage(tempDmg);        
    }

    public void OnTakeDamage(DamageInfo damageInfo)
    {
        OnTakeDamageEvent.Invoke(damageInfo);
    }

    public void Break(int iAmount)
    {
        var tempDmg = new DamageInfo();
        tempDmg.Init();

        tempDmg.iAmount = iAmount;

        Break(tempDmg);
    }

    public void Break(DamageInfo damageInfo)
    {
        OnBreakEvent.Invoke(damageInfo);
    }

    public bool IsDead() => health <= 0;
    public bool BulletsPenetrateWhenBroken() => penetrateWhenBroken;

}

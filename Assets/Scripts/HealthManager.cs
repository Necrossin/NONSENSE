using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class DamageInfoEvent : UnityEvent<DamageInfo> {}

public class HealthManager : MonoBehaviour, IDamageable
{
    [SerializeField]
    int m_iHealth = 100;
    [SerializeField]
    int m_iMaxHealth = 100;
    [SerializeField]
    bool m_bInvincible = false;

    public DamageInfoEvent OnTakeDamageEvent = new DamageInfoEvent();
    public DamageInfoEvent OnBreakEvent = new DamageInfoEvent();

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public int Health() => m_iHealth;
    public int MaxHealth() => m_iMaxHealth;

    public void SetHealth(int iAmount) => m_iHealth = iAmount;
    public void SetMaxHealth(int iAmount) => m_iMaxHealth = iAmount;

    public void TakeDamage(int iAmount)
    {
        OnTakeDamage(iAmount);
        
        if (m_bInvincible)
            return;

        SetHealth(Mathf.Clamp(m_iHealth - iAmount, 0, m_iMaxHealth));
    }

    public void TakeDamage(DamageInfo damageInfo)
    {
        OnTakeDamage(damageInfo);

        if (m_bInvincible)
            return;

        SetHealth(Mathf.Clamp(m_iHealth - damageInfo.iAmount, 0, m_iMaxHealth));
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

    public bool IsDead() => m_iHealth <= 0;

}

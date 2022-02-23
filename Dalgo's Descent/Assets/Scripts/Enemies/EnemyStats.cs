using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : MonoBehaviour
{
    protected Health m_Health;
    public int baseDamage;
    public float damageModifier; // Difficulty Modifier for enemy damage

    void Awake()
    {
        m_Health = GetComponent<Health>();
    }

    public void Init(float strength)
    {
        m_Health.maxHealth = m_Health.currentHealth = m_Health.maxHealth * strength;
        baseDamage = (int)(baseDamage * strength);
    }
        
    public int FinalDamage() { return (int)(baseDamage * damageModifier); }

    public Health health { get { return m_Health; } }
}

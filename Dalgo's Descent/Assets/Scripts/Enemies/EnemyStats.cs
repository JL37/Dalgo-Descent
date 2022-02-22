using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : MonoBehaviour
{
    protected Health m_Health;
    public int baseDamage;
    public float damageModifier; // Difficulty Modifier for enemy damage

    void Start()
    {
        m_Health = GetComponent<Health>();
    }
        
    public int FinalDamage() { return (int)(baseDamage * damageModifier); }

    public Health health { get { return m_Health; } }
}

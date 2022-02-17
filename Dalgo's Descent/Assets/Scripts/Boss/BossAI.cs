using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[DefaultExecutionOrder(1)]
public class BossAI : MonoBehaviour
{
    public Animator m_animator;
    public GameObject m_playerRef;
    public NavMeshAgent m_agent;
    public Vector3 m_targetPoint = new Vector3();

    public Collider m_damageCollider;
    public bool m_inAttackRange = false;

    public Health m_Health;
    private int m_attackChoice;

    void Awake()
    {
        m_Health = GetComponent<Health>();
    }

    public void AttackPlayer()
    {
        if (m_inAttackRange)
        {
            Debug.Log("Player Hit");
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            Damage(10);
        }
    }
    public void Damage(float amount)
    {
        m_Health.TakeDamage(amount);
    }

    public void Die()
    {
        m_Health.DieAnimation();
    }
}

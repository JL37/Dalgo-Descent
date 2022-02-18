using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Animations.Rigging;

[RequireComponent(typeof(NavMeshAgent))]
[DefaultExecutionOrder(1)]
public class BossAI : MonoBehaviour
{
    [Header("References")]
    public GameObject m_playerRef;
    public NavMeshAgent m_agent;
    public Collider m_damageCollider;
    public Health m_Health;
    public Vector3 m_targetPoint = new Vector3();

    [Header("Animation")]
    public Animator m_animator;
    public Rig m_rig;

    [HideInInspector] public bool m_inAttackRange = false;
    [HideInInspector] public float m_bossTimer;
    public float m_bossAttackIntervals;
    private bool m_rigActive = true;

    void Awake()
    {
        m_playerRef = GameObject.FindGameObjectWithTag("Player");
        m_Health = GetComponent<Health>();
    }

    public void AttackPlayer()
    {
        if (m_inAttackRange)
        {
            Debug.Log("Player Hit");
        }
    }

    public void MoveTo(Vector3 position)
    {
        m_targetPoint = position;
        m_agent.SetDestination(m_targetPoint);
    }

    void Update()
    {
        m_rig.weight = m_rigActive ? m_rig.weight + Time.deltaTime : m_rig.weight - Time.deltaTime;
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

    public void SetRigActive(bool active)
    {
        m_rigActive = active;
    }
}

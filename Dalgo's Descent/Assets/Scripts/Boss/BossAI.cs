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
    public Animator m_animator;
    public GameObject m_playerRef;
    public NavMeshAgent m_agent;
    public Collider m_damageCollider;
    public Health m_Health;
    public Vector3 m_targetPoint = new Vector3();
    public RigBuilder m_rig;

    [HideInInspector] public bool m_inAttackRange = false;
    [HideInInspector] public float m_bossTimer;
    public float m_bossAttackIntervals;

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

    public void SetRigActive(bool active)
    {
        m_rig.enabled = active;
    }
}

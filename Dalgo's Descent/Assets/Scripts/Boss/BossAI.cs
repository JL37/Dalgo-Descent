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

    private GameManager m_GameManager;
    private bool m_rigActive = true;
    private bool m_AggroActivated = false;

    void Awake()
    {
<<<<<<< HEAD
        playerRef = GameObject.FindGameObjectWithTag("Player");
        health = GetComponent<Health>();
        m_GameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
=======
        m_playerRef = GameObject.FindGameObjectWithTag("Player");
        m_Health = GetComponent<Health>();
>>>>>>> parent of 19d8647 (Merge branch 'main' into BasicUI-and-SceneSetup)
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

        //Check for any signs of aggression, if have then bruh update the gamemanager
        if (!m_AggroActivated && IsAggro())
        {
            //Add to the gamemanager to say got enemy here
            m_AggroActivated = true;
            m_GameManager.AddToEnemyArray(gameObject);
        }
    }

    public bool IsAggro()
    {
        return !animator.GetCurrentAnimatorStateInfo(0).IsName("Idle") && !animator.GetCurrentAnimatorStateInfo(0).IsName("Spawn");
    }

    public void Damage(float amount)
    {
<<<<<<< HEAD
		if (health.currentHealth <= 0)
			return;
		
        if (!m_AggroActivated)
        {
            //Add to the gamemanager to say got enemy here
            m_AggroActivated = true;
            m_GameManager.AddToEnemyArray(gameObject);
        }

        health.TakeDamage(amount);

        if (health.currentHealth <= 0)
            m_GameManager.RemoveFromEnemyArray(gameObject);
=======
        m_Health.TakeDamage(amount);
>>>>>>> parent of 19d8647 (Merge branch 'main' into BasicUI-and-SceneSetup)
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

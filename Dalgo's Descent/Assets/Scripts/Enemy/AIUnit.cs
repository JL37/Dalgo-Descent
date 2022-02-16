using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[DefaultExecutionOrder(1)]
public class AIUnit : MonoBehaviour
{
    public Animator m_animator;
    public GameObject m_playerRef;
    public Rigidbody m_rigidbody;
    public NavMeshAgent m_agent;
    public Vector3 m_targetPoint = new Vector3();
    public LayerMask m_groundLayer;

    public Collider m_damageCollider;
    public bool m_inAttackRange = false;

    private Health m_Health;

    private void Awake()
    {
        m_animator = GetComponentInChildren<Animator>();
        m_Health = GetComponent<Health>();
        m_rigidbody = GetComponent<Rigidbody>();
        m_agent = GetComponent<NavMeshAgent>();
        m_playerRef = GameObject.FindGameObjectWithTag("Player");
        AIManager.Instance.Units.Add(this);
    }

    public void Update()
    {
        if (Input.GetKey(KeyCode.P))
        {
            Debug.Log("Knockup");
            m_animator.SetTrigger("Knockup");
        }

        if (Input.GetKeyDown(KeyCode.O))
        {
            EnemyHit();
        }
    }

    public void MoveTo(Vector3 Position)
    {
        m_agent.SetDestination(Position);
    }

    public void AttackPlayer()
    {
        if (m_inAttackRange)
        {
            Debug.Log("Player Hit");
        }
    }

    public void EnemyHit(/*Skill enum or smth idk*/) 
    {
        m_animator.SetTrigger("Hit");
        m_animator.SetBool("IsHit", true);
        m_Health.TakeDamage(10);
    }
}
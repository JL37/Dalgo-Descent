using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[DefaultExecutionOrder(1)]
public class AIUnit : MonoBehaviour
{
    public GameObject m_playerRef;
    public Rigidbody m_rigidbody;
    public NavMeshAgent m_agent;
    public Vector3 m_targetPoint = new Vector3();
    public LayerMask m_groundLayer;

    public Collider m_damageCollider;
    public bool m_inAttackRange = false;

    private void Awake()
    {
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
            GetComponentInChildren<Animator>().SetTrigger("Knockup");
        }

        if (Input.GetKey(KeyCode.O))
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
        GetComponentInChildren<Animator>().SetBool("IsHit", true); 
    }
}
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[DefaultExecutionOrder(1)]
public class AIUnit : MonoBehaviour
{
    public NavMeshAgent m_agent;
    public Vector3 m_targetPoint = new Vector3();
    public LayerMask m_groundLayer;

    public Collider m_damageCollider;
    public bool m_inAttackRange = false;

    private void Awake()
    {
        m_agent = GetComponent<NavMeshAgent>();
        AIManager.Instance.Units.Add(this);
    }

    public void Update()
    {
        if (Input.GetKey(KeyCode.P))
        {
            Debug.Log("Knockup");
            GetComponentInChildren<Animator>().SetTrigger("Knockup");
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
}
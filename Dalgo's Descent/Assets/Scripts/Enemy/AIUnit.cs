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
        AISeparation();

        if (Input.GetKey(KeyCode.P))
        {
            Debug.Log("Knockup");
            GetComponentInChildren<Animator>().SetTrigger("Knockup");
        }

        if (Input.GetKeyDown(KeyCode.O))
        {
            Debug.Log("Hit");
            if (m_agent.enabled)
                m_agent.ResetPath();


            GetComponentInChildren<Animator>().speed = 0.8f;
            GetComponentInChildren<Animator>().SetTrigger("Hit");
            GetComponentInChildren<Animator>().SetFloat("Speed", 0f);

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

    private void AISeparation()
    {
    }
}
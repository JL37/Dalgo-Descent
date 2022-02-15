using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[DefaultExecutionOrder(1)]
public class AIUnit : MonoBehaviour
{
    public NavMeshAgent m_agent;
    public Vector3 m_targetPoint = new Vector3();

    private void Awake()
    {
        m_agent = GetComponent<NavMeshAgent>();
        AIManager.Instance.Units.Add(this);
    }

    public void MoveTo(Vector3 Position)
    {
        m_agent.SetDestination(Position);
    }
}
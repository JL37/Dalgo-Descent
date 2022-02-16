using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyPatrolState : EnemyBaseState
{
    AIUnit aiUnit;
    bool walkpointSet;
    NavMeshAgent agent;
    float destinationChangeTime;
    float maxDestinationChangeTime;
    GameObject Player;

    FieldOfView FOV;
    public override void OnSLStatePostEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        walkpointSet = false;

        aiUnit = animator.transform.parent.GetComponent<AIUnit>();
        agent = animator.transform.parent.GetComponent<NavMeshAgent>();
        agent.speed = 0.5f;
        destinationChangeTime = maxDestinationChangeTime = 0.2f;
        Player = GameObject.FindGameObjectWithTag("Player");
        FOV = agent.transform.GetComponent<FieldOfView>();

        agent.SetDestination(aiUnit.m_targetPoint);
    }

    public override void OnSLStateNoTransitionUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

        Vector3 target = aiUnit.m_targetPoint - new Vector3(animator.transform.parent.position.x, 0, animator.transform.parent.position.z);
        var q = Quaternion.LookRotation(target);
        float velocity = agent.velocity.magnitude; /// agent.speed;
        animator.SetFloat("Speed", velocity);
        animator.speed = 0.42814f;
        animator.transform.parent.rotation = Quaternion.RotateTowards(animator.transform.parent.rotation, q, 90 * Time.deltaTime);

        // Quaternion lookat = Quaternion.RotateTowards(animator.transform.rotation, walkpoint, Time.deltaTime * 10f);
        // animator.transform.LookAt(new Vector3(, animator.transform.position.y, walkpoint.z));
        Vector3 distanceToWalkpoint = animator.transform.position - aiUnit.m_targetPoint;

        // walkpoint reached
        if (distanceToWalkpoint.magnitude < 1f)
            animator.SetBool("IsPatrolling", false);

        if (FOV.m_canSeeTarget)
        {
            animator.SetBool("IsAttack", true);
        }
    }

    public override void OnSLStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.speed = 1;
    }

    private void SearchWalkPoint()
    {
        aiUnit.m_targetPoint = new Vector3(aiUnit.transform.position.x + Random.Range(5, -5), 0, aiUnit.transform.position.z + Random.Range(5, -5));
        Debug.Log("Walkpoint Set");
        walkpointSet = true;
    }
}

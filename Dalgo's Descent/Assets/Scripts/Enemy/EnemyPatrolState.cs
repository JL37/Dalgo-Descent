using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyPatrolState : EnemyBaseState
{
    Vector3 walkpoint;
    bool walkpointSet;
    NavMeshAgent agent;
    public override void OnSLStatePostEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        walkpointSet = false;
        agent = animator.transform.GetComponent<NavMeshAgent>();
    }

    public override void OnSLStateNoTransitionUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!walkpointSet) SearchWalkPoint(animator.transform);
        if (walkpointSet) agent.SetDestination(walkpoint);

        var q = Quaternion.LookRotation(walkpoint - animator.transform.position);
        float velocity = agent.velocity.magnitude / agent.speed;
        animator.speed = velocity;
        animator.transform.rotation = Quaternion.RotateTowards(animator.transform.rotation, q, 30f * Time.deltaTime);

        // Quaternion lookat = Quaternion.RotateTowards(animator.transform.rotation, walkpoint, Time.deltaTime * 10f);
        // animator.transform.LookAt(new Vector3(, animator.transform.position.y, walkpoint.z));
        Vector3 distanceToWalkpoint = animator.transform.position - walkpoint;

        // walkpoint reached
        if (distanceToWalkpoint.magnitude < 1f)
            animator.SetBool("IsPatrolling", false);
    }

    public override void OnSLStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.speed = 1;
    }

    private void SearchWalkPoint(Transform transform)
    {
        walkpoint = new Vector3(Random.Range(25, -25), 0, Random.Range(25, -25));
        Debug.Log("Walkpoint Set");
        walkpointSet = true;

        if (Physics.Raycast(walkpoint, -transform.up, 2f, 40))
        {
            
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyPatrolState : EnemyBaseState
{
    Vector3 walkpoint;
    bool walkpointSet;
    NavMeshAgent agent;
    float destinationChangeTime;
    float maxDestinationChangeTime;
    GameObject Player;

    FieldOfView FOV;
    public override void OnSLStatePostEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        walkpointSet = false;
        agent = animator.transform.parent.GetComponent<NavMeshAgent>();
        agent.speed = 0.8f;
        destinationChangeTime = maxDestinationChangeTime = 0.2f;
        Player = GameObject.FindGameObjectWithTag("Player");
        FOV = agent.transform.GetComponent<FieldOfView>();
    }

    public override void OnSLStateNoTransitionUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!walkpointSet) SearchWalkPoint(animator.transform);
        if (walkpointSet)
        {
            destinationChangeTime -= Time.deltaTime;

            if (destinationChangeTime < 0f)
            {
                destinationChangeTime = maxDestinationChangeTime;
                agent.SetDestination(walkpoint);
            }
        }

        Vector3 target = walkpoint - animator.transform.parent.position;
        var q = Quaternion.LookRotation(target);
        float velocity = agent.velocity.magnitude; /// agent.speed;
        animator.SetFloat("Speed", velocity);
        animator.speed = 0.42814f;
        animator.transform.parent.rotation = Quaternion.RotateTowards(animator.transform.parent.rotation, q, 30f * Time.deltaTime);

        // Quaternion lookat = Quaternion.RotateTowards(animator.transform.rotation, walkpoint, Time.deltaTime * 10f);
        // animator.transform.LookAt(new Vector3(, animator.transform.position.y, walkpoint.z));
        Vector3 distanceToWalkpoint = animator.transform.position - walkpoint;

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

    private void SearchWalkPoint(Transform transform)
    {
        walkpoint = new Vector3(Random.Range(25, -25), 0, Random.Range(25, -25));
        Debug.Log("Walkpoint Set");
        walkpointSet = true;
    }
}

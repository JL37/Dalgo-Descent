using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyPreparingAttackState : EnemyBaseState
{
    NavMeshAgent agent;
    float destinationChangeTime;
    float maxDestinationChangeTime;
    GameObject Player;
    public override void OnSLStatePostEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        agent = animator.transform.parent.GetComponent<NavMeshAgent>();
        agent.speed = 3.5f;
        destinationChangeTime = maxDestinationChangeTime = 0.2f;
    }

    public override void OnSLStateNoTransitionUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        destinationChangeTime -= Time.deltaTime;

        if (destinationChangeTime < 0f)
        {
            destinationChangeTime = maxDestinationChangeTime;
            agent.SetDestination(Player.transform.position);
        }

        var q = Quaternion.LookRotation(Player.transform.position - animator.transform.parent.position);
        float velocity = agent.velocity.magnitude; /// agent.speed;
        animator.SetFloat("Speed", velocity);
        animator.speed = 1f;
        animator.transform.parent.rotation = Quaternion.RotateTowards(animator.transform.parent.rotation, q, 0.1f);

        // Quaternion lookat = Quaternion.RotateTowards(animator.transform.rotation, walkpoint, Time.deltaTime * 10f);
        // animator.transform.LookAt(new Vector3(, animator.transform.position.y, walkpoint.z));
        Vector3 distanceToWalkpoint = Player.transform.position - animator.transform.position;
        Debug.Log(distanceToWalkpoint.magnitude);

        // walkpoint reached
        if (distanceToWalkpoint.magnitude < 3f)
            animator.SetTrigger("Attack");
    }

    public override void OnSLStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.speed = 1;
    }
}

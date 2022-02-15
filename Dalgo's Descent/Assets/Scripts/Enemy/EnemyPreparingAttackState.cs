using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Animations.Rigging;

public class EnemyPreparingAttackState : EnemyBaseState
{
    NavMeshAgent agent;
    AIUnit aiUnit;
    float destinationChangeTime;
    float maxDestinationChangeTime;
    GameObject Player;
    FieldOfView FOV;
    public override void OnSLStatePostEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        agent = animator.transform.parent.GetComponent<NavMeshAgent>();
        agent.speed = 3.5f;
        destinationChangeTime = maxDestinationChangeTime = 0.2f;
        FOV = agent.GetComponent<FieldOfView>();
        FOV.m_multiAimConstraint.data.sourceObjects.Add(new WeightedTransform(Player.transform, 1));
        FOV.m_rig.Build();
        aiUnit = agent.GetComponent<AIUnit>();
    }

    public override void OnSLStateNoTransitionUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        destinationChangeTime -= Time.deltaTime;

        if (destinationChangeTime < 0f)
        {
            destinationChangeTime = maxDestinationChangeTime;
            agent.SetDestination(Player.transform.position);
            aiUnit.MoveTo(Player.transform.position);
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
        {
            Debug.Log("Stopped");
            agent.ResetPath();
            animator.SetTrigger("Attack");
        }

        
    }

    public override void OnSLStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.speed = 1;
        FOV.m_multiAimConstraint.data.sourceObjects.RemoveAt(0);
        FOV.m_rig.Build();
    }
}

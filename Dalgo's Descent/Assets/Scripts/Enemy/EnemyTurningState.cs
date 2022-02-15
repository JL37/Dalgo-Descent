using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyTurningState : EnemyBaseState
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

        SearchWalkPoint();
    }

    public override void OnSLStateNoTransitionUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // if (!walkpointSet) SearchWalkPoint(animator.transform);
        //if (walkpointSet)
        //{
        //    destinationChangeTime -= Time.deltaTime;

        //    if (destinationChangeTime < 0f)
        //    {
        //        destinationChangeTime = maxDestinationChangeTime;
        //        agent.SetDestination(aiUnit.m_targetPoint);
        //    }
        //}

        if (!walkpointSet)
            return;

        Vector3 target = aiUnit.m_targetPoint - animator.transform.parent.position;
        var q = Quaternion.LookRotation(target);
        float velocity = agent.velocity.magnitude; /// agent.speed;
        animator.SetFloat("Speed", velocity);
        animator.speed = 0.42814f;
        animator.transform.parent.rotation = Quaternion.RotateTowards(animator.transform.parent.rotation, q, 50f * Time.deltaTime);
        
        if (Quaternion.Angle(animator.transform.parent.rotation, q) < 10f)
        {
            animator.SetBool("PatrolDoneTurning", true);
        }

        if (FOV.m_canSeeTarget)
        {
            animator.SetBool("IsAttack", true);
        }
    }

    public override void OnSLStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.speed = 1;
        animator.SetBool("PatrolDoneTurning", false);
    }

    private void SearchWalkPoint()
    {
        aiUnit.m_targetPoint = new Vector3(aiUnit.transform.position.x + Random.Range(5, -5), 0, aiUnit.transform.position.z + Random.Range(5, -5));
        Debug.Log("Walkpoint Set");
        walkpointSet = true;
    }
}

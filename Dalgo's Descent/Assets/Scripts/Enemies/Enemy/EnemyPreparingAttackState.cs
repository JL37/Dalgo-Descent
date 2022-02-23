using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Animations.Rigging;

public class EnemyPreparingAttackState : EnemyBaseState
{
    AIUnit aiUnit;
    GameObject Player;
    FieldOfView FOV;
    float destinationChangeTime;
    float maxDestinationChangeTime;
    public override void OnSLStatePostEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        aiUnit = animator.transform.parent.GetComponent<AIUnit>();
        aiUnit.agent.speed = Random.Range(3f, 3.5f);
        destinationChangeTime = maxDestinationChangeTime = 0.2f;
        FOV = aiUnit.GetComponent<FieldOfView>();
        Player = aiUnit.playerRef;
    }

    public override void OnSLStateNoTransitionUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (Player == null)
            return;

        destinationChangeTime -= Time.deltaTime;

        if (destinationChangeTime < 0f)
        {
            destinationChangeTime = maxDestinationChangeTime;
            aiUnit.agent.SetDestination(Player.transform.position);
            aiUnit.MoveTo(Player.transform.position);
        }

        var q = Quaternion.LookRotation(new Vector3(Player.transform.position.x, 0, Player.transform.position.z) - new Vector3(animator.transform.parent.position.x, 0, animator.transform.parent.position.z));
        float velocity = aiUnit.agent.velocity.magnitude; /// agent.speed;
        animator.SetFloat("Speed", velocity);
        animator.speed = aiUnit.agent.speed / 3.5f;
        animator.transform.parent.rotation = Quaternion.RotateTowards(animator.transform.parent.rotation, q, 0.1f);

        // Quaternion lookat = Quaternion.RotateTowards(animator.transform.rotation, walkpoint, Time.deltaTime * 10f);
        // animator.transform.LookAt(new Vector3(, animator.transform.position.y, walkpoint.z));
        // Vector3 distanceToWalkpoint = Player.transform.position - animator.transform.position;
        // Debug.Log(distanceToWalkpoint.magnitude);

        // walkpoint reached
        if (aiUnit.inAttackRange)
        {
            if (aiUnit.agent.enabled)
                aiUnit.agent.ResetPath();

            animator.SetBool("InAttackRange", true);
        }

        
    }

    public override void OnSLStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.speed = 1f;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossChargingState : BossBaseState
{
    BossAI bossAI;
    public override void OnSLStatePostEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        bossAI = animator.transform.parent.GetComponent<BossAI>();
        // Vector3 targetPoint = 
        bossAI.MoveTo(bossAI.playerRef.transform.position);
        bossAI.agent.speed = 9f;
    }

    public override void OnSLStateNoTransitionUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetFloat("Speed", bossAI.agent.speed);
        // animator.transform.parent.LookAt(new Vector3(bossAI.m_playerRef.transform.position.x, bossAI.transform.position.y, bossAI.m_playerRef.transform.position.z));
        Vector3 distanceToWalkpoint = animator.transform.position - bossAI.targetPosition;
        if (distanceToWalkpoint.sqrMagnitude < 2f)
        {
            animator.SetBool("ReachedDestination", true);
        }
    }

    public override void OnSLStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool("ReachedDestination", false);
    }
}

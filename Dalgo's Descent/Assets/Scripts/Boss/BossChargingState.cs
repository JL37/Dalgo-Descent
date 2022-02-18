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
        bossAI.MoveTo(bossAI.m_playerRef.transform.position);
    }

    public override void OnSLStateNoTransitionUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetFloat("Speed", bossAI.m_agent.speed);
        // animator.transform.parent.LookAt(new Vector3(bossAI.m_playerRef.transform.position.x, bossAI.transform.position.y, bossAI.m_playerRef.transform.position.z));
        Vector3 distanceToWalkpoint = animator.transform.position - bossAI.m_targetPoint;
        if (distanceToWalkpoint.sqrMagnitude < 4f)
        {
            animator.SetBool("IsCharging", false);
        }
    }

    public override void OnSLStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
    }
}

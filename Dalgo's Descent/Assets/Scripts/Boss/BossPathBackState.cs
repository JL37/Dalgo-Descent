using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossPathBackState : BossBaseState
{
    BossAI bossAI;
    public override void OnSLStatePostEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        bossAI = animator.transform.parent.GetComponent<BossAI>();
        bossAI.SetRigActive(false);
        bossAI.MoveTo(bossAI.centerOfRoom.position);
        bossAI.agent.speed = 5f;
    }

    public override void OnSLStateNoTransitionUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetFloat("Speed", bossAI.agent.speed);
        animator.transform.parent.LookAt(new Vector3(bossAI.targetPoint.x, bossAI.transform.position.y, bossAI.targetPoint.z));
        Vector3 distanceToWalkpoint = animator.transform.position - bossAI.targetPoint;
        if (distanceToWalkpoint.sqrMagnitude < 4f)
        {
            animator.SetBool("ReachedDestination", true);
        }
    }

    public override void OnSLStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
    }
}

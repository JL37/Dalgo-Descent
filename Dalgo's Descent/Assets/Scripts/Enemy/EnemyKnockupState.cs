using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyKnockupState : EnemyBaseState
{
    AIUnit aiUnit;
    public override void OnSLStatePostEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        aiUnit = animator.transform.parent.GetComponent<AIUnit>();
        animator.ResetTrigger("Knockup");
        animator.SetBool("IsAirborne", true);

        aiUnit.m_rigidbody.isKinematic = false;
        aiUnit.m_agent.enabled = false;
    }

    public override void OnSLStateNoTransitionUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (aiUnit.m_rigidbody.velocity.y < 0)
            animator.SetBool("IsFalling", true);
    }

    public override void OnSLStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }
}

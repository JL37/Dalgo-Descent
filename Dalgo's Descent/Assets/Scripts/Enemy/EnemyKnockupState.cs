using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyKnockupState : EnemyBaseState
{
    AIUnit aiUnit;
    public override void OnSLStatePostEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        aiUnit = animator.transform.parent.GetComponent<AIUnit>();
        aiUnit.m_rigidbody.isKinematic = false;
        aiUnit.m_agent.enabled = false;
        aiUnit.m_rigidbody.velocity = Vector3.zero;
        aiUnit.m_rigidbody.AddForce(new Vector3(0, 400, 0));
        animator.SetBool("IsAirborne", true);
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHitState : EnemyBaseState
{
    AIUnit aiUnit;
    public override void OnSLStatePostEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.ResetTrigger("Hit");
        aiUnit = animator.transform.parent.GetComponent<AIUnit>();
        animator.SetFloat("Speed", 0f);

        Debug.Log("Hit");
        if (aiUnit.m_agent.enabled)
            aiUnit.m_agent.ResetPath();

        aiUnit.m_rigidbody.isKinematic = false;
        aiUnit.m_agent.enabled = false;
        aiUnit.m_rigidbody.velocity = Vector3.zero;

    }

    public override void OnSLStateNoTransitionUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }

    public override void OnSLStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool("IsHit", false);
        aiUnit.m_agent.enabled = true;
        aiUnit.m_rigidbody.isKinematic = true;
        animator.speed = 1f;
    }
}

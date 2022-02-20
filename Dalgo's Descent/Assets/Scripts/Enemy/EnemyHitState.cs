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
        if (aiUnit.agent.enabled)
            aiUnit.agent.ResetPath();

        aiUnit.GetComponent<Rigidbody>().isKinematic = false;
        aiUnit.agent.enabled = false;
        // aiUnit.m_rigidbody.velocity = Vector3.zero;

    }

    public override void OnSLStateNoTransitionUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }

    public override void OnSLStatePreExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool("IsHit", false);
        aiUnit.agent.enabled = true;
        aiUnit.GetComponent<Rigidbody>().isKinematic = true;
        animator.speed = 1f;
    }
}

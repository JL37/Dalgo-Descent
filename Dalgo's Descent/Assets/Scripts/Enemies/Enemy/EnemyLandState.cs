using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLandState : EnemyBaseState
{
    Rigidbody rb;
    AIUnit aiUnit;
    public override void OnSLStatePostEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        aiUnit = animator.transform.parent.GetComponent<AIUnit>();
        rb = animator.transform.parent.GetComponent<Rigidbody>();
        animator.SetBool("IsAirborne", false);
        aiUnit.agent.enabled = true;
        rb.isKinematic = true;
    }

    public override void OnSLStateNoTransitionUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
    }

    public override void OnSLStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFallingState : EnemyBaseState
{
    Rigidbody rb;
    AIUnit aiUnit;
    public override void OnSLStatePostEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        aiUnit = animator.transform.parent.GetComponent<AIUnit>();
        rb = animator.transform.parent.GetComponent<Rigidbody>();
    }

    public override void OnSLStateNoTransitionUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (Physics.CheckSphere(aiUnit.transform.position, 0.4f, aiUnit.m_groundLayer))
        {
            animator.SetBool("IsFalling", false);
        }
    }

    public override void OnSLStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }

}

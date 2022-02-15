using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyKnockupState : EnemyBaseState
{
    Rigidbody rb;
    AIUnit aiUnit;
    public override void OnSLStatePostEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        aiUnit = animator.transform.parent.GetComponent<AIUnit>();
        rb = animator.transform.parent.GetComponent<Rigidbody>();
        rb.isKinematic = false;
        aiUnit.m_agent.enabled = false;
        // aiUnit.transform.GetComponent<Collider>().enabled = true;
        rb.velocity = Vector3.zero;
        rb.AddForce(new Vector3(0, 400, 0));
        animator.SetBool("IsAirborne", true);
    }

    public override void OnSLStateNoTransitionUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (rb.velocity.y < 0)
            animator.SetBool("IsFalling", true);
    }

    public override void OnSLStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }
}

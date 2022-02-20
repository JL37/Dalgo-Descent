using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFallingState : EnemyBaseState
{
    AIUnit aiUnit;
    public override void OnSLStatePostEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        aiUnit = animator.transform.parent.GetComponent<AIUnit>();
    }

    public override void OnSLStateNoTransitionUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Debug.Log(Physics.CheckSphere(aiUnit.transform.position, 0.4f, aiUnit.groundLayer));
        if (Physics.CheckSphere(aiUnit.transform.position, 0.4f, aiUnit.groundLayer))
        {
            animator.SetBool("IsFalling", false);
        }
    }

    public override void OnSLStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }

}

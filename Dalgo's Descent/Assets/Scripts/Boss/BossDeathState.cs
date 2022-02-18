using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossDeathState : BossBaseState
{
    BossAI bossAI;
    public override void OnSLStatePostEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        bossAI = animator.transform.parent.GetComponent<BossAI>();
        bossAI.SetRigActive(false);
        bossAI.agent.ResetPath();
    }

    public override void OnSLStateNoTransitionUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }

    public override void OnSLStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }
}

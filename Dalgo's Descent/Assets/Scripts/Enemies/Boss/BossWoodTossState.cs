using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossWoodTossState : BossBaseState
{
    BossAI bossAI;
    public override void OnSLStatePostEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        bossAI = animator.transform.parent.GetComponent<BossAI>();
    }

    public override void OnSLStateNoTransitionUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.transform.parent.LookAt(new Vector3(bossAI.playerRef.transform.position.x, bossAI.transform.position.y, bossAI.playerRef.transform.position.z));
    }

    public override void OnSLStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
    }
}

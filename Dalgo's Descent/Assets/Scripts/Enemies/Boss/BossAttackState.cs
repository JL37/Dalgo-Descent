using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAttackState : SceneLinkedSMB<MyMonoBehaviour>
{
    BossAI bossAI;
    public override void OnSLStatePostEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        bossAI = animator.transform.parent.GetComponent<BossAI>();
    }

    public override void OnSLStateNoTransitionUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
    }

    public override void OnSLStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // bossAI.SetRigActive(true);
    }
}

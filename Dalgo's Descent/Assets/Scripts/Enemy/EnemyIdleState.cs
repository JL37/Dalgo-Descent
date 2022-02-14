using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyIdleState : EnemyBaseState
{
    float time;
    public override void OnSLStatePostEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.speed = 0.4f;
        time = 0f;
    }

    public override void OnSLStateNoTransitionUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        time += Time.deltaTime;
        // Debug.Log(time);
        if (animator.GetFloat("Speed") > 0) animator.SetFloat("Speed", animator.GetFloat("Speed") - Time.deltaTime * 5f);
        
        if (time > 3f)
        {
            animator.SetBool("IsPatrolling", true);
        }

        if (Input.GetKey(KeyCode.P))
        {
            animator.SetBool("IsAttack", true);
        }
    }

    public override void OnSLStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }
}

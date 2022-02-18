using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyIdleState : EnemyBaseState
{
    float time;
    float delay;
    FieldOfView FOV;
    public override void OnSLStatePostEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        delay = Random.Range(2f, 5f);
        time = 0f;
        FOV = animator.transform.parent.GetComponent<FieldOfView>();
    }

    public override void OnSLStateNoTransitionUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        time += Time.deltaTime;
        // Debug.Log(time);
        if (animator.GetFloat("Speed") > 0) animator.SetFloat("Speed", animator.GetFloat("Speed") - Time.deltaTime * 5f);
        
        if (time > delay)
        {
            animator.SetBool("IsPatrolling", true);
        }

        if (FOV.canSeeTarget)
        {
            animator.SetBool("IsAttack", true);
        }
    }

    public override void OnSLStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBaseState : SceneLinkedSMB<MyMonoBehaviour>
{
    public override void OnSLStatePostEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }

    public override void OnSLStateNoTransitionUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }

    public override void OnSLStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }

    public bool CanSeeTarget(Transform enemy, Transform target, float viewAngle, float viewRange)
    {
        Vector3 toTarget = target.position - enemy.transform.position;
        if (Vector3.Angle(enemy.transform.forward, toTarget) <= viewAngle)
        {
            if (Physics.Raycast(enemy.transform.position, toTarget, out RaycastHit hit, viewRange))
            {
                if (hit.transform.root == target)
                    return true;
            }
        }
        return false;
    }
}

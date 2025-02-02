using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossIdleState : SceneLinkedSMB<MyMonoBehaviour>
{
    BossAI bossAI;
    public override void OnSLStatePostEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        bossAI = animator.transform.parent.GetComponent<BossAI>();
        bossAI.m_bossTimer = bossAI.bossAttackIntervals;
        bossAI.SetRigActive(true);
        bossAI.ChooseAttack();
    }

    public override void OnSLStateNoTransitionUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (bossAI.playerRef == null)
            return;

        Vector3 directionToPlayer = (bossAI.playerRef.transform.position - bossAI.transform.position).normalized;
        float angle = Vector3.Angle(directionToPlayer, bossAI.transform.forward);
        if (angle > 30)
        {
            Vector3 target = bossAI.playerRef.transform.position - new Vector3(animator.transform.parent.position.x, 0, animator.transform.parent.position.z);
            var q = Quaternion.LookRotation(target);
            animator.transform.parent.rotation = Quaternion.RotateTowards(animator.transform.parent.rotation, q, 60 * Time.deltaTime);
        }

        bossAI.m_bossTimer -= Time.deltaTime;
        if (bossAI.m_bossTimer <= 0)
        {
            // Chance of ground slam is higher than other 2 attacks
            switch (bossAI.attackChoice)
            {
                case 1:
                    animator.SetTrigger("ChargeAttack");
                    break;
                case 2:
                    animator.SetTrigger("WoodToss");
                    break;
                default:
                    animator.SetTrigger("GroundSlam");
                    break;
            }
        }
    }

    public override void OnSLStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossIdleState : SceneLinkedSMB<MyMonoBehaviour>
{
    BossAI bossAI;
    public override void OnSLStatePostEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        bossAI = animator.transform.parent.GetComponent<BossAI>();
        bossAI.m_bossTimer = bossAI.m_bossAttackIntervals;
    }

    public override void OnSLStateNoTransitionUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Vector3 directionToPlayer = (bossAI.m_playerRef.transform.position - bossAI.transform.position).normalized;
        float angle = Vector3.Angle(directionToPlayer, bossAI.transform.forward);
        if (angle > 30)
        {
            Vector3 target = bossAI.m_playerRef.transform.position - new Vector3(animator.transform.parent.position.x, 0, animator.transform.parent.position.z);
            var q = Quaternion.LookRotation(target);
            animator.transform.parent.rotation = Quaternion.RotateTowards(animator.transform.parent.rotation, q, 60 * Time.deltaTime);
        }

        bossAI.m_bossTimer -= Time.deltaTime;
        if (bossAI.m_bossTimer <= 0)
        {   
            // Chance of ground slam is higher than other 2 attacks
            int attackChoice = (bossAI.m_Health.currentHealth < bossAI.m_Health.maxHealth * 0.5) ? Random.Range(1, 5) : Random.Range(1, 2);
            attackChoice = 1;
            switch (attackChoice)
            {
                case 1:
                    bossAI.SetRigActive(false);
                    animator.transform.parent.LookAt(new Vector3(bossAI.m_playerRef.transform.position.x, bossAI.transform.position.y, bossAI.m_playerRef.transform.position.z));
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

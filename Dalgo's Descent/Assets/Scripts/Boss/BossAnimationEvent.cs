using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAnimationEvent : MonoBehaviour
{
    BossAI bossAI;
    void Start()
    {
        bossAI = transform.parent.GetComponent<BossAI>();
    }

    public void SetChargeAttackDestination()
    {
        bossAI.m_agent.SetDestination(bossAI.m_playerRef.transform.position);
    }

    public void AttackPlayer()
    {
        bossAI.AttackPlayer();
    }

    public void Die()
    {
        bossAI.Die();
    }
}

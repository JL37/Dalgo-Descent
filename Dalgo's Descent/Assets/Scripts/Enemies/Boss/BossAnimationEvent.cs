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
        bossAI.agent.SetDestination(bossAI.playerRef.transform.position);
    }

    public void AttackPlayer()
    {
        bossAI.AttackPlayer();
    }

    public void Die()
    {
        bossAI.Die();
    }

    public void GrabWood()
    {
        bossAI.GrabWood();
    }

    public void TossWood()
    {
        bossAI.TossWood();
    }

    public void GroundSlam()
    {
        bossAI.GroundSlam();
    }

    public void ScreamSound()
    {
        bossAI.Scream();
    }

    public void LandSound()
    {
        bossAI.Land();
    }
}

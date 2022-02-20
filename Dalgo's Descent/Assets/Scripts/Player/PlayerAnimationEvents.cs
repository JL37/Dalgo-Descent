using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerController))]
public class PlayerAnimationEvents : MonoBehaviour
{
    private PlayerController m_playerController;
    public WeaponCollider weaponCollider;

    private void Start()
    {
        m_playerController = GetComponent<PlayerController>();
    }

    public void PlayerAttack()
    {
        foreach (Collider c in weaponCollider.collisionEvents)
        {
            if (c.gameObject.tag == "AI")
            {
                AI ai = c.gameObject.GetComponent<AI>();
                if (ai.aiType == AI.AI_TYPE.AI_TYPE_ENEMY)
                {
                    ((AIUnit)ai).EnemyHit(10);
                }
                else if (ai.aiType == AI.AI_TYPE.AI_TYPE_BOSS)
                {
                    ((BossAI)ai).Damage(10);
                }

                if (ai.health.currentHealth <= 0)
                    weaponCollider.collisionEvents.Remove(c);
            }
        }
    }
}

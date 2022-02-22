using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerController))]
public class PlayerAnimationEvents : MonoBehaviour
{
    private PlayerController m_playerController;
    public Weapon weapon;

    private void Start()
    {
        m_playerController = GetComponent<PlayerController>();
    }

    public void PlayerAttack()
    {
        foreach (Collider c in weapon.collisionEvents)
        {
            if (c == null) continue;

            if (c.gameObject.tag == "AI")
            {
                AI ai = c.gameObject.GetComponent<AI>();
                if (ai.aiType == AI.AI_TYPE.AI_TYPE_ENEMY)
                {
                    Debug.Log("Hit");
                    ((AIUnit)ai).EnemyHit(GetComponent<PlayerStats>().BasicAtk);
                }
                else if (ai.aiType == AI.AI_TYPE.AI_TYPE_BOSS)
                {
                    ((BossAI)ai).Damage(GetComponent<PlayerStats>().BasicAtk);
                }
            }
        }
    }
}

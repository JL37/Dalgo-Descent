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

    public void Attack()
    {
        foreach (Collider c in weaponCollider.collisionEvents)
        {
            if (c.gameObject.tag == "AI")
            {
                // c.GetComponent<>
            }
        }
    }
}

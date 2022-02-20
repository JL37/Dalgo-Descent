using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerController))]
public class PlayerAnimationEvents : MonoBehaviour
{
    private PlayerController m_playerController;

    private void Start()
    {
        m_playerController = GetComponent<PlayerController>();
    }

    public void Attack()
    {
    }
}

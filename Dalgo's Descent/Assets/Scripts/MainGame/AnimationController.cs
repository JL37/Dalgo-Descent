using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    protected Animator m_Animator;
    protected float m_PrevSpd;

    private void Start()
    {
        m_Animator = GetComponent<Animator>();
    }

    public void PauseAnimation()
    {
        m_PrevSpd = m_Animator.speed > 0 ? m_Animator.speed : 1;
        m_Animator.speed = 0;
    }

    public void ResumeAnimation(float customSpd = -1)
    {
        m_Animator.speed = customSpd > 0 ? customSpd : m_PrevSpd;
    }
}

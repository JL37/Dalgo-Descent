using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    private AIUnit m_aiUnit;

    public float maxHealth;
    [HideInInspector]
    public float currentHealth;

    [Header("Blink Effect")]
    public float blinkIntensity;
    public float blinkDuration;
    private float m_blinkTimer;

    private SkinnedMeshRenderer m_SkinnedMeshRenderer;

    void Start()
    {
        m_aiUnit = GetComponent<AIUnit>();
        m_SkinnedMeshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
        currentHealth = maxHealth;
    }

    private void Update()
    {
        m_blinkTimer -= Time.deltaTime;
        float lerp = Mathf.Clamp01(m_blinkTimer / blinkDuration);
        float intensity = lerp * blinkIntensity;
        // Debug.Log(intensity);
        m_SkinnedMeshRenderer.materials[1].color = new Vector4(1, 1, 1, lerp);
    }

    public void TakeDamage(float amount)
    {
        m_aiUnit.m_animator.speed = 1f;
        m_aiUnit.m_rigidbody.isKinematic = false;
        m_aiUnit.m_agent.enabled = false;
        // m_aiUnit.m_rigidbody.velocity = Vector3.zero;

        currentHealth -= amount;
        if (currentHealth <= 0.0f)
        {
            Die();
        }

        m_blinkTimer = blinkDuration;
    }

    public void Die()
    {
        GetComponentInChildren<Animator>().SetTrigger("Die");
    }
}

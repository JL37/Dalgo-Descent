using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public float maxHealth;
    [HideInInspector]
    public float currentHealth;

    [Header("Blink Effect")]
    public float blinkDuration;
    private float m_blinkTimer;

    [Header("Death Animation Effect")]
    public GameObject[] models;
    public ParticleSystem deathParticles;
    public float deathDuration;
    public bool playDeathAnimation;
    private float m_deathTimer;
    private float lerp2 = 0.0f;

    private SkinnedMeshRenderer m_SkinnedMeshRenderer;

    void Start()
    {
        playDeathAnimation = false;
        m_SkinnedMeshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
        currentHealth = maxHealth;
    }

    private void Update()
    {
        m_blinkTimer -= Time.deltaTime;
        float lerp1 = Mathf.Clamp01(m_blinkTimer / blinkDuration);
        m_SkinnedMeshRenderer.materials[1].color = new Vector4(1, 1, 1, lerp1);

        if (currentHealth <= 0.0f && playDeathAnimation)
        {
            m_deathTimer += Time.deltaTime;
            Debug.Log(m_deathTimer);
            lerp2 = Mathf.Lerp(lerp2, deathDuration, Time.deltaTime);
        }

        if (m_deathTimer > deathDuration)
        {
            deathParticles.Play();
            foreach (var model in models)
                model.SetActive(false);

            if (m_deathTimer - deathDuration > 2f)
                Destroy(gameObject);
        }

        m_SkinnedMeshRenderer.materials[2].color = new Vector4(1, 1, 1, lerp2);
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        if (currentHealth <= 0.0f)
        {
            Die();
        }

        m_blinkTimer = blinkDuration;
    }

    public void Die()
    {
        GetComponentInChildren<Animator>().SetBool("Death", true);
    }

    public void DieAnimation()
    {
        m_deathTimer = 0.0f;
        playDeathAnimation = true; 
    }
}

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
    private ObjectPoolManager m_UIPoolManager;

    void Start()
    {
        m_aiUnit = GetComponent<AIUnit>();
        m_SkinnedMeshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
        currentHealth = maxHealth;
        m_UIPoolManager = GameObject.FindGameObjectWithTag("HUD").GetComponent<GameUI>().GetObjectPoolManager();
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
        SpawnText(((int)(amount)).ToString());
    }

    protected void SpawnText(string txt)
    {
        txt = "<color=red>" + txt + "</color>";
        GameObject obj = m_UIPoolManager.GetFromPool();

        //Initialisation
        obj.GetComponent<DamageTextUI>().Initialise(transform, txt, 1f);
    }

    public void Die()
    {
        GetComponentInChildren<Animator>().SetTrigger("Die");
    }
}

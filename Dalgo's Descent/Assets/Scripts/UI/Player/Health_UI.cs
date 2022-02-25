using System;
using UnityEngine;
using UnityEngine.UI;

public class Health_UI : MonoBehaviour
{
    private PlayerStats playerStats;

    [Header("Linked objects")]
    public Image m_HealthBar, m_BufferBar, m_PlayerIcon;
    public Sprite m_Healthy, m_HalfDead, m_CriticalHealth, m_Dead;

    [Header("Variables to adjust")]
    public float m_HealthLerpSpd = 0.25f;
    public float m_BufferLerpSpd = 0.1f;
    public float m_MaxBufferTimer = 0.4f;

    protected float m_TargetHealthFillAmt = 0;
    protected float m_CurrBufferTimer = 0;

    public void Setup(PlayerStats playerStats)
    {
        this.playerStats = playerStats;

        playerStats.onHealthChanged += PlayerStats_onHealthChanged;
        playerStats.onHalfHealth += PlayerStats_onHalfHealth;
        playerStats.onCriticalHealth += PlayerStats_onCriticalHealth;
        playerStats.onDead += PlayerStats_onDead;
        playerStats.onHealthy += PlayerStats_onHealthy;

        m_TargetHealthFillAmt = m_HealthBar.fillAmount;
    }
    private void PlayerStats_onHalfHealth(object sender, EventArgs e)
    {
        m_PlayerIcon.sprite = m_HalfDead;
    }

    private void PlayerStats_onCriticalHealth(object sender, EventArgs e)
    {
        m_PlayerIcon.sprite = m_CriticalHealth;
    }

    private void PlayerStats_onDead(object sender, EventArgs e)
    {
        m_PlayerIcon.sprite = m_Dead;
    }
    private void PlayerStats_onHealthy(object sender, EventArgs e)
    {
        m_PlayerIcon.sprite = m_Healthy;
    }

    private void PlayerStats_onHealthChanged(object sender, EventArgs e)
    {
        
        m_TargetHealthFillAmt = playerStats.GetHealthPerc();
    }

    private void Update()
    {
        if (GameStateManager.Get_Instance.CurrentGameState == GameState.Paused) //Ignore key presses when paused
            return;

        UpdateHealthBar();
        UpdateBufferBar();
    }

    protected void UpdateBufferBar()
    {
        //Check if target is reached. If reached, return value
        if (Mathf.Abs(m_BufferBar.fillAmount - m_HealthBar.fillAmount) < 0.01f)
        {
            m_BufferBar.fillAmount = m_HealthBar.fillAmount;
            m_CurrBufferTimer = m_MaxBufferTimer;
            return;
        }

        //Timer offset (Bar will linger for awhile before going down in value)
        if (m_CurrBufferTimer > 0)
        {
            m_CurrBufferTimer -= Time.deltaTime;
            return;
        }

        //Lerping animation
        float targetFill = m_HealthBar.fillAmount;
        float currFill = m_BufferBar.fillAmount;

        currFill = Mathf.Lerp(currFill, targetFill, m_BufferLerpSpd);
        m_BufferBar.fillAmount = currFill;
    }

    protected void UpdateHealthBar()
    {
        m_HealthBar.fillAmount = Mathf.Lerp(m_HealthBar.fillAmount, m_TargetHealthFillAmt, m_HealthLerpSpd);
    }
}
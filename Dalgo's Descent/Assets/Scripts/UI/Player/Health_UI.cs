using System;
using UnityEngine;
using UnityEngine.UI;

public class Health_UI : MonoBehaviour
{
    private PlayerStats playerStats;

    public Image healthBar,playerIcon;
    public Sprite healthy, halfDead, criticalHealth, dead;
    public void Setup(PlayerStats playerStats)
    {
        this.playerStats = playerStats;

        playerStats.onHealthChanged += PlayerStats_onHealthChanged;
        playerStats.onHalfHealth += PlayerStats_onHalfHealth;
        playerStats.onCriticalHealth += PlayerStats_onCriticalHealth;
        playerStats.onDead += PlayerStats_onDead;
        playerStats.onHealthy += PlayerStats_onHealthy;

        
    }
    private void PlayerStats_onHalfHealth(object sender, EventArgs e)
    {
        playerIcon.sprite = halfDead;
    }

    private void PlayerStats_onCriticalHealth(object sender, EventArgs e)
    {
        playerIcon.sprite = criticalHealth;
    }

    private void PlayerStats_onDead(object sender, EventArgs e)
    {
        playerIcon.sprite = dead;
    }
    private void PlayerStats_onHealthy(object sender, EventArgs e)
    {
        playerIcon.sprite = healthy;
    }

    private void PlayerStats_onHealthChanged(object sender, EventArgs e)
    {
        Debug.Log("Hello");
        healthBar.fillAmount = playerStats.GetHealthPerc();
    }

    private void Update()
    {

        //transform.Find("healthbar").localScale = new Vector3(playerStats.GetHealthPerc(), 1);
    }
}
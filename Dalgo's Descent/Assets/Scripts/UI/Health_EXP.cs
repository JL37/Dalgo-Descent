using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health_EXP : MonoBehaviour
{
    private PlayerStats playerStats;

    public Image coverImage,expImage,playerIcon;
    public Sprite healthy, halfDead, criticalHealth, dead;
    public void Setup(PlayerStats playerStats)
    {
        this.playerStats = playerStats;

        playerStats.onHealthChanged += PlayerStats_onHealthChanged;
        playerStats.onEXPChanged += PlayerStats_onEXPChanged;
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
        transform.localScale = new Vector3(playerStats.GetHealthPerc() * 0.85f, 0.9f);
        if(transform.localScale.x <=.1f)
        {
            var tempColor = coverImage.color;
            tempColor.a = 0.0f;
            coverImage.color = tempColor;
        }
        else
        {
            var tempColor = coverImage.color;
            tempColor.a = 1f;
            coverImage.color = tempColor;
        }
    }

    private void PlayerStats_onEXPChanged(object sender, EventArgs e) //change the health only when the player gets attack
    {
        expImage.transform.localScale = new Vector3(playerStats.GetEXPPerc() * 0.95f, 1.0f);
    }

    private void Update()
    {

        //transform.Find("healthbar").localScale = new Vector3(playerStats.GetHealthPerc(), 1);
    }
}
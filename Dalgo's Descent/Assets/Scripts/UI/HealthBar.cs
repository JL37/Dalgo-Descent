using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    private PlayerStats playerStats;
    public void Setup(PlayerStats playerStats)
    {
        this.playerStats = playerStats;

        playerStats.onHealthChanged += PlayerStats_onHealthChanged;
    }

    private void PlayerStats_onHealthChanged(object sender, EventArgs e)
    {
        transform.Find("healthbar").localScale = new Vector3(playerStats.GetHealthPerc(), 1);
    }

    private void Update()
    {

        //transform.Find("healthbar").localScale = new Vector3(playerStats.GetHealthPerc(), 1);
    }
}

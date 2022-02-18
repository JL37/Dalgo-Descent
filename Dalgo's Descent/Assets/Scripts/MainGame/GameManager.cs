using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public HealthBar healthBar;
    public PlayerStats playerStats;
    void Start()
    {
        playerStats = new PlayerStats();
        healthBar.Setup(playerStats);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Awake()
    {
        
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.LeftAlt))
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        if (Input.GetKeyUp(KeyCode.LeftAlt))
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
        if (Input.GetKeyDown(KeyCode.M))
        {
            Debug.Log("Attack");
            playerStats.Damage(5);
            print("health now is : " + playerStats.Health);
        }

    }
}
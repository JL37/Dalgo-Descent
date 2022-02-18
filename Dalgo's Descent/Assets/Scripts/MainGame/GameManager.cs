using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;

public class GameManager : MonoBehaviour
{
    public Health_EXP status;
    public PlayerStats playerStats;
    [SerializeField] DynamicCamera m_Camera;
    protected PauseController m_PauseController;
    
    //protected int m_EnemiesToFight = 0;

    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        playerStats = new PlayerStats();
        status.Setup(playerStats);

        m_PauseController = GetComponent<PauseController>();
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
        if (Input.GetKeyDown(KeyCode.M)) // testing for receiving damage
        {
            Debug.Log("Attack");
            playerStats.Received_Damage(5);
            print("health now is : " + playerStats.Health);
        }
        if (Input.GetKeyDown(KeyCode.N)) //testing for exp
        {
            Debug.Log("EXP");
            playerStats.EXP_Update(5);
            print("EXP now is : " + playerStats.EXP);
        }
        if (Input.GetKeyDown(KeyCode.L)) //testing too add health
        {
            Debug.Log("Heal");
            playerStats.Replenish_Health(5);
            print("health now is  : " + playerStats.Health);
        }
    }
}
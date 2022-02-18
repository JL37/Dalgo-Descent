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

    protected List<GameObject> m_EnemyArr;
    protected bool m_InCombat = false;

    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        playerStats = new PlayerStats();
        status.Setup(playerStats);

        m_PauseController = GetComponent<PauseController>();
        m_EnemyArr = new List<GameObject>();
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

        if (m_EnemyArr.Count > 0)
            m_InCombat = true;
        else
            m_InCombat = false;
    }

    public bool GetInCombat() { return m_InCombat; }
    public void AddToEnemyArray(GameObject enemy)
    {
        for (int i = 0; i < m_EnemyArr.Count;++i)
        {
            if (m_EnemyArr[i] == enemy)
                return;
        }

        m_EnemyArr.Add(enemy);
        print("Enemy " + enemy + " added to manager!");
    }

    public void RemoveFromEnemyArray(GameObject enemy)
    {
        for (int i = 0; i < m_EnemyArr.Count; ++i)
        {
            if (m_EnemyArr[i] == enemy)
            {
                print("Enemy " + enemy + " removed from manager!");
                m_EnemyArr.RemoveAt(i);
                return;
            }
        }

        print("Enemy to remove from game manager array is not even in array.");

    }
}
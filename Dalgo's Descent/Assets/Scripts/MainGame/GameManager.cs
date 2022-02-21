using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>   
{
    public Health_UI m_healthUI;
    public PlayerStats playerStats;

    [SerializeField] DynamicCamera m_Camera;


    protected List<GameObject> m_EnemyArr;
    protected bool m_InCombat = false;

    [SerializeField] LevelWindow levelWindow;
    private LevelSystem m_LevelSystem;
    private LevelSystemAnimated m_levelSystemAnimated;

    public Texture2D cursorTexture;
    public CursorMode cursorMode = CursorMode.Auto;
    public Vector2 hotSpot = Vector2.zero;

    void Start()
    {
        playerStats = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>();
        m_healthUI.Setup(playerStats);
        m_EnemyArr = new List<GameObject>();       
    }

    void Awake()
    {
        m_LevelSystem = new LevelSystem();
        levelWindow.SetLevelSystem(m_LevelSystem);
        m_levelSystemAnimated = new LevelSystemAnimated(m_LevelSystem);
        levelWindow.SetLevelSystemAnimated(m_levelSystemAnimated);
    }

    void Update()
    {
        m_levelSystemAnimated.Update();

        if (GameStateManager.Get_Instance.CurrentGameState == GameState.Paused) //Ignore key presses when paused
            return;

        if (Input.GetKeyDown(KeyCode.M)) // testing for receiving damage
        {
            Debug.Log("Attack");
            playerStats.Received_Damage(5);
            print("health now is : " + playerStats.Health);
        }
        if (Input.GetKeyDown(KeyCode.N)) //testing for exp
        {
            m_LevelSystem.AddExperience(60);

        }
        if(Input.GetMouseButtonDown(0))
        {
            OnMouseEnter();
        }
        if (Input.GetMouseButtonUp(0))
        {
            OnMouseExit();
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

    void OnMouseEnter()
    {
        Cursor.SetCursor(cursorTexture, hotSpot, cursorMode);
    }

    void OnMouseExit()
    {
        Cursor.SetCursor(null, Vector2.zero, cursorMode);
    }

    public bool GetInCombat() { return m_InCombat; }
    public void AddToEnemyArray(GameObject enemy)
    {
        for (int i = 0; i < m_EnemyArr.Count; ++i)
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
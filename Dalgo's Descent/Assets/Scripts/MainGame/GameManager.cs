using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>   
{
    public Health_UI m_healthUI;
    public PlayerStats playerStats;

    [Header("Objects")]
    [SerializeField] DynamicCamera m_Camera;
    [SerializeField] GameUI m_VisibleCanvas;
    [SerializeField] ObjectPoolManager m_WorldCanvasPool;

    protected List<GameObject> m_EnemyArr;
    protected bool m_InCombat = false;
    protected bool m_GameOver = false;

    [SerializeField] LevelWindow levelWindow;
    private LevelSystem m_LevelSystem;
    private LevelSystemAnimated m_levelSystemAnimated;

    protected PostGameInfo m_PostGameInfo;
    
    // public UI_SkillTree skill1,skill2,skill3,skill4,healthUpgrade;

    void Start()
    {
        playerStats = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>();
        m_healthUI.Setup(playerStats);
        m_EnemyArr = new List<GameObject>();
        Tooltip.HideTooltip_Static();
        Tooltip_Warning.HideTooltip_Static();

        m_PostGameInfo = PostGameInfo.GetInstance();
        m_PostGameInfo.Reset();

        AudioManager.Instance.Play("Gameplay");
    }

    protected override void OnAwake()
    {
        m_LevelSystem = new LevelSystem();
        levelWindow.SetLevelSystem(m_LevelSystem);
        m_levelSystemAnimated = new LevelSystemAnimated(m_LevelSystem);
        levelWindow.SetLevelSystemAnimated(m_levelSystemAnimated);
    }

    void Update()
    {
        if (GameStateManager.Get_Instance.CurrentGameState == GameState.Paused) //Ignore key presses when paused
            return;

        m_PostGameInfo.UpdateTime(Time.deltaTime);

        if (!playerStats && !m_GameOver)
        {
            //Run animation
            GameOver();
            m_GameOver = true;
        }

        m_levelSystemAnimated.Update();

#if UNITY_EDITOR
        
        if (Input.GetKeyDown(KeyCode.M)) // testing for receiving damage
        {
            Debug.Log("Attack");
            playerStats.Received_Damage(5);
            //print("health now is : " + playerStats.Health);
        }
        if (Input.GetKeyDown(KeyCode.N)) //testing for exp
        {
            m_LevelSystem.AddExperience(7);

        }
        if (Input.GetKeyDown(KeyCode.L)) //testing too add health
        {
            Debug.Log("Heal");
            playerStats.Replenish_Health(5);
            //print("health now is  : " + playerStats.Health);
        }

#endif
        if (m_EnemyArr.Count > 0)
            m_InCombat = true;
        else
            m_InCombat = false;
    }

    public bool ReturnGameOver() { return m_GameOver; }

    public void GameOver()
    {
        m_VisibleCanvas.FadeOutGame(false);
    }

    public void Victory()
    {
        m_VisibleCanvas.FadeOutGame(true);
    }

    public void EnableBossHealthUI(Health health)
    {
        m_VisibleCanvas.EnableBossUI(health);
    }

    public void DisableBossHealthUI()
    {
        m_VisibleCanvas.DisableBossUI();
    }

    public EnemyHealthUI ActivateEnemyHealthUI(Health health)
    {
        EnemyHealthUI enemyHealth = m_WorldCanvasPool.GetFromPool().GetComponent<EnemyHealthUI>();
        enemyHealth.SetHealth(health);
        enemyHealth.StartFadeAnimation(false);
        enemyHealth.SetTarget(health.gameObject);

        return enemyHealth;
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
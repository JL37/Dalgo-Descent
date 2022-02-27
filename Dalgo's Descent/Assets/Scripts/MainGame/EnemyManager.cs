using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyManager : MonoBehaviour
{
    private LevelStructure m_AssociatedLevel;

    public BossAI boss;

    [Header("Enemy")]
    [SerializeField] GameObject m_EnemyPrefab;
    [SerializeField] Transform m_EnemyHolder;

    // Total number of enemies.
    //[HideInInspector]
    public List<AI> m_Enemies;
    private bool hasSpawnedEnemies;

    private int m_Wave;
    private int m_NumWaves;

    public bool BossKilled { get; set; }
    public bool LevelComplete { get; private set; }

    private void Start()
    {
        m_AssociatedLevel = GetComponent<LevelStructure>();
        m_EnemyHolder = GameObject.FindGameObjectWithTag("EnemyParent").transform;
        m_NumWaves = DifficultyManager.Instance.NumWaves;
        BossKilled = false;

        SpawnEnemiesInNewLevel();
    }

    private void Update()
    {
        //#if UNITY_EDITOR
        //if (Input.GetKeyDown(KeyCode.Y))
        //{
        //    SpawnEnemies(1, 1, new Vector3(0, FindObjectOfType<PlayerController>().transform.position.y, 0));
        //}

        //if (Input.GetKeyDown(KeyCode.T))
        //{
        //    SpawnMiniboss(new Vector3(0, FindObjectOfType<PlayerController>().transform.position.y, 0));
        //}
        //#endif

        if (m_Enemies.Count <= 0 && m_Wave < m_NumWaves)
        {
            SpawnNextWaveOfEnemies();
        }
        else if (m_Enemies.Count <= 0 && (m_Wave == m_NumWaves || BossKilled))
        {
            if(!LevelComplete)
            {
                LevelComplete = true;
                GetComponent<LevelStructure>().OnLevelComplete();
            }
        }
    }

    public void SpawnEnemies(float timeToSpawnEnemies, int EnemyCount, Vector3 position)
    {
        StartCoroutine(DoSpawnEnemies(timeToSpawnEnemies, EnemyCount, position));
    }

    public void SpawnMiniboss(Vector3 position)
    {
        NavMeshHit hit;

        if (NavMesh.SamplePosition(position, out hit, 5f, NavMesh.AllAreas))
        {
            AIUnit newEnemy = Instantiate(m_EnemyPrefab, m_EnemyHolder).GetComponent<AIUnit>();
            newEnemy.agent.Warp(position);
            newEnemy.Init(3f, DifficultyManager.Instance.difficultyScaling * 2f, true, this);
            newEnemy.transform.rotation = Quaternion.Euler(new Vector3(0, Random.Range(0, 360), 0));
            m_Enemies.Add(newEnemy.GetComponent<AIUnit>());
        }
    }

    public BossAI SpawnBoss(Vector3 position, EnemyManager enemyManager)
    {
        BossAI boss = GameLevelManager.Instance.BossObject;
        boss.gameObject.SetActive(true);
        boss.Init(DifficultyManager.Instance.difficultyScaling, enemyManager);
        boss.agent.Warp(position);

        return boss;
    }

    IEnumerator DoSpawnEnemies(float timeToSpawnEnemies, int EnemyCount, Vector3 position)
    {
        float radiusOffset = 10f;
        int enemiesSpawned = 0;
        WaitForSeconds timeBetweenNextEnemy = new WaitForSeconds(timeToSpawnEnemies / (float)EnemyCount);

        while (enemiesSpawned < EnemyCount)
        { 
            Debug.Log("Spawning enemy " + enemiesSpawned);

            // Check if the position the enemy is trying to spawn in is available on the navmesh. If it is not, do not spawn the enemy and continue.
            Vector3 newPosition = new Vector3(position.x + Random.Range(-radiusOffset, radiusOffset), position.y, position.z + Random.Range(-radiusOffset, radiusOffset));
            NavMeshHit hit;

            if (NavMesh.SamplePosition(newPosition, out hit, 5f, NavMesh.AllAreas))
            {
                GameObject newEnemy = Instantiate(m_EnemyPrefab, m_EnemyHolder);
                m_Enemies.Add(newEnemy.GetComponent<AIUnit>());
                newEnemy.GetComponent<NavMeshAgent>().Warp(newPosition);

                float enemySize = Random.Range(0.7f, 1.5f);
                newEnemy.GetComponent<AIUnit>().Init(enemySize, DifficultyManager.Instance.difficultyScaling * enemySize, false, this);
                newEnemy.transform.rotation = Quaternion.Euler(new Vector3(0, Random.Range(0, 360), 0));

                enemiesSpawned++;
                yield return timeBetweenNextEnemy;
            }
            yield return null;
        }
    }

    public void SpawnNextWaveOfEnemies()
    {
        if (!hasSpawnedEnemies)
        {
            hasSpawnedEnemies = true;
            StartCoroutine(DoSpawnNextWaveOfEnemies());
        }
    }

    public void SpawnEnemiesInNewLevel()
    {
        m_Wave = 0;
        SpawnNextWaveOfEnemies();
    }

    IEnumerator DoSpawnNextWaveOfEnemies()
    {
        yield return new WaitForSeconds(2f);
        m_Wave++;
        if (!GameLevelManager.Instance.IsLastLevel())
        {
            if (m_Wave < m_NumWaves)
                SpawnEnemies(1f, DifficultyManager.Instance.NumEnemiesPerWave, m_AssociatedLevel.EnemySpawnLocation.position);
            else if (m_Wave == m_NumWaves)
                SpawnMiniboss(m_AssociatedLevel.EnemySpawnLocation.position);
            hasSpawnedEnemies = false;
        }
        else
        {
            boss = SpawnBoss(m_AssociatedLevel.EnemySpawnLocation.position, this);
            m_Enemies.Add(boss);
        }
    }

    public void DestroyAllEnemies()
    {
        foreach(var enemy in m_EnemyHolder.GetComponentsInChildren<AIUnit>())
        {
            Destroy(enemy.gameObject);
        }
    }
}

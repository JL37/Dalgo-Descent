using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyManager : MonoBehaviour
{

    [Header("Enemy")]
    [SerializeField] GameObject m_EnemyPrefab;
    [SerializeField] Transform m_EnemyHolder;

    // Total number of enemies.
    [HideInInspector] public List<AI> m_Enemies;
    bool hasSpawnedEnemies;

    private int m_Wave;
    private int m_NumWaves;

    bool LevelComplete = false;

    private void Start()
    {
        m_EnemyHolder = GameObject.FindGameObjectWithTag("EnemyParent").transform;
        m_NumWaves = DifficultyManager.Instance.NumWaves;
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
        else if (m_Enemies.Count <= 0 && m_Wave == m_NumWaves)
        {
            LevelComplete = true;
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
            GameObject newEnemy = Instantiate(m_EnemyPrefab, m_EnemyHolder);
            newEnemy.GetComponent<NavMeshAgent>().Warp(position);
            newEnemy.GetComponent<AIUnit>().Init(3f, DifficultyManager.Instance.difficultyScaling * 2f, true, this);
            newEnemy.transform.rotation = Quaternion.Euler(new Vector3(0, Random.Range(0, 360), 0));
        }
    }

    IEnumerator DoSpawnEnemies(float timeToSpawnEnemies, int EnemyCount, Vector3 position)
    {
        float radiusOffset = 10f;
        int enemiesSpawned = 0;
        WaitForSeconds wfs = new WaitForSeconds(timeToSpawnEnemies / (float)EnemyCount);
        while (enemiesSpawned < EnemyCount)
        { 
            Debug.Log("Spawning enemy " + enemiesSpawned);
            Vector3 newPosition = new Vector3(position.x + Random.Range(-radiusOffset, radiusOffset), position.y, position.z + Random.Range(-radiusOffset, radiusOffset));
            NavMeshHit hit;

            if (NavMesh.SamplePosition(newPosition, out hit, 5f, NavMesh.AllAreas))
            {
                GameObject newEnemy = Instantiate(m_EnemyPrefab, m_EnemyHolder);
                m_Enemies.Add(newEnemy.GetComponent<AIUnit>());
                newEnemy.GetComponent<NavMeshAgent>().Warp(newPosition);
                float size = Random.Range(0.7f, 1.5f);
                newEnemy.GetComponent<AIUnit>().Init(size, DifficultyManager.Instance.difficultyScaling * size, false, this);
                newEnemy.transform.rotation = Quaternion.Euler(new Vector3(0, Random.Range(0, 360), 0));

                enemiesSpawned++;
            }
            yield return wfs;
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
        yield return new WaitForSeconds(1f);
        m_Wave++;
        if (m_Wave < m_NumWaves)
            SpawnEnemies(1f, DifficultyManager.Instance.NumEnemiesPerWave, GameLevelManager.Instance.EnemySpawnLocation.position);
        else if (m_Wave == m_NumWaves)
            SpawnMiniboss(GameLevelManager.Instance.EnemySpawnLocation.position);

        hasSpawnedEnemies = false;
    }
}

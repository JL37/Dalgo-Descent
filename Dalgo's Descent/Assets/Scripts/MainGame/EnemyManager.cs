using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyManager : MonoBehaviour
{

    [Header("Enemy")]
    [SerializeField] GameObject m_EnemyPrefab;
    [SerializeField] Transform m_EnemyHolder;

    [HideInInspector] public List<AI> m_Enemies;

    private void Update()
    {
        #if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.Y))
        {
            SpawnEnemies(1, 1, new Vector3(0, FindObjectOfType<PlayerController>().transform.position.y, 0));
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            SpawnMiniboss(new Vector3(0, FindObjectOfType<PlayerController>().transform.position.y, 0));
        }
#endif
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
            newEnemy.GetComponent<AIUnit>().Init(3f, DifficultyManager.Instance.difficultyScaling * 3f, true);
            newEnemy.transform.rotation = Quaternion.Euler(new Vector3(0, Random.Range(0, 360), 0));
        }
    }

    IEnumerator DoSpawnEnemies(float timeToSpawnEnemies, int EnemyCount, Vector3 position)
    {
        float radiusOffset = 5f;
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
                newEnemy.GetComponent<NavMeshAgent>().Warp(newPosition);
                float size = Random.Range(0.7f, 1.5f);
                newEnemy.GetComponent<AIUnit>().Init(size, DifficultyManager.Instance.difficultyScaling * size, false);
                newEnemy.transform.rotation = Quaternion.Euler(new Vector3(0, Random.Range(0, 360), 0));

                enemiesSpawned++;
            }
            yield return wfs;
        }
    }
}

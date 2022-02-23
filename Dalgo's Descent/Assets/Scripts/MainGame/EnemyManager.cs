using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{

    [Header("Enemy")]
    [SerializeField] GameObject m_EnemyPrefab;
    [SerializeField] Transform m_Enemies;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Y))
        {
            SpawnEnemies(1, 5, new Vector3(10, 0, 10));
        }
    }

    public void SpawnEnemies(float timeToSpawnEnemies, int EnemyCount, Vector3 position)
    {
        StartCoroutine(DoSpawnEnemies(timeToSpawnEnemies, EnemyCount, position));
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
            GameObject newEnemy = Instantiate(m_EnemyPrefab, m_Enemies);
            newEnemy.GetComponent<AIUnit>().Init(Random.Range(0.7f, 1.5f));
            newEnemy.transform.position = newPosition;
            newEnemy.transform.rotation = Quaternion.Euler(new Vector3(0, Random.Range(0, 360), 0));
            enemiesSpawned++;
            yield return wfs;
        }
    }
}

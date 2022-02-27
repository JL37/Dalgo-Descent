using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    [Header("Variables to adjust")]
    [SerializeField] string m_ChestPoolName;
    protected ObjectPoolManager m_ChestPool;

    [SerializeField] float m_Radius = 10.5f;
    [SerializeField] Transform m_SpawnPos;

    protected bool m_IsSpawning = true;

    // Start is called before the first frame update
    void Start()
    {
        m_ChestPool = GameObject.Find(m_ChestPoolName).GetComponent<ObjectPoolManager>();

        //Disable all instances first...
        GameObject poolFolder = GameObject.Find("ObjectPooling");
        foreach (Transform pool in poolFolder.transform)
        {
            pool.GetComponent<ObjectPoolManager>().DisableAll();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (m_IsSpawning)
        {
            SpawnChests(Random.Range(1, 2) * (int)DifficultyManager.Instance.difficultyScaling);
            m_IsSpawning = false;
        }
    }

    public void SpawnChests(int num, int limit = 12)
    {   
        int max = (num <= limit ? num : limit);
        float degree = 360f / max;

        Vector3 dir = new Vector3(0, 0, 1);

        for (int i = 0; i < max; ++i)
        {
            GameObject chest = m_ChestPool.GetFromPool();
            Vector3 basePos = m_SpawnPos.position;
            basePos.y -= 0.2f;

            chest.transform.position = basePos + (dir * m_Radius);
            chest.transform.LookAt(m_SpawnPos);

            Vector3 newRotate = chest.transform.rotation.eulerAngles;
            newRotate.y = 0;
            chest.transform.rotation = Quaternion.Euler(newRotate);
            chest.transform.Rotate(Vector3.up, 180);

            //Rotate vector
            dir = Quaternion.Euler(0, degree, 0) * dir;
        }
    }
}

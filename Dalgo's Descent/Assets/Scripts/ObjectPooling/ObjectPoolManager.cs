using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolManager : MonoBehaviour
{
    //Very basic for now, will add more if i have to
    [Header("Variables that affects the manager")]
    [SerializeField] int m_MaxObject = 50;
    [SerializeField] int m_InitialSize = 10;
    [SerializeField] GameObject prefab;

    protected List<GameObject> poolArr;

    ~ObjectPoolManager()
    {
        for (int i = 0; i < poolArr.Count; ++i)
        {
            Destroy(poolArr[i]);
        }

        poolArr.Clear();
    }

    // Start is called before the first frame update
    void Start()
    {
        poolArr = new List<GameObject>();

        for (int i = 0; i < m_InitialSize;++i)
        {
            GameObject obj = Instantiate(prefab, transform);
            obj.SetActive(false);
            poolArr.Add(obj);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (poolArr.Count > m_MaxObject)
            Flush();
    }

    public void Flush()
    {
        for (int i = 0; i < poolArr.Count; ++i)
        {
            if (!poolArr[i].activeSelf) //Destroy object if inactive
            {
                Destroy(poolArr[i]);
                poolArr.RemoveAt(i);
                i -= 1;
            }
        }
    }

    public void DisableAll()
    {
        for (int i = 0; i < poolArr.Count; ++i)
        {
            poolArr[i].SetActive(false);
        }
    }
    
    public GameObject GetFromPool()
    {
        for (int i = 0; i < poolArr.Count;++i)
        {
            if (!poolArr[i].activeSelf)
            {
                poolArr[i].SetActive(true);
                return poolArr[i];
            }
        }

        //Create if there's noone free
        GameObject obj = Instantiate(prefab, transform);
        poolArr.Add(obj);

        return obj;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;

public class GameManager : Singleton<GameManager>   
{
    [SerializeField] DynamicCamera m_Camera;

    protected PauseController m_PauseController;
    protected List<GameObject> m_EnemyArr;
    protected bool m_InCombat = false;

    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

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
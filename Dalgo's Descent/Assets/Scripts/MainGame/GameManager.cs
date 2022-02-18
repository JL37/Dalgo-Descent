using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;

public class GameManager : MonoBehaviour
{
    [SerializeField] DynamicCamera m_Camera;

    protected PauseController m_PauseController;
    protected List<GameObject> m_EnemyArr;

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
            m_Camera.SetInCombat(true);
        else
            m_Camera.SetInCombat(false);
    }
}
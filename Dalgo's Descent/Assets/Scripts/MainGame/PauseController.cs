using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PauseController : MonoBehaviour
{
    [Header("Objects")]
    public GameObject m_PausePanel;
    [SerializeField] CinemachineFreeLook m_FreeLookCamera = null;

    protected float m_CamSpdX = 0;
    protected float m_CamSpdY = 0;

    private void Start()
    {
        if (m_FreeLookCamera)
        {
            m_CamSpdX = m_FreeLookCamera.m_XAxis.m_MaxSpeed;
            m_CamSpdY = m_FreeLookCamera.m_YAxis.m_MaxSpeed;
        }
    }

    // Update is called once per frame

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }
    public void ButtonClicked()
    {
        if(m_PausePanel.activeInHierarchy == true)
        {
            m_PausePanel.SetActive(false);
            Cursor.visible = false;
            GameState currentGameState = GameStateManager.Get_Instance.CurrentGameState;
            GameState newGameState = currentGameState == GameState.Gameplay ? GameState.Gameplay : GameState.Gameplay; //changing the gamestate to gameplay
            GameStateManager.Get_Instance.SetState(newGameState);

            CameraToggle();
        }
        else
        {
            m_PausePanel.SetActive(true);
        }
    }    

    protected void TogglePause()
    {
        GameState currentGameState = GameStateManager.Get_Instance.CurrentGameState;
        GameState newGameState = currentGameState == GameState.Gameplay ? GameState.Paused : GameState.Gameplay; //changing the gamestate to pause
        GameStateManager.Get_Instance.SetState(newGameState);
        m_PausePanel.SetActive(!m_PausePanel.activeSelf);
        Debug.Log("Paused Toggled");

        CameraToggle();
    }

    protected void CameraToggle()
    {
        if (!m_FreeLookCamera)
            return;

        if (GameStateManager.Get_Instance.CurrentGameState == GameState.Paused)
        {
            m_FreeLookCamera.m_XAxis.m_MaxSpeed = 0;
            m_FreeLookCamera.m_YAxis.m_MaxSpeed = 0;
        }
        else
        {
            m_FreeLookCamera.m_XAxis.m_MaxSpeed = m_CamSpdX;
            m_FreeLookCamera.m_YAxis.m_MaxSpeed = m_CamSpdY;
        }
    }
}

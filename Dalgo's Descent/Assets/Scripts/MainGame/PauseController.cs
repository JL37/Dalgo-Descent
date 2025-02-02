using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.InputSystem;

public class PauseController : MonoBehaviour
{
    [Header("Objects")]
    public GameObject m_PausePanel;
    public GameObject m_OptionPanel;
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
        if (GameManager.Instance.ReturnGameOver())
            return;

/*        if (Input.GetKeyDown(KeyCode.Escape))
            TogglePause();*/
        
    }
    public void ButtonClicked()
    {
        if (m_PausePanel.activeInHierarchy == true)
            TogglePause();
        else
        {
            m_PausePanel.SetActive(true);
            m_OptionPanel.SetActive(false);
        }
    }

    public bool IsPaused()
    {
        return GameStateManager.Get_Instance.CurrentGameState == GameState.Paused;
    }

    public void OnExit(InputAction.CallbackContext context)
    {
        if (context.started)
            TogglePause();
        
    }

    public void OnExitClick()
    {
        TogglePause();
    }

    protected void TogglePause()
    {
        GameState currentGameState = GameStateManager.Get_Instance.CurrentGameState;
        GameState newGameState = currentGameState == GameState.Gameplay ? GameState.Paused : GameState.Gameplay; //changing the gamestate to pause
        GameStateManager.Get_Instance.SetState(newGameState);
        print(newGameState.ToString());

        m_PausePanel.SetActive(!m_PausePanel.activeSelf);
        PauseManager.Instance.OpenMenu("Options");
        if (m_PausePanel.activeSelf)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }

        Debug.Log("Paused Toggled");

        Time.timeScale = GameStateManager.Get_Instance.CurrentGameState == GameState.Paused ? 0 : 1;
        CameraToggle(GameStateManager.Get_Instance.CurrentGameState == GameState.Paused);

    }

    public void CameraToggle(bool disable)
    {
        if (!m_FreeLookCamera)
            return;

        if (disable)
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

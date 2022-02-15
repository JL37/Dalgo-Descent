using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseController : MonoBehaviour
{
    public GameObject m_PausePanel;

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            GameState currentGameState = GameStateManager.Get_Instance.CurrentGameState;
            GameState newGameState = currentGameState == GameState.Gameplay ? GameState.Paused : GameState.Gameplay; //changing the gamestate to pause
            GameStateManager.Get_Instance.SetState(newGameState);
            m_PausePanel.SetActive(true);
            Debug.Log("Paused");
            
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
        }
        else
        {
            m_PausePanel.SetActive(true);
        }
    }    
}

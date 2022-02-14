using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseController : MonoBehaviour
{
    public GameObject PausePanel;

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            GameState currentGameState = GameStateManager.Get_Instance.CurrentGameState;
            GameState newGameState = currentGameState == GameState.Gameplay ? GameState.Paused : GameState.Gameplay; //changing the gamestate to pause
            GameStateManager.Get_Instance.SetState(newGameState);
            PausePanel.SetActive(true);
            Debug.Log("Paused");
        }
    }
    public void ButtonClicked()
    {
        if(PausePanel.activeInHierarchy == true)
            PausePanel.SetActive(false); 
        else
            PausePanel.SetActive(true);
    }    
}

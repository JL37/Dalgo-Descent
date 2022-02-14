using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainScreen : MonoBehaviour
{
    void Awake()
    {
        GameStateManager.Get_Instance.OnGameStateChanged += OnGameStateChanged;
    }

    void OnDestroy()
    {
        GameStateManager.Get_Instance.OnGameStateChanged -= OnGameStateChanged;
    }
    void Update()
    {

    }

    private void OnGameStateChanged(GameState newGameState) //change the game state to gameplay
    {
        enabled = newGameState == GameState.Gameplay;
    }
}

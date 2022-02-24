using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MouseCursor : MonoBehaviour
{
    public Texture2D mouseCursor;
    protected PauseController m_PauseController;

    Vector2 hotSpot = new Vector2(0, 0);
    CursorMode cursorMode = CursorMode.Auto;

    private void Start()
    {
        hotSpot.x = mouseCursor.width * 0.1f;
        hotSpot.y = mouseCursor.height * 0.22f;
        Cursor.SetCursor(mouseCursor, hotSpot, cursorMode);
        m_PauseController = GetComponent<PauseController>();
        //Cursor.visible = false;
        //Cursor.lockState = CursorLockMode.Locked;
        
    }

    // Update is called once per frame
    void Update()
    {
        if (GameStateManager.Get_Instance.CurrentGameState == GameState.Paused) //Ignore key presses when paused
            return;

        if (Input.GetKeyDown(KeyCode.LeftAlt))
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            m_PauseController.CameraToggle(true);
        }
        if (Input.GetKeyUp(KeyCode.LeftAlt))
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            m_PauseController.CameraToggle(false);
        }
    }
}

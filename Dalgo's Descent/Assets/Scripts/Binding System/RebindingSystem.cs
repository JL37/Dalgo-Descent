using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class RebindingSystem : MonoBehaviour
{
    [SerializeField] private InputActionReference move_forward;
    [SerializeField] private Text buttonText;
    


    private InputActionRebindingExtensions.RebindingOperation rebindingOperation;
    public void StartRebinding()
    {
        EventSystem.current.SetSelectedGameObject(null);
        buttonText.text = "PRESS ANY BUTTON";
        move_forward.action.Disable(); //disable action script to rebind
        rebindingOperation = move_forward.action.PerformInteractiveRebinding()
            .OnMatchWaitForAnother(0.1f)
            .OnComplete(
            operation =>
            {
                buttonText.text = InputControlPath.ToHumanReadableString(move_forward.action.bindings[0].effectivePath, InputControlPath.HumanReadableStringOptions.OmitDevice); //replace this text to the text that just bind
                rebindingOperation.Dispose(); //dispose to prevent memory leak
                move_forward.action.Enable(); //renable keybinding
            })
            .Start();
    }
}

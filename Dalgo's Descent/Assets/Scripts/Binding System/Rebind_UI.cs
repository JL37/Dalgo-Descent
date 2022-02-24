using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Rebind_UI : MonoBehaviour
{
    [SerializeField] private InputActionReference inputActionReference;

    [SerializeField] private bool excludeMouse = true;
    [Range(0, 10)] //increase the value if we have more keybinding
    [SerializeField] private int selectedBinding;
    [SerializeField] private InputBinding.DisplayStringOptions displayStringOptions;

    [Header("Binding Info --- DO NOT EDIT, VIEW ONLY!")]
    [SerializeField]
    private InputBinding inputBinding;
    private int bindingIndex;

    private string actionName;

    [Header("UI Fields")]
    [SerializeField] private Text actionText;
    [SerializeField] private Button rebindButton;
    [SerializeField] private Text rebindText;

    private void OnEnable()
    {
        rebindButton.onClick.AddListener(() => DoRebind()); //add listener for rebind

        if(inputActionReference!=null)
        {
            GetBindingInfo();
            UpdateUI();
        }
    }
    private void OnValidate()
    {
        if (inputActionReference != null)
            return;
        GetBindingInfo();
        UpdateUI();
    }

    private void GetBindingInfo()
    {
        if (inputActionReference != null)
            actionName = inputActionReference.action.name;

        if (inputActionReference.action.bindings.Count > selectedBinding)
        {
            inputBinding = inputActionReference.action.bindings[selectedBinding];
            bindingIndex = selectedBinding;
        }
    }

    private void UpdateUI()
    {
        if (actionText != null)
            actionText.text = actionName;

        if (rebindText != null)
        {
            if (Application.isPlaying)
            {
                //grab from input manager
            }
            else
                rebindText.text = inputActionReference.action.GetBindingDisplayString(bindingIndex);


        }
    }

    private void DoRebind()
    {
        InputManager.StartRebind(actionName, bindingIndex, rebindText);
    }
}

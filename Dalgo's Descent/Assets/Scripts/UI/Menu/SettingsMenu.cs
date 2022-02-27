using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class SettingsMenu : MenuBase
{
    //Variables for keybind
    public  Event events;
    private string keycode;
    public bool startListen = false;
    private InputActionReference  setAction = null;
    private Text setText;
    private int keyID = 0;


    [Header("Attack Properties")]
    public InputActionReference AttackAction;
    public Button AttackButton;
    public Text AttackText;

    [Header("Jump Properties")]
    public InputActionReference JumpAction;
    public Button JumpButton;
    public Text JumpText;

    [Header("Interact Properties")]
    public InputActionReference InteractAction;
    public Button InteractButton;
    public Text InteractText;

    [Header("Skills Properties")]
    public InputActionReference SkillAction;
    public Button SkillButton;
    public Text SkillText;

    [Header("Settings Properties")]
    public InputActionReference SettingsAction;
    public Button SettingsButton;
    public Text SettingsText;

    [Header("Cursor Properties")]
    public InputActionReference CursorAction;
    public Button CursorButton;
    public Text CursorText;

    private GameState currentGameState = GameStateManager.Get_Instance.CurrentGameState;
    public void Start()
    {
        //display respective keys from the system binding once the game starts
        /*        AttackText.text = AttackAction.action.GetBindingDisplayString(); 
                JumpText.text = JumpAction.action.GetBindingDisplayString(); 
                InteractText.text = InteractAction.action.GetBindingDisplayString(); 
                SkillText.text = SkillAction.action.GetBindingDisplayString(); 
                SettingsText.text = SettingsAction.action.GetBindingDisplayString(); 
                CursorText.text = CursorAction.action.GetBindingDisplayString();*/
        
    }
    public void SetVolume(float volume)
    {
        AudioManager.Instance.SetVolume(volume, "Gameplay");
        AudioManager.Instance.SetVolume(volume, "BossMusic");
    }

    public void SetQuality(int qualityIndex) //this does not work hmmm
    {
        QualitySettings.SetQualityLevel(qualityIndex);
    }

    public void SetFullScreen(bool isFullScreen)
    {
        Screen.fullScreen = isFullScreen;
    }



    #region Rebinding section

    public void UpdateText(int id)
    {
        switch(id)
        {
            case 5: //attack
                AttackText = setText;
                break;
            case 6: //jump
                JumpText = setText;
                break;
            case 7: //interact
                InteractText = setText;
                break;
            case 9: //skills
                SkillText = setText;
                break;
            case 10: //settings
                SettingsText = setText;
                break;
            case 11: //cursor
                CursorText = setText;
                break;

        }
    }

    public void AttackClick()
    {
        setAction = AttackAction;
        setText = AttackText;
        keyID = 5;
        StartBinding();
    }

    public void JumpClick()
    {
        setAction = JumpAction;
        setText = JumpText;
        keyID = 6;
        StartBinding();
    }

    public void InteractClick()
    {
        setAction = InteractAction;
        setText = InteractText;
        keyID = 7;
        StartBinding();
    }
    public void SkillClick()
    {
        setAction = SkillAction;
        setText = SkillText;
        keyID = 9;
        StartBinding();
    }
    public void SettingsClick()
    {
        setAction = SettingsAction;
        setText = SettingsText;
        keyID = 10;
        StartBinding();
    }
    public void CursorClick()
    {
        setAction = CursorAction;
        
        setText = CursorText;
        keyID = 11;
        StartBinding();
    }
    public void StartBinding()
    {
        startListen = true;
    }
 /*   public void ChangeBinding(string bindingKey)
    {
        InputBinding binding = jumpAction.action.bindings[0];
        bindingKey.ToLower();
        binding.overridePath = "<Keyboard>/#(" + bindingKey + ")";
        jumpAction.action.ApplyBindingOverride(0, binding);
        print(binding.ToDisplayString());
        startListen = false;
    }*/
    public void ChangeBinding(string bindingKey, int id, InputActionReference reference)
    {
        OverrideBinding(bindingKey, reference);
        // print(binding.ToDisplayString());
        UpdateText(id);
        startListen = false;

    }

    public static void OverrideBinding(string bindingKey, InputActionReference reference)
    {
        InputBinding binding = reference.action.bindings[0];
        binding.overridePath = bindingKey;
        reference.action.ApplyBindingOverride(0, binding);
    }

    public void OnGUI() //keybind listener
    {
        if (startListen)
        {
            events = Event.current;
            setText.text = "Press a key";
            if (events.isKey)
            {
                string key = events.keyCode.ToString();
                setText.text = key;
                this.keycode = key;
                ChangeBinding("<Keyboard>/" + this.keycode, keyID, setAction);
            }
            else if (events.isMouse)
            {
                string mouse = events.button.ToString();
                switch (mouse)
                {
                    case "0":
                        this.keycode = "<Mouse>/leftButton";
                        setText.text = "LMB";
                        break;
                    case "1":
                        this.keycode = "<Mouse>/rightButton";
                        setText.text = "RMB";
                        break;
                    default:
                        this.keycode = "<Mouse>/leftButton";
                        setText.text = "LMB";
                        break;

                }
                ChangeBinding(this.keycode, keyID, setAction);
            }
        }
        
    }

    public void QuitToMainMenu()
    {
        SceneManager.LoadScene(1);
    }
    #endregion
}

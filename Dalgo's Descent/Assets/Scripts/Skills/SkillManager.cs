using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class SkillManager : MonoBehaviour
{
    public PlayerInput InputScript;
    public List<Image> skill = new List<Image>();

    public bool isCooldown = false;
    public bool isSkill1Pressed = false;
    public bool isSkill2Pressed = false;
    public bool isSkill3Pressed = false;
    public bool isSkill4Pressed = false;

    void Awake()
    {
        InputScript = GetComponent<PlayerInput>();
    }

    void OnEnable()
    {
        InputScript.ActivateInput();

    }

    void OnDisable()
    {
        InputScript.DeactivateInput();
    }

    void Start()
    {
        for(int i = 0;i<skill.Count;i++)
        {
            skill[i].fillAmount = 1.0f;
        }
    }

    void Update()
    {
        Render_Skill_Cooldown();
    }

    public delegate void SkillState(Skill newSkill);
    public event SkillState SkillChangeTo; //call this to change skill

    #region InputAction
    public void OnSkillUsed(InputAction.CallbackContext context)
    {
        if (context.action.activeControl.name == "q")
        {
            Debug.Log("q press");
            isSkill1Pressed = true;
        }
        if (context.action.activeControl.name == "e")
        {
            Debug.Log("eeee press");
            isSkill2Pressed = true;
        }
        if (context.action.activeControl.name == "r")
        {
            Debug.Log("r press");
            isSkill3Pressed = true;
        }
        if (context.action.activeControl.name == "t")
        {
            Debug.Log("t press");
            isSkill4Pressed = true;
        }

    }
    #endregion
    public void Render_Skill_Cooldown()
    {
        if (isSkill1Pressed && !isCooldown) //if skill 1 key is pressed and cooldown is 0
        {
            isCooldown = true; //reset these var
            skill[0].fillAmount = 1.0f;  //display the image that has cooldown
        }
        if (isSkill2Pressed && !isCooldown) 
        {
            isCooldown = true; 
            skill[1].fillAmount = 1.0f;  
        }
        if (isSkill3Pressed && !isCooldown) 
        {
            isCooldown = true; 
            skill[2].fillAmount = 1.0f;  
        }
        if (isSkill4Pressed && !isCooldown) 
        {
            isCooldown = true; 
            skill[3].fillAmount = 1.0f;  
        }

        if (isCooldown) //while the skill is on cooldown
        {
            if(isSkill1Pressed)
            {
                float cooldown = 5.0f;
                isSkill1Pressed =false;
                
                skill[0].fillAmount -= 1 / cooldown * Time.deltaTime; //reduce the fill amount by 1 with respective to delta time
            }
            else if (isSkill2Pressed)
            {
                float cooldown = 5.0f;
                isSkill2Pressed = false;
                skill[1].fillAmount -= 1 / cooldown * Time.deltaTime;
            }
            else if (isSkill3Pressed)
            {
                float cooldown = 5.0f;
                isSkill3Pressed = false;
                skill[2].fillAmount -= 1 / cooldown * Time.deltaTime;
            }
            else if (isSkill4Pressed)
            {
                float cooldown = 5.0f;
                isSkill4Pressed = false;
                skill[3].fillAmount -= 1 / cooldown * Time.deltaTime;
            }
            
            for (int i = 0; i < skill.Count; i++)
            {
                if(skill[i].fillAmount <-0.0f)
                {
                    skill[i].fillAmount = 1.0f;
                    isCooldown = false;
                }
            }
        }
    }

    /*public void Render_Skill2_Cooldown()
    {
        if (isSkill2Pressed && !isCooldown2)
        {
            isCooldown2 = true;
            isSkill2Pressed = false;
            skill_image2.fillAmount = 1.0f;
        }

        if (isCooldown2)
        {
            skill_image2.fillAmount -= 1 / cooldown2 * Time.deltaTime;

            if (skill_image2.fillAmount <= 0.0f)
            {
                skill_image2.fillAmount = 1.0f;
                isCooldown2 = false;
            }

        }
    }

    public void Render_Skill3_Cooldown()
    {
        if (isSkill3Pressed && !isCooldown3)
        {
            isCooldown3 = true;
            isSkill3Pressed = false;
            skill_image3.fillAmount = 1.0f;
        }

        if (isCooldown3)
        {
            skill_image3.fillAmount -= 1 / cooldown3 * Time.deltaTime;

            if (skill_image3.fillAmount <= 0.0f)
            {
                skill_image3.fillAmount = 1.0f;
                isCooldown3 = false;
            }

        }
    }

    public void Render_Skill4_Cooldown()
    {
        if (isSkill4Pressed && !isCooldown4)
        {
            isCooldown4 = true;
            isSkill4Pressed = false;
            skill_image4.fillAmount = 1.0f;
        }

        if (isCooldown4)
        {
            skill_image4.fillAmount -= 1 / cooldown4 * Time.deltaTime;

            if (skill_image4.fillAmount <= 0.0f)
            {
                skill_image4.fillAmount = 1.0f;
                isCooldown4 = false;
            }
        }
    }*/
}

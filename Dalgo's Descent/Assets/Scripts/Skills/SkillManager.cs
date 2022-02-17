using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class SkillManager : MonoBehaviour
{
    public PlayerInput InputScript;
    public List<Image> skill;

    [Header("Skill 1")]
    public Image skill_image1;
    public float cooldown1 = 5.0f;
    public bool isCooldown = false;
    public bool isSkill1Pressed = false;

    [Header("Skill 2")]
    public Image skill_image2;
    public float cooldown2 = 5.0f;
    public bool isCooldown2 = false;
    public bool isSkill2Pressed = false;

    [Header("Skill 3")]
    public Image skill_image3;
    public float cooldown3 = 5.0f;
    public bool isCooldown3 = false;
    public bool isSkill3Pressed = false;

    [Header("Skill 4")]
    public Image skill_image4;
    public float cooldown4 = 5.0f;
    public bool isCooldown4 = false;
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
        skill_image1.fillAmount = 1.0f;
        skill_image2.fillAmount = 1.0f;
        skill_image3.fillAmount = 1.0f;
        skill_image4.fillAmount = 1.0f;
    }

    void Update()
    {
        Render_Skill1_Cooldown();
        Render_Skill2_Cooldown();
        Render_Skill3_Cooldown();
        Render_Skill4_Cooldown();
    }

    public delegate void SkillState(Skill newSkill);
    public event SkillState SkillChangeTo; //call this to change skill

    #region InputAction
    public void OnSkill1Used(InputAction.CallbackContext context)
    {
        if (context.action.activeControl.name == "q")
        {
            Debug.Log("eeee press");
            isSkill1Pressed = true;
        }
/*        for (int i = 0;i<skill.Count;i++)
        {
            if(context.action.activeControl.name == "q")
            {
                Debug.Log("eeee press");
                isSkill1Pressed = true;
            }
        }*/
/*        if (context.started)
        {
            Debug.Log("Q press");
            isSkill1Pressed = true;
        }*/

    }

    public void OnSkill2Used(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            Debug.Log("E press");
            isSkill2Pressed = true;

        }
    }

    public void OnSkill3Used(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            Debug.Log("R press");
            isSkill3Pressed = true;

        }
    }

    public void OnSkill4Used(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            Debug.Log("T press");
            isSkill4Pressed = true;

        }
    }
    #endregion
    public void Render_Skill1_Cooldown()
    {
        if (isSkill1Pressed && !isCooldown) //if skill 1 key is pressed and cooldown is 0
        {
            isCooldown = true; //reset these var
            isSkill1Pressed = false;
            
            skill_image1.fillAmount = 1.0f; //display the image that has cooldown
        }

        if (isCooldown) //while the skill is on cooldown
        {
            skill_image1.fillAmount -= 1 / cooldown1 * Time.deltaTime; //reduce the fill amount by 1 with respective to delta time

            if (skill_image1.fillAmount <= 0.0f) //if fill amount is lesser or equal to 0, make it to 0 and reset the cooldown to false
            {
                skill_image1.fillAmount = 1.0f;
                isCooldown = false;
            }
        }
    }

    public void Render_Skill2_Cooldown()
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
    }
}

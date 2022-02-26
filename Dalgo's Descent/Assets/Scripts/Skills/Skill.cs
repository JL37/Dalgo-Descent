using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Skill : TooltipTrigger
{
    public SkillObject SkillScriptable;
    private Image m_SkillImage;

    private void Start()
    {
        header = SkillScriptable.SkillName;
        body = SkillScriptable.SkillDescription;
        TriggerActive = true;
        m_SkillImage = GetComponent<Image>();
    }

    
    private void Update()
    {
        if (SkillScriptable.CurrentSkillPoints == 0)
            m_SkillImage.color = new Color(0.6f, 0.6f, 0.6f, 1);
        else m_SkillImage.color = Color.white;


        #if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.G))
        {
            SkillScriptable.CurrentSkillPoints++;
            Debug.Log(SkillScriptable.SkillName + ": " + SkillScriptable.CurrentSkillPoints + " points");
        }
        #endif
    }
}
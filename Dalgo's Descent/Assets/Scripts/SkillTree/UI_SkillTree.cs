using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_SkillTree : MonoBehaviour
{
    private PlayerSkills m_playerskills;
    private void Awake()
    {
        
    }
    public void Test()
    {
        Debug.Log("Click");
    }
    public void Skill1_Clicked()
    {
        m_playerskills.UnlockSkill(PlayerSkills.SkillType.Skill_1);
        Debug.Log("Skill 1 unlocked!");
    }

    public void Skill2_Clicked()
    {
        m_playerskills.UnlockSkill(PlayerSkills.SkillType.Skill_2);
        Debug.Log("Skill 2 unlocked!");
    }

    public void Skill3_Clicked()
    {
        m_playerskills.UnlockSkill(PlayerSkills.SkillType.Skill_3);
        Debug.Log("Skill 3 unlocked!");
    }

    public void Skill4_Clicked()
    {
        m_playerskills.UnlockSkill(PlayerSkills.SkillType.Skill_4);
        Debug.Log("Skill 4 unlocked!");
    }

    public void Health_Upgrade()
    {
        m_playerskills.UnlockSkill(PlayerSkills.SkillType.Health_Upgrade);
    }
    public void SetPlayerSkills(PlayerSkills playerskill)
    {
        this.m_playerskills = playerskill;
    }

    public PlayerSkills GetPlayerSkills()
    {
        return this.m_playerskills;
    }


}

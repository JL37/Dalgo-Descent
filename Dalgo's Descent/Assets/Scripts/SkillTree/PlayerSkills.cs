using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkills
{
    public event EventHandler<OnSkillUnlockedEventArgs> OnSkillUnlocked;
    public class OnSkillUnlockedEventArgs : EventArgs
    {
        public SkillType skilltype;
    }
    public enum SkillType
    {
        Skill_1 = 0, //Cleave
        Skill_2 = 1, //shove up
        Skill_3 = 2, //slam dunk
        Skill_4 = 3, //nothing yet idk
        Health_Upgrade = 4, //health upgrade to increase health stats
    }

    private List<SkillType> unlockedSkillTypeList;
  

    public PlayerSkills()
    {
        unlockedSkillTypeList = new List<SkillType>();
    }
    public void UnlockSkill(SkillType skillType)
    {
        if(!IsSkillUnlocked(skillType)) //if skill haven unlock 
        {
            unlockedSkillTypeList.Add(skillType);
            OnSkillUnlocked?.Invoke(this, new OnSkillUnlockedEventArgs { skilltype = skillType });
        }    

    }

    public void RemoveSkill(SkillType type)
    {
        if (type != SkillType.Health_Upgrade) //we want to remove health upgrade skill because we want to keep upgrading our health stats
            return;
        else
            unlockedSkillTypeList.Remove(type);

    }

    public bool IsSkillUnlocked(SkillType skillType)
    {
        return unlockedSkillTypeList.Contains(skillType); 
    }
}

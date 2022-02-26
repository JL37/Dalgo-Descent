using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class LevelSystem
{
    public event EventHandler OnExperienceChanged;
    public event EventHandler OnLevelChanged;

    
    private int m_level;
    private int m_experience;
    private int m_skillpoints;


    public LevelSystem()
    {
        //init var
        this.m_level = 0;
        this.m_experience = 0;
        this.m_skillpoints = 0;
    }

    public void AddExperience(int _amount)
    {
        m_experience += _amount;

        while (m_experience>=GetExperienceToNextLevel(m_level)) //if enough exp to lvl up
        {
            m_experience -= GetExperienceToNextLevel(m_level);
            m_level++;
            m_skillpoints++;
            if(OnLevelChanged != null)
                OnLevelChanged(this,EventArgs.Empty);
        }
        if(OnExperienceChanged != null)
            OnExperienceChanged(this, EventArgs.Empty);
    }

    public int GetCurrentLevel()
    {
        return this.m_level;
    }

    public float GetExperienceNormalized()
    {
        return (float)m_experience / GetExperienceToNextLevel(m_level);
    }

    public int GetExperience()
    {
        return m_experience;
    }

    public int GetExperienceToNextLevel(int level)
    {
        return level * 10;
    }

    public void setSkillpoints(int point)
    {
        this.m_skillpoints += point;
    }

    public int getSkillPoints()
    {
        return m_skillpoints;
    }
}

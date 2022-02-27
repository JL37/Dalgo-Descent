using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class LevelSystem
{
    public event EventHandler OnExperienceChanged;
    public event EventHandler OnLevelChanged;
 

    private int m_level;
    private float m_experience;
    private int m_skillpoints;

    public LevelSystem()
    {
        //init var
        this.m_level = 0;
        this.m_experience = 0.0f;
        this.m_skillpoints = 0;
    }

    public void AddExperience(float _amount)
    {
        m_experience += _amount;

        while (m_experience>=GetExperienceToNextLevel(m_level)) //if enough exp to lvl up
        {
            m_experience -= GetExperienceToNextLevel(m_level);
            m_level++;
            m_skillpoints++;
            PlayerStats playerstats = GameObject.FindObjectOfType<PlayerStats>();
            playerstats.m_Health.maxHealth = playerstats.m_Health.currentHealth = playerstats.m_Health.maxHealth + (100 * 0.5f * m_level);

            if (OnLevelChanged != null)
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

    public float GetExperience()
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

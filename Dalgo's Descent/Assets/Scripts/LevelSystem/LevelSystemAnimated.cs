using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSystemAnimated
{
    public event EventHandler OnExperienceChanged;
    public event EventHandler OnLevelChanged;

    private LevelSystem m_levelSystem;
    private bool bAnimating;
    private float m_updateTimer;
    private float m_updateTimerMax;

    private int m_level;
    private float m_experience;


    public LevelSystemAnimated(LevelSystem level_system)
    {
        SetLevelSystem(level_system);
        m_updateTimerMax = .022f; //CHANGE THIS VALUE TO SPEEED UP/ SLOW DOWN THE EXP PROGRESS BAR

        
    }

    public void SetLevelSystem(LevelSystem level_system)
    {
        this.m_levelSystem = level_system;

        this.m_level = m_levelSystem.GetCurrentLevel();
        this.m_experience = m_levelSystem.GetExperience();
    

        level_system.OnExperienceChanged += LevelSystem_OnExperienceChanged;
        level_system.OnLevelChanged += LevelSystem_OnLevelChanged;
    }

    private void LevelSystem_OnLevelChanged(object sender, EventArgs e)
    {
        bAnimating = true;
    }

    private void LevelSystem_OnExperienceChanged(object sender, EventArgs e)
    {
        bAnimating = true;
    }

    public void Update()
    {
        if(bAnimating)
        {
            m_updateTimer += Time.deltaTime;
            while (m_updateTimer > m_updateTimerMax) //making animation not depending on fps
            {
                m_updateTimer -= m_updateTimerMax;
                UpdateTypeAddExperience();
            }

        }
    }

    private void UpdateTypeAddExperience()
    {
        if (m_level < m_levelSystem.GetCurrentLevel()) //if local level is under the targetted level, you add exp
            AddExperience();
        else //if its same or more than the targetted level
        {
            if (m_experience < m_levelSystem.GetExperience())
                AddExperience();
            else
                bAnimating = false;
        }
    }

    private void AddExperience()
    {
        m_experience++;
        if (m_experience >= m_levelSystem.GetExperienceToNextLevel(m_level))
        {
            m_level++;
            m_experience = 0;
            if (OnLevelChanged != null)
                OnLevelChanged(this, EventArgs.Empty);
        }
        if (OnExperienceChanged != null)
            OnExperienceChanged(this, EventArgs.Empty);
    }

    public int GetCurrentLevel()
    {
        return this.m_level;
    }

    public float GetExperienceNormalized()
    {
        return (float)m_experience / m_levelSystem.GetExperienceToNextLevel(m_level);
    }

    public void SetCurrentLevel(int num)
    {
        this.m_level += num;
    }



}

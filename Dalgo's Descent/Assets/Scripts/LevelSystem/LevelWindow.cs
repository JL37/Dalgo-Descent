using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class LevelWindow : MonoBehaviour
{
    public Image m_experienceBarImage;
    public Text m_levelText;
    private LevelSystem m_levelSystem;
    private LevelSystemAnimated m_levelSystemAnimated;

    private void Awake()
    {
        m_levelText = transform.Find("level_text").GetComponent<Text>();
        m_experienceBarImage = transform.Find("lv_bar").Find("lv_fill").GetComponent<Image>();

    }

    private void Update()
    {
/*        LevelSystem levelSystem = new LevelSystem();
        Debug.Log(levelSystem.GetCurrentLevel());
        levelSystem.AddExperience(50);
        Debug.Log(levelSystem.GetCurrentLevel());
        levelSystem.AddExperience(60);
        Debug.Log(levelSystem.GetCurrentLevel());

        SetLevelSystem(levelSystem);*/
            
    }

    private void SetExperienceBarSize(float size)
    {
        m_experienceBarImage.fillAmount = size;
    }

    private void setLevelNum(int num)
    {
        m_levelText.text = "Level : " + (num + 1);
    }

    public void SetLevelSystem(LevelSystem levelSystem)
    {
        this.m_levelSystem = levelSystem;
    }
    public void SetLevelSystemAnimated(LevelSystemAnimated levelSystemAnimated)
    {
        this.m_levelSystemAnimated = levelSystemAnimated;

        //setLevelNum(levelSystemAnimated.GetCurrentLevel());
        SetExperienceBarSize(levelSystemAnimated.GetExperienceNormalized());

        levelSystemAnimated.OnExperienceChanged += LevelSystemAnimated_OnExperienceChanged;
        levelSystemAnimated.OnLevelChanged += LevelSystemAnimated_OnLevelChanged;
    }

    private void LevelSystemAnimated_OnLevelChanged(object sender, EventArgs e)
    {
        setLevelNum(m_levelSystemAnimated.GetCurrentLevel()); //after level has been changed, change the text
    }

    private void LevelSystemAnimated_OnExperienceChanged(object sender, EventArgs e)
    {
        SetExperienceBarSize(m_levelSystemAnimated.GetExperienceNormalized()); //exp lvl up, change the exp bar
    }
}

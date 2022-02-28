using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;

public class LevelWindow : MonoBehaviour
{
    public static LevelWindow Instance;

    public Image m_experienceBarImage;
    public TMP_Text m_levelText;
    public LevelSystem m_levelSystem;
    private LevelSystemAnimated m_levelSystemAnimated;

    private void Awake()
    {
        Instance = this;
/*        m_levelText = transform.Find("level_text").GetComponent<Text>();
        m_experienceBarImage = transform.Find("lv_bar").Find("lv_fill").GetComponent<Image>();*/

    }

    private void Update()
    {
        m_levelText.text = "Lv " + (m_levelSystem.GetCurrentLevel());
    }

    public LevelSystemAnimated getLevelSystemAnimated()
    {
        return this.m_levelSystemAnimated;
    }

    private void SetExperienceBarSize(float size)
    {
        m_experienceBarImage.fillAmount = size;
    }

    public void setLevelNum(int num)
    {
        // m_levelText.text = "Lv " + (num + 1);
    }

    public void setLevel(int num)
    {
        m_levelSystemAnimated.SetCurrentLevel(num);
    }

    public int getSkillpoints()
    {
        return this.m_levelSystem.getSkillPoints();
    }
    public void setSkillpoints(int num)
    {
        this.m_levelSystem.setSkillpoints(num);
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

    /*    private void LevelSystemAnimated_OnLevelChanged(object sender, EventArgs e)
        {
            setLevelNum(m_levelSystemAnimated.GetCurrentLevel()); //after level has been changed, change the text
        }*/
    private void LevelSystemAnimated_OnLevelChanged(object sender, EventArgs e)
    {
        setLevelNum(m_levelSystem.GetCurrentLevel()); //after level has been changed, change the text
    }

    private void LevelSystemAnimated_OnExperienceChanged(object sender, EventArgs e)
    {
        SetExperienceBarSize(m_levelSystemAnimated.GetExperienceNormalized()); //exp lvl up, change the exp bar
        m_levelText.text = "Lv " + m_levelSystem.GetCurrentLevel().ToString();
    }
}

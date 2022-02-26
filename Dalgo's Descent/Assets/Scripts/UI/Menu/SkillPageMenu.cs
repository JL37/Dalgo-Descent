using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillPageMenu : MenuBase
{
    public SkillObject CurrentSelectedSkillObject;
    public GameObject CurrentOpenSkillDesc;
    public SkillObject[] SkillObjectsList;
    public GameObject[] SkillPointsList;
    public GameObject[] UnlockedLinesList;
    public Button[] AddPointsButtons;
    public LevelWindow m_LevelWindow;
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < SkillPointsList.Length; i++)
        {
            SkillPointsList[i].SetActive(CurrentSelectedSkillObject.CurrentSkillPoints - 1 == i ? true : false);
        }

        for (int i = 0; i < AddPointsButtons.Length; i++)
        {
            if (SkillObjectsList[i].CurrentSkillPoints < 4 && m_LevelWindow.getSkillpoints() - 1 >0)
                AddPointsButtons[i].interactable = true;
            else AddPointsButtons[i].interactable = false;
        }

        for (int i = 0; i < UnlockedLinesList.Length; i++)
        {
            if (SkillObjectsList[i].CurrentSkillPoints != 0)
                UnlockedLinesList[i].SetActive(true);
            else UnlockedLinesList[i].SetActive(false);
        }
    }

    public void ChangeCurrentSkillDescription(GameObject nextSkillDesc)
    {
        CurrentOpenSkillDesc.SetActive(false);
        CurrentOpenSkillDesc = nextSkillDesc;
        CurrentOpenSkillDesc.SetActive(true);
    }

    public void ChangeCurrentSkillObject(Skill nextSkillObject)
    {
        CurrentSelectedSkillObject = nextSkillObject.SkillScriptable;
    }

    public void AddPointsToSkill(Skill skillToAdd) 
    {
        skillToAdd.SkillScriptable.CurrentSkillPoints++;
       
        // MINUS FROM OWNED SKILL POINTS
        m_LevelWindow.setSkillpoints(-1);
        m_LevelWindow.setLevelNum(m_LevelWindow.getSkillpoints());
    }
}

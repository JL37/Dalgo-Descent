using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillPageMenu : MenuBase
{
    public SkillObject CurrentSelectedSkillObject;
    public GameObject CurrentOpenSkillDesc;
    public GameObject[] SkillPointsList;
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
}

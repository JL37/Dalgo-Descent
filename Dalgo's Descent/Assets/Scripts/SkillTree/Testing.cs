using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Testing : MonoBehaviour
{
    [SerializeField] private PlayerStats playerStat;
    public UI_SkillTree skill1;
    public UI_SkillTree skill2;
    public UI_SkillTree skill3;
    public UI_SkillTree skill4;
    public UI_SkillTree healthUpgrade;
    private void Start()
    {
        skill1.SetPlayerSkills(playerStat.GetPlayerSkills());
        skill2.SetPlayerSkills(playerStat.GetPlayerSkills());
        skill3.SetPlayerSkills(playerStat.GetPlayerSkills());
        skill4.SetPlayerSkills(playerStat.GetPlayerSkills());
        healthUpgrade.SetPlayerSkills(playerStat.GetPlayerSkills());

    }

    private void Update()
    {
/*        if(playerStat.CanUseSkill1())
        {
            Debug.Log("Can use skill 1");
        }
        if (playerStat.CanUseSkill2())
        {
            Debug.Log("Can use skill 2");
        }
        if (playerStat.CanUseSkill3())
        {
            Debug.Log("Can use skill 3");
        }
        if (playerStat.CanUseSkill4())
        {
            Debug.Log("Can use skill 4");
        }*/
    }
}

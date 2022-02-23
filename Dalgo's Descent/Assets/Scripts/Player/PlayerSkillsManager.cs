using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerSkillsManager : MonoBehaviour
{
    public Animator PlayerAnimator;
    public List<SkillObject> Skills;
    public UI_SkillTree Cleave;
    public UI_SkillTree ShovelCut;
    public UI_SkillTree Dunk;



    public int ActiveSkillIndex { get; private set; }

    private int SkillLayerIndex;
    private double SkillAnimationTimer;
    // Start is called before the first frame update
    void Start()
    {
        ActiveSkillIndex = -1;
        SkillLayerIndex = PlayerAnimator.GetLayerIndex("Skill Layer");
        
    }

    // Update is called once per frame
    void Update()
    {
        if (ActiveSkillIndex >= 0)
            if (SkillAnimationTimer <= Skills[ActiveSkillIndex].SkillAnimation.length)
                SkillAnimationTimer += Time.deltaTime;
            else
                SkillFinish();
        else
            SkillFinish();

    }

    public void OnCleavePressed(InputAction.CallbackContext context)
    {
        if(context.started)
        {
            if(Cleave.GetPlayerSkills().IsSkillUnlocked(PlayerSkills.SkillType.Skill_1)) //check if skill has been unlock already
            {
                Debug.Log("USING SKILL 1 LIAO");
                UseSkill(0);

            }
        }
    }

    public void OnShovelCutPressed(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            if (ShovelCut.GetPlayerSkills().IsSkillUnlocked(PlayerSkills.SkillType.Skill_2))
            {
                Debug.Log("USING SKILL 2 LIAO");

                UseSkill(1);

            }
        }
    }
    public void OnSlamDunkPressed(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            if (Dunk.GetPlayerSkills().IsSkillUnlocked(PlayerSkills.SkillType.Skill_3))
            {
                Debug.Log("USING SKILL 3 LIAO");

                UseSkill(2);

            }
        }
    }

    private void SkillFinish()
    {
        SkillAnimationTimer = 0;
        ActiveSkillIndex = -1;
        PlayerAnimator.SetLayerWeight(SkillLayerIndex, 0);
    }
    private void UseSkill(int index)
    {
        SkillAnimationTimer = 0;
        GetComponent<PlayerAttackManager>().ResetState();
        ActiveSkillIndex = index;
        PlayerAnimator.Play(Skills[index].SkillAnimation.name, SkillLayerIndex);
        PlayerAnimator.SetLayerWeight(SkillLayerIndex, 1);
    }
}

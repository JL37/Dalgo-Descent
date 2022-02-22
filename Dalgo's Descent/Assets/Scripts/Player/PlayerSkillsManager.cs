using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerSkillsManager : MonoBehaviour
{
    public Animator PlayerAnimator;
    public List<SkillObject> Skills;

    public int ActiveSkillIndex { get; private set; }

    public List<int> SkillTriggers { get; private set; }
    private int SkillLayerIndex;
    private double SkillAnimationTimer;
    // Start is called before the first frame update
    void Start()
    {
        SkillTriggers = new List<int>();
        ActiveSkillIndex = -1;
        SkillLayerIndex = PlayerAnimator.GetLayerIndex("Skill Layer");
        for (int i = 0; i < Skills.Count; i++)
        {
            SkillTriggers.Add(Animator.StringToHash(Skills[i].AnimationTrigger));
        }
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

    public void Cleave()
    {
        UseSkill(0);
    }
    
    public void OnCleavePressed(InputAction.CallbackContext context)
    {
        if(context.started)
        {
            Cleave();
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
        GetComponent<PlayerAttackManager>().ResetState();
        ActiveSkillIndex = index;
        PlayerAnimator.SetTrigger(SkillTriggers[index]);
        PlayerAnimator.SetLayerWeight(SkillLayerIndex, 1);
    }
}

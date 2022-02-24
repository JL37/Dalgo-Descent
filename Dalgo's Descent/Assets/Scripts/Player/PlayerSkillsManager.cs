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

    private PlayerStats m_playerStats;

    [Header("Skill Prefabs")]
    [SerializeField] GameObject CleaveVFXPrefab;
    [SerializeField] GameObject ShovelCutVFXPrefab;
    [SerializeField] GameObject SlamDunkVFXPrefab;

    public int ActiveSkillIndex { get; private set; }

    private int SkillLayerIndex;
    private double SkillAnimationTimer;
    // Start is called before the first frame update
    void Start()
    {
        ActiveSkillIndex = -1;
        SkillLayerIndex = PlayerAnimator.GetLayerIndex("Skill Layer");
        m_playerStats = GetComponent<PlayerStats>();
        
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
            UseSkill(0);
            if(Cleave.GetPlayerSkills().IsSkillUnlocked(PlayerSkills.SkillType.Skill_1)) //check if skill has been unlock already
            {
                Debug.Log("USING SKILL 1 LIAO");

            }
        }
    }

    public void OnShovelCutPressed(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            if (ShovelCut.GetPlayerSkills().IsSkillUnlocked(PlayerSkills.SkillType.Skill_2))
            {
                UseSkill(1);
                Debug.Log("USING SKILL 2 LIAO");
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

    #region SkillEvents
     public void CleaveEvent()
    {
        Vector3 Position = transform.position + transform.forward;
        Quaternion rotation = transform.rotation;
        Instantiate(CleaveVFXPrefab, Position, rotation);
        print(rotation);

        Collider[] colliders = Physics.OverlapSphere(Position, 3f);
        foreach (Collider c in colliders)
        {
            if (c.gameObject.tag == "AI")
            {
                AI ai = c.gameObject.GetComponent<AI>();
                if (ai.aiType == AI.AI_TYPE.AI_TYPE_ENEMY)
                {
                    ((AIUnit)ai).EnemyPushBack((int)(m_playerStats.BaseBasicAtk * m_playerStats.SkillDmg));
                }
                if (ai.aiType == AI.AI_TYPE.AI_TYPE_BOSS)
                {
                    ((BossAI)ai).Damage((int)(m_playerStats.BaseBasicAtk * m_playerStats.SkillDmg));
                }
            }
        }
    }

    public void ShovelCutEvent()
    {
        Vector3 instantiationPosition = transform.position + transform.forward * 1.2f;
        Instantiate(ShovelCutVFXPrefab, instantiationPosition, Quaternion.identity);

        Collider[] colliders = Physics.OverlapSphere(instantiationPosition, 3f);
        foreach (Collider c in colliders)
        {
            if (c.gameObject.tag == "AI")
            {
                AI ai = c.gameObject.GetComponent<AI>();
                if (ai.aiType == AI.AI_TYPE.AI_TYPE_ENEMY)
                {
                    ((AIUnit)ai).EnemyKnockup((int)(m_playerStats.BaseBasicAtk * m_playerStats.SkillDmg));
                }
                if (ai.aiType == AI.AI_TYPE.AI_TYPE_BOSS)
                {
                    ((BossAI)ai).Damage((int)(m_playerStats.BaseBasicAtk * m_playerStats.SkillDmg));
                }
            }
        }
    }
    #endregion

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

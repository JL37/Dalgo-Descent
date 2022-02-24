using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerSkillsManager : MonoBehaviour
{
    [Header("Objects")]
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

    [Header("Adjustable variables")]
    [SerializeField] Vector3 m_TargetedGravity;

    public int ActiveSkillIndex { get; private set; }


    private int SkillLayerIndex;
    private double SkillAnimationTimer;
    private Vector3 m_OriginalGravity;

    //References 
    protected AnimationController m_AController;
    protected PlayerController m_PlayerController;

    // Start is called before the first frame update
    void Start()
    {
        ActiveSkillIndex = -1;
        SkillLayerIndex = PlayerAnimator.GetLayerIndex("Skill Layer");
        m_playerStats = GetComponent<PlayerStats>();
        m_AController = GetComponent<AnimationController>();
        m_PlayerController = GetComponent<PlayerController>();
        m_OriginalGravity = m_PlayerController.GetGravity();
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
            UseSkill(1);
            if (ShovelCut.GetPlayerSkills().IsSkillUnlocked(PlayerSkills.SkillType.Skill_2))
            {
                Debug.Log("USING SKILL 2 LIAO");
            }
        }
    } 

    public void OnSlamDunkPressed(InputAction.CallbackContext context)
    {
        PlayerController playerController = GetComponent<PlayerController>();

        if (context.started && !playerController.IsGrounded)
        {
            UseSkill(2);

            //Make him go up first
            playerController.ResetImpactForJump();
            playerController.AddImpact(Vector3.up, 15f);

            //if (Dunk.GetPlayerSkills().IsSkillUnlocked(PlayerSkills.SkillType.Skill_3))
            //{
            //    Debug.Log("USING SKILL 3 LIAO");
            //    UseSkill(2);
            //}
        }
    }

    #region SkillEvents
    public void CleaveEvent()
    {
        Vector3 Position = new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z);
        Instantiate(CleaveVFXPrefab, transform);
        // print(rotation);
    }

    public void ShovelCutEvent()
    {
        Vector3 instantiationPosition = transform.position + transform.forward * 1.2f;
        Instantiate(ShovelCutVFXPrefab, instantiationPosition, Quaternion.identity);
    }

    public void SlamDunkInAirEvent()
    {
        m_AController.PauseAnimation();

        StartCoroutine(ResumeAnimationWhenOnGround());
        StartCoroutine(ChangeGravity());
    }

    public void SlamDunkGroundedEvent()
    {
        Vector3 instantiationPosition = transform.position + transform.forward * 1.2f;
        GameObject particle = Instantiate(SlamDunkVFXPrefab, instantiationPosition, Quaternion.identity);
        particle.transform.localScale = new Vector3(1.5f, 1.5f, 2);

        float distLimit = 5f; //Distancing from player to enemies
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("AI");
        foreach (GameObject enemy in enemies)
        {
            if (!enemy.activeSelf)
                continue;

            float dist = (enemy.transform.position - gameObject.transform.position).magnitude;
            if (dist < distLimit)
            {
                AIUnit normalEnemy = enemy.GetComponent<AIUnit>();
                BossAI bossEnemy = enemy.GetComponent<BossAI>();

                if (normalEnemy)
                    normalEnemy.EnemyHit(10, normalEnemy.isMiniboss ? 0 : 500f);
                else if (bossEnemy)
                    bossEnemy.Damage(10);
            }
        }
    }
    #endregion

    protected IEnumerator ChangeGravity()
    {
        while (!m_PlayerController.IsLanding)
            yield return null;

        m_PlayerController.SetGravity(m_TargetedGravity);
    }

    protected IEnumerator ResumeAnimationWhenOnGround()
    {
        //Wait until player is on ground, then continue animation!
        while (!m_PlayerController.IsGrounded)
        {
            yield return new WaitForEndOfFrame();
        }

        m_PlayerController.SetGravity(m_OriginalGravity);
        m_AController.ResumeAnimation();
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

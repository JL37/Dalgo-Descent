using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttackManager : MonoBehaviour
{
    public Animator PlayerAnimator;

    [Header("Attack Modifier")]
    [Range(0, 1)] [SerializeField] private double AttackCoolDown; //Start Recording after this period
    [Range(0, 1)] [SerializeField] private double AttackInputDelay; //Start Recording after this period
    [SerializeField] private List<AnimationClip> Combos;
    [SerializeField] private List<GameObject> SlashVFXPrefabs;

    [Header("Debug")]
    public int CurrentCombo;
    public int CurrentSlash;
    public double AttackCDTimer = 0;
    public double AttackInputTimer = 0;
    public bool IsAttacking;
    public bool AttackTriggered;
    public bool HasAttackInput;

    public int IsAttackHash { get; private set; }
    public int AttackTriggerHash { get; private set; }

    void Start()
    {
        IsAttackHash = Animator.StringToHash("IsAttack");
        AttackTriggerHash = Animator.StringToHash("AttackTrigger");
    }

    // Update is called once per frame
    void Update()
    {
        if (AttackCDTimer > 0)
            AttackCDTimer -= Time.deltaTime;

        if (IsAttacking)
        {
            AttackInputTimer += Time.deltaTime;

            if (CurrentCombo <= Combos.Count)
            {
                if (AttackInputTimer >= AttackInputDelay) // Wait 0.1s after Start of Attack
                {
                    if (AttackInputTimer < Combos[CurrentCombo - 1].length)
                    {
                        if (HasAttackInput && !AttackTriggered)
                        {
                            HasAttackInput = false;
                            if (CurrentCombo != Combos.Count)
                            {
                                AttackTriggered = true;
                                PlayerAnimator.SetTrigger(AttackTriggerHash);
                            }
                        }
                    }
                    else
                    {
                        AttackInputTimer = 0;
                        if (AttackTriggered)
                        {
                            CurrentCombo++;
                            AttackTriggered = false;
                        }
                        else
                            ResetState();
                    }
                }
            }
            else
            {
                IsAttacking = false;
                AttackTriggered = false;
            }
        }
        else
        {
            if (HasAttackInput && AttackCDTimer <= 0 && !AttackTriggered)
            {
                HasAttackInput = false;
                IsAttacking = true;
                CurrentCombo++;
                PlayerAnimator.SetTrigger(AttackTriggerHash);
            }
            else
                ResetState(true);
        }

        PlayerAnimator.SetBool(IsAttackHash, IsAttacking);
    }

    public void ResetState(bool isFirst = false)
    {
        PlayerAnimator.ResetTrigger(AttackTriggerHash);
        AttackInputTimer = 0;
        CurrentCombo = 0;
        CurrentSlash = 0;
        IsAttacking = false;
        AttackTriggered = false;
        HasAttackInput = false;

        if(!isFirst)
            AttackCDTimer = AttackCoolDown;
    }

    public void Slash() // Called by Animation
    {
        var slash = Instantiate(SlashVFXPrefabs[CurrentSlash], transform);
        //slash.transform.position += transform.position;
        //slash.transform.rotation = transform.rotation * slash.transform.rotation;
        CurrentSlash++;
    }

    public void Attack(InputAction.CallbackContext context)
    {
        //if (Cursor.visible)
        //    return;

        if (context.started && AttackCDTimer <= 0 && GetComponent<PlayerSkillsManager>().ActiveSkillIndex < 0)
            HasAttackInput = true;
    }
}

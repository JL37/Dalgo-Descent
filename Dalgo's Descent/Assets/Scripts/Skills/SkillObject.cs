using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Skill", menuName = "ScriptableObjects/SkillObject")]
public class SkillObject : ScriptableObject
{
    #region scriptable_object_variables
    [SerializeField] string m_SkillName;
    [SerializeField] string m_SkillDescription;

    [SerializeField] Sprite m_SkillSprite;
    [SerializeField] AnimationClip m_SkillAnimation;
    [SerializeField] float m_SkillCooldown;
    [SerializeField] float m_MaxSkillCooldown;
    [SerializeField] bool m_Unlocked = false;
    [SerializeField] int m_MaxSkillPoints;
    [SerializeField] InputActionReference m_InputActionReference;
    #endregion

    #region instance_variables
    private string _skillName;
    private string _skillDescription;

    private Sprite _skillSprite;
    private AnimationClip _skillAnimation;

    private float _originalSkillCooldown;
    private float _skillCooldown;
    private float _finalSkillCooldown;
    private float _skillCooldownTimer;

    private bool _unlocked;
    private int _skillPoints;
    private int _maxSkillPoints;

    private InputActionReference _inputActionReference;
    
    public string SkillName { get { return _skillName;} }
    public string SkillDescription { get { return _skillDescription;} }
    public Sprite SkillSprite { get { return _skillSprite;} }
    public AnimationClip SkillAnimation { get { return _skillAnimation; } }
    public float SkillCooldown { get { return _skillCooldown;} }
    public float SkillCooldownTimer { get { return _skillCooldownTimer; } set { _skillCooldownTimer = value; } }
    public bool Unlocked { get { return _unlocked; } set { _unlocked = value; } }
    public int CurrentSkillPoints { get { return _skillPoints; } set { if (value > 0) _skillPoints = value; } }
    public int MaxSkillPoints { get { return _maxSkillPoints; } }
    public InputActionReference InputActionReference { get { return _inputActionReference; } }
    #endregion

    private void OnEnable()
    {
        _skillName = m_SkillName;
        _skillDescription = m_SkillDescription;
        _skillSprite = m_SkillSprite;
        _skillAnimation = m_SkillAnimation;
        _skillCooldown = m_SkillCooldown;
        _originalSkillCooldown = m_SkillCooldown;
        _finalSkillCooldown = m_MaxSkillCooldown;
        _skillCooldownTimer = 0f;
        _unlocked = m_Unlocked;
        _skillPoints = 0; 
        _maxSkillPoints = m_MaxSkillPoints;
        _inputActionReference = m_InputActionReference;
    }

    public void RebindKey(string keycode)
    {
        SettingsMenu.OverrideBinding(keycode, _inputActionReference);
    }

    public bool UpgradeSkill()
    {
        if (CurrentSkillPoints >= MaxSkillPoints)
            return false;

        CurrentSkillPoints++;

        if (m_MaxSkillPoints == 1)
            return true;

        _skillCooldown = _originalSkillCooldown - ((CurrentSkillPoints - 1) * ((_originalSkillCooldown - _finalSkillCooldown) / (_maxSkillPoints - 1)));

        return true;
    }
}

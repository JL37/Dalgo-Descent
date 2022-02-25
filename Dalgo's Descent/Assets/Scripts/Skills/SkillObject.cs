using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Skill", menuName = "ScriptableObjects/SkillObject")]
public class SkillObject : ScriptableObject
{
    [SerializeField] string m_Name;
    [SerializeField] Sprite m_SkillSprite;
    [SerializeField] AnimationClip m_SkillAnimation;
    [SerializeField] float m_SkillCooldown;
    [SerializeField] bool m_Unlocked = false;

    private string _name;
    private Sprite _skillSprite;
    private AnimationClip _skillAnimation;
    private float _skillCooldown;
    private float _skillCooldownTimer;
    private bool _unlocked;

    public string Name { get { return _name;} }
    public Sprite SkillSprite { get { return _skillSprite;} }
    public AnimationClip SkillAnimation { get { return _skillAnimation; } }
    public float SkillCooldown { get { return _skillCooldown;} }
    public float SkillCooldownTimer { get { return _skillCooldownTimer; } set { _skillCooldownTimer = value; } }
    public bool Unlocked { get { return _unlocked; } set { _unlocked = value; } }

    private void OnEnable()
    {
        _name = m_Name;
        _skillSprite = m_SkillSprite;
        _skillAnimation = m_SkillAnimation;
        _skillCooldown = m_SkillCooldown;
        _skillCooldownTimer = 0f;
        _unlocked = m_Unlocked;
    }

}

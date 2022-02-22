using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Skill", menuName = "ScriptableObjects/SkillObject")]
public class SkillObject : ScriptableObject
{
    public string Name;
    public Sprite SkillSprite;
    public AnimationClip SkillAnimation;
    public string AnimationTrigger;
}

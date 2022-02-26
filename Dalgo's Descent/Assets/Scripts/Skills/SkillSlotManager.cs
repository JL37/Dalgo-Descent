using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillSlotManager : MonoBehaviour
{
    public SkillSlot[] skillSlots;
    public static SkillSlotManager Instance;  

    private void Awake()
    {
        if (Instance)
            Destroy(Instance.gameObject);

        Instance = this;
    }

    private void Start()
    {
        skillSlots = GetComponentsInChildren<SkillSlot>();
    }

    public void UpdateSkillSlots(SkillSlot updatedSkillSlot)
    {
        for (int i = 0; i < skillSlots.Length; i++)
        {
            if (skillSlots[i] == updatedSkillSlot || skillSlots[i].AnchoredSkill == null) continue;

            if (skillSlots[i].AnchoredSkill.name == updatedSkillSlot.AnchoredSkill.name)
            {
                skillSlots[i].ResetSkill();

            }
        }
    }
}

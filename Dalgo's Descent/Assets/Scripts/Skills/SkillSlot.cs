using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SkillSlot : TooltipTrigger, IDropHandler
{
    // public DragDrop dragdrop;
    public SkillObject AnchoredSkill;
    public string keycode;
    public Sprite baseImage;
    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log("OnDrop");
        // Debug.Log(dragdrop.getCurrentRect());

        if(eventData.pointerDrag != null) //if you are dragging a game object
        {   
            AnchoredSkill = eventData.pointerDrag.GetComponent<Skill>().SkillScriptable;

            // Rebind Skill
            string keyCodeStore;
            keyCodeStore = (keycode == "LMB" || keycode == "RMB") ? "<Mouse>/" + keycode : "<Keyboard>/" + keycode;
            AnchoredSkill.RebindKey(keyCodeStore);
            Debug.Log("Rebinded \"" + AnchoredSkill.SkillName + "\" to " + keycode);

            // Update tooltip info
            header = AnchoredSkill.SkillName + " [" + keycode + "]";
            body = AnchoredSkill.SkillDescription;

            // update skillslot image
            GetComponent<Image>().sprite = eventData.pointerDrag.GetComponent<Image>().sprite;

            SkillSlotManager.Instance.UpdateSkillSlots(this);
        }
    }

    public void ResetSkill()
    {
        GetComponent<Image>().sprite = baseImage;
        AnchoredSkill = null;
    }

    void Update()
    {
        TriggerActive = (AnchoredSkill != null);
    }

}

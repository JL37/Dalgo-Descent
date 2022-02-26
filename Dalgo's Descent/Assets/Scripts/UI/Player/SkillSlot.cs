using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SkillSlot : TooltipTrigger, IDropHandler
{
    // public DragDrop dragdrop;
    public SkillObject AnchoredSkill;
    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log("OnDrop");
        // Debug.Log(dragdrop.getCurrentRect());

        if(eventData.pointerDrag != null) //if you are dragging a game object
        {
            // GameObject newSkillObj = Instantiate(eventData.pointerDrag.gameObject, transform.parent);
            // newSkillObj.GetComponent<RectTransform>().anchoredPosition = GetComponent<RectTransform>().anchoredPosition;
            // eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition = eventData.pointerDrag.GetComponent<DragDrop>().PreDragPosition;
            AnchoredSkill = eventData.pointerDrag.GetComponent<Skill>().SkillScriptable;
            header = AnchoredSkill.SkillName;
            body = AnchoredSkill.SkillDescription;
            GetComponent<Image>().sprite = eventData.pointerDrag.GetComponent<Image>().sprite;
        }
    }

    void Update()
    {
        TriggerActive = (AnchoredSkill != null);
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SkillSlot : MonoBehaviour,IDropHandler
{
    public DragDrop dragdrop;
    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log("OnDrop");
        Debug.Log(dragdrop.getCurrentRect());

        if(eventData.pointerDrag != null) //if you are dragging a game object
        {
            eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition = GetComponent<RectTransform>().anchoredPosition;
        }
    }


}

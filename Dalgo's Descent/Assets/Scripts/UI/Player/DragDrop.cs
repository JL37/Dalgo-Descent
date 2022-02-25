using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Skill))]
public class DragDrop : MonoBehaviour, IPointerDownHandler,IBeginDragHandler,IEndDragHandler,IDragHandler, IInitializePotentialDragHandler
{
    [SerializeField] private Canvas canvas;

    private Skill skill;
    private RectTransform m_RectTransform;
    public Vector2 PreDragPosition;
    private CanvasGroup m_canvasGroup;
    private void Awake()
    {
        skill = GetComponent<Skill>();
        canvas = GameObject.FindGameObjectWithTag("HUD").GetComponent<Canvas>();
        m_RectTransform = GetComponent<RectTransform>();
        m_canvasGroup = GetComponent<CanvasGroup>();
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (skill.SkillScriptable.CurrentSkillPoints == 0)
            return;

        Debug.Log("OnBeginDrag");
        PreDragPosition = m_RectTransform.anchoredPosition;
        m_canvasGroup.alpha = .6f;
        m_canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (skill.SkillScriptable.CurrentSkillPoints == 0)
            return;

        m_RectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor; //this object will follow the mouse cursor
        
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (skill.SkillScriptable.CurrentSkillPoints == 0)
            return;

        Debug.Log("OnEndDrag");
        m_canvasGroup.alpha = 1.0f;
        m_canvasGroup.blocksRaycasts = true;
        m_RectTransform.anchoredPosition = PreDragPosition;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("On pointer down");
    }

    public void OnInitializePotentialDrag(PointerEventData eventData)
    {
        eventData.useDragThreshold = false; //double make sure object move exactly where i go 
    }

    public RectTransform getCurrentRect()
    {
        return this.m_RectTransform;
    }
}

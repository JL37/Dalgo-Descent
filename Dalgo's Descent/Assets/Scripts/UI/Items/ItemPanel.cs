using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemPanel : MonoBehaviour
{
    protected RectTransform m_RectTransform;
    protected Vector2 m_TargetPos;

    protected Vector2 m_OriginalPos;
    [Header("Offsets")]
    [SerializeField] Vector2 m_CursorPos;
    [SerializeField] Vector2 m_HoveredPos;

    [Header("Speed and stuff")]
    [SerializeField] float m_LerpSpd = 0.3f;

    protected bool m_IsHovered = false;

    // Start is called before the first frame update
    void Start()
    {
        m_RectTransform = GetComponent<RectTransform>();

        m_OriginalPos = m_RectTransform.anchoredPosition;
        m_CursorPos += m_OriginalPos;
        m_HoveredPos += m_OriginalPos;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameStateManager.Get_Instance.CurrentGameState == GameState.Paused) //Ignore key presses when paused
            return;

        UpdateHover();
        UpdateTargetPos();
        AnimatePos();
    }

    protected void UpdateHover()
    {
        Vector2 topLeftPos = transform.position;
        topLeftPos.x -= m_RectTransform.sizeDelta.x / 2 * transform.lossyScale.x;

        Vector2 botRightPos = transform.position;
        botRightPos.x += m_RectTransform.sizeDelta.x / 2 * transform.lossyScale.x;
        botRightPos.y -= m_RectTransform.sizeDelta.y * transform.lossyScale.y;

        //Check if within range
        bool inRangeX = Input.mousePosition.x >= topLeftPos.x && Input.mousePosition.x <= botRightPos.x;
        bool inRangeY = Input.mousePosition.y >= botRightPos.y && Input.mousePosition.y <= topLeftPos.y;

        m_IsHovered = inRangeX && inRangeY;
    }

    protected void AnimatePos()
    {
        Vector2 pos = m_RectTransform.anchoredPosition;

        pos.x = Mathf.Lerp(pos.x, m_TargetPos.x, m_LerpSpd);
        pos.y = Mathf.Lerp(pos.y, m_TargetPos.y, m_LerpSpd);

        m_RectTransform.anchoredPosition = pos;
    }

    protected void UpdateTargetPos()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            return;

        if (!Cursor.visible)
            m_TargetPos = m_OriginalPos;
        else if (m_IsHovered)
            m_TargetPos = m_HoveredPos;
        else
            m_TargetPos = m_CursorPos;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemUI : MonoBehaviour
{
    protected enum Animation
    {
        EXPAND,
        SHRINK,
        NORMAL
    }
    Animation m_CurrAnim = Animation.EXPAND;

    [SerializeField] ItemDescBoxUI m_DescBox;

    protected RawImage m_Image;
    protected RectTransform m_RectTransform;
    protected Item m_Item;

    protected int m_Idx = 0;
    protected float m_Size = 50;
    protected Vector2 m_TargetPos;

    //Things to adjust during initialisation phrase
    protected float m_XOffSet = 15;
    protected float m_YOffSet = 20;
    protected float m_TotalWidth = 576;
    protected float m_TotalHeight = 260;
    protected float m_NumPerRow = 8;
    protected float m_NumPerCol = 3;

    //Hovering stuff
    protected bool m_IsHovered = false;

    // Start is called before the first frame update
    void Start()
    {
        m_Image = GetComponent<RawImage>();
        m_Image.texture = m_Item.GetCurrTexture();
        m_RectTransform = m_Image.GetComponent<RectTransform>();
        m_Size = m_Image.GetComponent<RectTransform>().sizeDelta.x;

        if (m_CurrAnim == Animation.EXPAND)
            m_Image.GetComponent<RectTransform>().sizeDelta = new Vector2(0, 0);

        //Update description box
        m_DescBox.Initialise(m_Item);

        //Set position
        UpdatePositionFromIndex();
        m_RectTransform.anchoredPosition = m_TargetPos;
    }

    public void UpdatePositionFromIndex()
    {
        //Initialisation of variables
        float beginX = m_Size / 2 + m_XOffSet;
        float endX = m_TotalWidth - (m_Size / 2) - m_XOffSet;
        float incX = (endX - beginX) / (m_NumPerRow - 1);

        float beginY = -m_Size / 2 - m_YOffSet;
        float endY = -m_TotalHeight + (m_Size / 2) + m_YOffSet;
        float incY = (endY - beginY) / (m_NumPerCol - 1);

        int idxX = m_Idx % (int)m_NumPerRow;
        int idxY = m_Idx / (int)m_NumPerRow;

        //Calculate position
        float x = beginX + idxX * incX;
        float y = beginY + idxY * incY;

        //Setting anchor first
        m_TargetPos = new Vector2(x, y);
        transform.localScale = new Vector2(1, 1);
    }

    // Update is called once per frame
    void Update()
    {
        if (GameStateManager.Get_Instance.CurrentGameState == GameState.Paused)
        {
            m_IsHovered = false;
            return;
        }

        UpdateHover();
        UpdateAnimation();
        UpdatePosition();
        UpdateDescBox();
    }

    protected void UpdateDescBox()
    {
        if (m_IsHovered && !m_DescBox.gameObject.activeSelf)
        {
            transform.SetAsLastSibling();
            m_DescBox.gameObject.SetActive(true);
        }
        else if (!m_IsHovered)
        {
            if (m_DescBox.gameObject.activeSelf)
                m_DescBox.gameObject.SetActive(false);

            return;
        }

        Vector3 updatePos = Input.mousePosition;
        updatePos.z = -10; //depth
        m_DescBox.gameObject.transform.position = updatePos;

       //Reverse positioning if the info is too out of bounds
       RectTransform descBoxRect = m_DescBox.GetComponent<RectTransform>();

        Vector2 rightPos = m_DescBox.gameObject.transform.position;
        rightPos.x += descBoxRect.sizeDelta.x * descBoxRect.lossyScale.x;

        if (rightPos.x > Screen.width)
            descBoxRect.pivot = new Vector2(1, 0);
        else
            descBoxRect.pivot = new Vector2(0, 0);
    }

    protected void UpdateHover()
    {
        Vector2 topLeftPos = transform.position;
        topLeftPos.x -= m_RectTransform.sizeDelta.x / 2 * transform.lossyScale.x;
        topLeftPos.y += m_RectTransform.sizeDelta.y / 2 * transform.lossyScale.y;

        Vector2 botRightPos = transform.position;
        botRightPos.x += m_RectTransform.sizeDelta.x / 2 * transform.lossyScale.x;
        botRightPos.y -= m_RectTransform.sizeDelta.y / 2 * transform.lossyScale.y;

        //Check if within range
        bool inRangeX = Input.mousePosition.x >= topLeftPos.x && Input.mousePosition.x <= botRightPos.x;
        bool inRangeY = Input.mousePosition.y >= botRightPos.y && Input.mousePosition.y <= topLeftPos.y;

        m_IsHovered = inRangeX && inRangeY;
    }

    protected void UpdatePosition() //Animate position
    {
        float spd = 0.35f;
        Vector2 pos = m_RectTransform.anchoredPosition;

        pos.x = Mathf.Lerp(pos.y != m_TargetPos.y ? m_TargetPos.x : pos.x, m_TargetPos.x, spd);
        pos.y = m_TargetPos.y;

        //New position
        m_RectTransform.anchoredPosition = pos;
    }

    protected void UpdateAnimation()
    {
        Vector2 currSize = m_Image.GetComponent<RectTransform>().sizeDelta;
        float lerpSpd = 0.2f;

        switch (m_CurrAnim)
        {
            case Animation.NORMAL:
                break; //Do nothing

            case Animation.EXPAND:

                currSize.x = Mathf.Lerp(currSize.x, m_Size, lerpSpd);
                currSize.y = currSize.x;

                m_Image.GetComponent<RectTransform>().sizeDelta = currSize;

                break;

            case Animation.SHRINK:
                currSize.x = Mathf.Lerp(currSize.x, 0, lerpSpd);
                currSize.y = currSize.x;

                m_Image.GetComponent<RectTransform>().sizeDelta = currSize;

                break;
        }
    }

    public void AddToIndex(int value) { m_Idx += value; }

    public void SetPosition(Vector2 pos)
    {
        transform.position = pos;
    }

    public void Initialise(Item item, int idx = 0, bool animation = false)
    {
        m_Idx = idx;
        m_Item = item;

        if (animation)
            m_CurrAnim = Animation.EXPAND;

        if (m_DescBox)
            m_DescBox.Initialise(m_Item);
    }
}

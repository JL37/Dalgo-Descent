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

    protected RawImage m_Image;
    protected Item m_Item;

    protected int m_Idx = 0;
    protected float m_Size = 50;

    // Start is called before the first frame update
    void Start()
    {
        m_Image = GetComponent<RawImage>();
        m_Image.texture = m_Item.GetCurrTexture();
        m_Size = m_Image.GetComponent<RectTransform>().sizeDelta.x;

        if (m_CurrAnim == Animation.EXPAND)
            m_Image.GetComponent<RectTransform>().sizeDelta = new Vector2(0, 0);

        //Get position based on idx
        float x = 50f + (68 * (m_Idx % 8));
        float y = -50f;

        if (m_Idx >= 8)
            y = -112f;

        //Setting anchor first
        m_Image.GetComponent<RectTransform>().anchoredPosition = new Vector2(x, y);
        transform.localScale = new Vector2(1, 1);
    }

    // Update is called once per frame
    void Update()
    {
        UpdateAnimation();
    }

    protected void UpdateAnimation()
    {
        Vector2 currSize = m_Image.GetComponent<RectTransform>().sizeDelta;
        float lerpSpd = 0.3f;

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

    public void Initialise(Item item, int idx = 0, bool animation = false)
    {
        m_Idx = idx;
        m_Item = item;

        if (animation)
            m_CurrAnim = Animation.EXPAND;
    }
}

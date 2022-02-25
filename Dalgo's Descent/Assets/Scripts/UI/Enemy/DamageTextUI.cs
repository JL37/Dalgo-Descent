using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DamageTextUI : MonoBehaviour
{
    [Header("Adjustable things for the animation lmao")]
    [SerializeField] float m_MaxFontSize = 48;
    [SerializeField] float m_MaxXOffset = 90;
    [SerializeField] float m_MaxYOffset = 150;
    [Range(0, 1)]
    [SerializeField] float m_AbsXSetFadeOutTrigger = 0.7f; 

    protected TMP_Text m_TextCmpt;

    protected float m_XOffSet = 0;
    protected bool m_ShowText = true;

    protected Transform m_TransformToTrack;
    protected Canvas m_Canvas;

    protected float m_HeightOffSet;

    // Start is called before the first frame update
    void Start()
    {
        m_TextCmpt = GetComponent<TMP_Text>();
        m_TextCmpt.fontSize = 0;
        m_Canvas = GameObject.FindGameObjectWithTag("HUD").GetComponent<Canvas>();

        if (!m_ShowText)
            gameObject.SetActive(false);
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        if (GameStateManager.Get_Instance.CurrentGameState == GameState.Paused) //Return when paused
            return;

        if (m_ShowText)
            UpdateScale(m_MaxFontSize, 0.3f);
        else
            UpdateScale(0, 0.3f);

        StickToTarget();

        QuadEquation(Time.fixedDeltaTime);
        UpdateShowText();
    }

    protected void UpdateShowText()
    {
        float absX = m_XOffSet / m_MaxXOffset;
        if (absX >= m_AbsXSetFadeOutTrigger && m_ShowText)
            m_ShowText = false;

        //print(m_TextCmpt.fontSize + " Wow");
        if (m_TextCmpt.fontSize <= 0 && !m_ShowText)
            gameObject.SetActive(false);
    }

    protected void QuadEquation(float dt)
    {
        Vector2 canvasPos = m_TextCmpt.transform.localPosition;

        canvasPos.x += m_XOffSet;
        canvasPos.y += ReturnAbsY(m_XOffSet / m_MaxXOffset) * m_MaxYOffset;

        m_TextCmpt.transform.localPosition = canvasPos;

        //Add to x offset
        float spd = 225 * dt;
        m_XOffSet = m_XOffSet + spd > m_MaxXOffset ? m_MaxXOffset : m_XOffSet + spd;
    }

    protected void StickToTarget()
    {
        //Update positioning of UI above text
        //Final position of marker above chest in world space
        float offset = m_TransformToTrack.position.y + m_HeightOffSet;
        Vector3 offsetPos = new Vector3(m_TransformToTrack.position.x, offset, m_TransformToTrack.position.z);

        //Calculate "screen" position (NOT a canvas/recttransform position)
        Vector2 canvasPos;
        Vector2 screenPoint = Camera.main.WorldToScreenPoint(offsetPos);

        //Convert screen pos to canvas/recttransform space (LEAVE CAMERA NULL IF SCREEN SPACE OVERLAY)
        RectTransformUtility.ScreenPointToLocalPointInRectangle(m_Canvas.GetComponent<RectTransform>(), screenPoint, null, out canvasPos);

        m_TextCmpt.transform.localPosition = canvasPos;
    }

    protected void UpdateScale(float target, float lerp)
    {
        if (Mathf.Abs(m_TextCmpt.fontSize - target) < 1)
        {
            m_TextCmpt.fontSize = target;
            return;
        }

        m_TextCmpt.fontSize = Mathf.Lerp(m_TextCmpt.fontSize, target, lerp);
    }

    public void Initialise(Transform transform, string str, float offset = 0)
    {
        //Set important stuff here
        m_TransformToTrack = transform;
        GetComponent<TMP_Text>().text = str;
        m_HeightOffSet = offset;

        //Reset values
        m_ShowText = true;
        m_XOffSet = 0;
        GetComponent<TMP_Text>().fontSize = 0;

        gameObject.SetActive(true);
    }

    protected float ReturnAbsY(float xoffset) //Offset must be 0 to 1
    {
        return -4 * xoffset * xoffset + 4 * xoffset;
    }
}

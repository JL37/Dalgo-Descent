using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ItemPickup : MonoBehaviour
{
    [Header("Objects")]
    [SerializeField] Canvas m_Canvas;
    [SerializeField] TMP_Text m_NameText;
    [SerializeField] Texture m_Texture;

    [Header("Variables")]
    [SerializeField] float m_Scale_InRange = 30f;
    [SerializeField] float m_Scale_NotInRange = 15f;

    [Tooltip("For the height between the object and the original starting point when in range")]
    [SerializeField] float m_OffSet_InRange = 10f;

    [Tooltip("For the height between the object and the original starting point when in range (VALUE MUST ALWAYS BE LOWER THAN WHEN WITHIN RANGE")]
    [SerializeField] float m_OffSet_NotInRange = 5f;

    [Tooltip("For the height between the object and the name text")]
    [SerializeField] float m_HeightOffset = 1.5f;

    [SerializeField] int m_MaxFontSize = 80;

    [Header("Speed")]
    [SerializeField] float m_TransformSpd = 2;
    [SerializeField] float m_RotationSpd = 360f;
    [SerializeField] float m_ScaleLerp = 0.2f;

    protected Vector3 m_OriginalPos;
    protected float m_YToAdd = 0f;
    protected float m_CurrFontSize = 0f;

    protected Chest m_Chest;
    protected Item m_Item;

    protected bool m_InRange = true;
    protected bool m_Interacted = false;
    protected bool m_Animating = false;

    public bool InRange
    {
        get { return m_InRange; }
        set { m_InRange = value; }
    }

    // Start is called before the first frame update
    void Start()
    {
        gameObject.transform.localScale = new Vector3(0, 0, 0);
        m_OriginalPos = gameObject.transform.position;

        m_Canvas.gameObject.SetActive(true);
        m_NameText.fontSize = 0;

        //Randomise item str8 up
        m_Item = new Item();
        m_Item.InitialiseRandomStats();
        //For now, give all items same image
        m_Item.SetCurrTexture(m_Texture);

        m_NameText.text = m_Item.GetText() + "\n<color=yellow>(E)</color>";
    }

    public void Initialise(Chest chest)
    {
        m_Chest = chest;
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        if (GameStateManager.Get_Instance.CurrentGameState == GameState.Paused) //Return when paused
            return;

        UpdateNameTextPos();

        if (!m_Animating)
        {
            UpdateTransform();
        }
        else
        {
            AnimateScaleModel(0,0.5f);
        }

        if (m_InRange && !m_Interacted)
            LerpFontSize(m_MaxFontSize);
        else
            LerpFontSize(0);
    }

    protected void LerpFontSize(int newSize)
    {
        if (m_NameText.fontSize == newSize)
        {
            m_CurrFontSize = newSize;
            return;
        }

        float spd = 0.3f;
        m_CurrFontSize = Mathf.Lerp(m_CurrFontSize, newSize, spd);
        m_NameText.fontSize = (int)m_CurrFontSize;
    }

    protected void UpdateNameTextPos()
    {
        //Update positioning of UI above text
        //Offset Y axis
        float offsetPosY = gameObject.transform.position.y + m_HeightOffset;

        //Final position of marker above chest in world space
        Vector3 offsetPos = new Vector3(gameObject.transform.position.x, offsetPosY, gameObject.transform.position.z);

        //Calculate "screen" position (NOT a canvas/recttransform position)
        Vector2 canvasPos;
        Vector2 screenPoint = Camera.main.WorldToScreenPoint(offsetPos);

        //Convert screen pos to canvas/recttransform space (LEAVE CAMERA NULL IF SCREEN SPACE OVERLAY)
        RectTransformUtility.ScreenPointToLocalPointInRectangle(m_Canvas.GetComponent<RectTransform>(), screenPoint, null, out canvasPos);

        m_NameText.transform.localPosition = canvasPos;
    }

    public void AnimateScaleModel(float scaleTarget, float lerpSpd = 0)
    {
        Vector3 ogWidthHeight = gameObject.transform.localScale;

        lerpSpd = lerpSpd > 0 ? lerpSpd : m_ScaleLerp;

        ogWidthHeight.x = Mathf.Lerp(ogWidthHeight.x, scaleTarget, lerpSpd);
        ogWidthHeight.y = ogWidthHeight.x;
        ogWidthHeight.z = ogWidthHeight.x;

        gameObject.transform.localScale = ogWidthHeight;
    }

    public void UpdateTransform()
    {
        //Transform
        float transformSpd = m_TransformSpd * Time.deltaTime;
        if (m_InRange || m_OffSet_NotInRange <= 0)
        {
            if (m_YToAdd < m_OffSet_InRange)
            {
                m_YToAdd += transformSpd;
            }
            else
            {
                m_YToAdd = m_OffSet_InRange;
            }
        } 
        else
        {
            if (m_YToAdd > m_OffSet_NotInRange + transformSpd)
                m_YToAdd -= transformSpd;
            else if (m_YToAdd < m_OffSet_NotInRange - transformSpd)
                m_YToAdd += transformSpd;
            else
                m_YToAdd = m_OffSet_NotInRange;
        }

        Vector3 pos = m_OriginalPos;
        pos.y += m_YToAdd;

        gameObject.transform.position = pos;

        //Scaling
        float scale = m_InRange ? m_Scale_InRange : m_Scale_NotInRange;
        AnimateScaleModel(scale);

        //Rotation
        RotateModel(Time.deltaTime);
    }

    protected void RotateModel(float delta)
    {
        //Rotation
        Vector3 rotation = gameObject.transform.eulerAngles;
        rotation.y += m_RotationSpd * delta;
        if (rotation.y > 360)
            rotation.y -= 360;

        gameObject.transform.eulerAngles = rotation;
    }

    public void OnInteract()
    {
        if (m_Interacted)
            return;

        m_Interacted = true;
        PlayerStats stat = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>();

        //Start animation
        m_Animating = true;
        stat.AddItem(m_Item, true);
    }
}

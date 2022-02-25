using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ItemPickup : MonoBehaviour, IObjectPooling
{
    [Header("Objects")]
    [SerializeField] Canvas m_Canvas;
    [SerializeField] RectTransform m_DescBox;
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

    [Header("Speed")]
    [SerializeField] float m_TransformSpd = 2;
    [SerializeField] float m_RotationSpd = 360f;
    
    protected float m_MaxScaleLerp = 1f;
    protected float m_CurrScaleLerp = 0f;

    protected Vector3 m_OriginalPos;
    protected float m_YToAdd = 0f;

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

    private void Awake()
    {
        //Randomise item str8 up
        m_Item = new Item();
        m_Item.InitialiseRandomStats();
        //For now, give all items same image
        m_Item.SetCurrTexture(m_Texture);


        //Change desc box text
        m_DescBox.GetComponent<ItemDescBoxUI>().Initialise(m_Item);
    }

    // Start is called before the first frame update
    void Start()
    {
        gameObject.transform.localScale = new Vector3(0, 0, 0);
        m_OriginalPos = gameObject.transform.position;

        m_Canvas.gameObject.SetActive(true);
        m_DescBox.transform.localScale = new Vector3(0, 0,0);
    }

    public Item GetItemRef() { return m_Item; }

    public void Initialise()
    {
        //Do nothing
    }

    public void Initialise(Chest chest)
    {
        //Randomise item str8 up
        m_Item = null;
        m_Item = new Item();
        m_Item.InitialiseRandomStats();
        //For now, give all items same image
        m_Item.SetCurrTexture(m_Texture);


        //Change desc box text
        if (m_DescBox != null)
            m_DescBox.GetComponent<ItemDescBoxUI>().Initialise(m_Item);

        m_OriginalPos = gameObject.transform.position;

        if (m_Canvas != null)
            m_Canvas.gameObject.SetActive(true);

        if (m_DescBox != null)
            m_DescBox.transform.localScale = new Vector3(0, 0, 0);

        m_Chest = chest;

        //Reset variables
        m_CurrScaleLerp = 0f;
        m_YToAdd = 0f;

        m_InRange = true;
        m_Interacted = false;
        m_Animating = false;
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
            AnimateScaleModel(0,0.55f);
        }

        if (m_InRange && !m_Interacted)
        {
            LerpDescBoxScale(m_MaxScaleLerp);
        }
        else
        {
            LerpDescBoxScale(0,0.5f);

            if (transform.localScale.x < 0.01f)
            {
                transform.localScale = Vector3.zero;

                if (transform.parent.GetComponent<ObjectPoolManager>() == null)
                    Destroy(gameObject);
                else
                    gameObject.SetActive(false);
            }
        }
    }

    protected void LerpDescBoxScale(float newSize, float spd = 0)
    {
        if (m_CurrScaleLerp == newSize)
            return;

        spd = spd <= 0 ? 0.3f : spd;
        m_CurrScaleLerp = Mathf.Lerp(m_CurrScaleLerp, newSize, spd);

        //Set new scale value
        Vector3 newScale;
        newScale.x = m_CurrScaleLerp;
        newScale.y = m_CurrScaleLerp;
        newScale.z = 1;

        m_DescBox.transform.localScale = newScale;

        if (Mathf.Abs(m_CurrScaleLerp - newSize) < 0.01f)
        {
            m_DescBox.transform.localScale = new Vector3(newSize, newSize, newSize);
            m_CurrScaleLerp = newSize;
        }
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

        m_DescBox.transform.localPosition = canvasPos;
    }

    public void AnimateScaleModel(float scaleTarget, float lerpSpd = 0)
    {
        Vector3 ogWidthHeight = gameObject.transform.localScale;

        lerpSpd = lerpSpd > 0 ? lerpSpd : m_MaxScaleLerp;

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

        //Remove reference from chest
        m_Chest.RemoveSpawnedItem();
        m_Chest.GetParticleSystem().Stop();
    }
}

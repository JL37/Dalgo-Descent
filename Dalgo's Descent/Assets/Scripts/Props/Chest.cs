using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Chest : MonoBehaviour
{
    [SerializeField] TMP_Text m_NameText;
    [SerializeField] Canvas m_Canvas;
    [SerializeField] GameUI m_GameUI;
    [SerializeField] RawImage m_ItemImage;

    [Header("Font size set for name text")]
    [SerializeField] int m_MaxFontSize = 80;
    protected float m_CurrFontSize;

    protected bool m_WithinRange = false;
    protected bool m_Opened = false;
    protected bool m_ItemAnimation = false;

    protected float m_Radian = 0;
    protected int m_AbValue = 0;
    protected int m_Cost = 1;
    protected float m_HeightOffset = 1.5f;
    protected float m_ImageDimensions = 100;

    protected PlayerStats m_PlayerStats = null;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<MeshRenderer>().material.color = Color.yellow;

        m_Canvas.gameObject.SetActive(true);
        m_CurrFontSize = 0;

        //Set text to the cost
        m_NameText.text = "<color=yellow>$</color>" + m_Cost + "\n<color=yellow>(E)</color>";
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        UpdateNameTextPos();

        if (m_ItemAnimation)
            UpdateItemPos();
        else if (m_ItemImage.gameObject.activeSelf)
            MoveItemToPanel();

        if (m_WithinRange && !m_Opened)
            LerpFontSize(m_MaxFontSize);
        else
            LerpFontSize(0);
    }

    protected void MoveItemToPanel()
    {
        if (GameStateManager.Get_Instance.CurrentGameState == GameState.Paused) //Return when paused
            return;

        //Move to panel and disappear
        Vector2 pos = m_ItemImage.transform.localPosition;
        Vector2 targetPos = m_GameUI.GetItemPanelLocalPos();

        //lerp the axis one by one
        float spd = 0.08f;
        pos.x = Mathf.Lerp(pos.x, targetPos.x, spd);
        pos.y = Mathf.Lerp(pos.y, targetPos.y, spd);

        //Set new position
        m_ItemImage.transform.localPosition = pos;

        //If its within item panel range, shrink the thing
        float length = (pos - m_GameUI.GetItemPanelLocalPos()).magnitude;
        if (length < 150) //SHRINK SIZE
        {
            Vector2 ogWidthHeight = m_ItemImage.GetComponent<RectTransform>().sizeDelta;

            spd = 0.3f;
            ogWidthHeight.x = Mathf.Lerp(ogWidthHeight.x, 0, spd);
            ogWidthHeight.y = ogWidthHeight.x;

            m_ItemImage.GetComponent<RectTransform>().sizeDelta = ogWidthHeight;

            if (ogWidthHeight.x == 0)
            {
                //Do stuff here
                m_ItemImage.gameObject.SetActive(false);
            }
        }
    }

    protected void LerpFontSize(int newSize)
    {
        if (GameStateManager.Get_Instance.CurrentGameState == GameState.Paused) //Return when paused
            return;

        if (m_NameText.fontSize == newSize)
        {
            m_CurrFontSize = newSize;
            return;
        }

        float spd = 0.3f;
        m_CurrFontSize = Mathf.Lerp(m_CurrFontSize, newSize, spd);
        m_NameText.fontSize = (int)m_CurrFontSize;
    }

    protected void UpdateItemPos()
    {
        if (GameStateManager.Get_Instance.CurrentGameState == GameState.Paused) //Return when paused
            return;

        if (!m_ItemImage.gameObject.activeSelf)
            m_ItemImage.gameObject.SetActive(true);

        //Item slowly increasing in size until maximum
        Vector2 ogWidthHeight = m_ItemImage.GetComponent<RectTransform>().sizeDelta;

        float spd = 0.3f;
        ogWidthHeight.x = Mathf.Lerp(ogWidthHeight.x, m_ImageDimensions, spd);
        ogWidthHeight.y = ogWidthHeight.x;

        m_ItemImage.GetComponent<RectTransform>().sizeDelta = ogWidthHeight;

        //Update positioning of UI above text
        //Offset Y axis
        float offsetPosY = gameObject.transform.position.y;

        //Final position of marker above chest in world space
        Vector3 offsetPos = new Vector3(gameObject.transform.position.x, offsetPosY, gameObject.transform.position.z);

        //Calculate "screen" position (NOT a canvas/recttransform position)
        Vector2 canvasPos;
        Vector2 screenPoint = Camera.main.WorldToScreenPoint(offsetPos);

        //Convert screen pos to canvas/recttransform space (LEAVE CAMERA NULL IF SCREEN SPACE OVERLAY)
        RectTransformUtility.ScreenPointToLocalPointInRectangle(m_Canvas.GetComponent<RectTransform>(), screenPoint, null, out canvasPos);

        if (m_AbValue == 0)
            m_AbValue = canvasPos.x > 0 ? -1 : 1;

        UpdateSineGraph(ref canvasPos);

        m_ItemImage.transform.localPosition = canvasPos;
    }

    protected void UpdateSineGraph(ref Vector2 ogPos)
    {
        //Move angle by speed
        float angleClamp = 315f;
        float xAxisRatio = 100f; //1 bump/ 180 degrees 
        float spd = Mathf.Deg2Rad * 300f * Time.fixedDeltaTime;

        //Sine graoh
        float height = 300f;
        if (Mathf.Deg2Rad * 180 >= m_Radian)
        {
            //Wide tall sine graph
            float y = height * Mathf.Sin(m_Radian);
            float x = (m_Radian / (Mathf.Deg2Rad * 180)) * xAxisRatio;

            ogPos.x += (x * m_AbValue);
            ogPos.y += y;
        } 
        else if (Mathf.Deg2Rad * angleClamp >= m_Radian)
        {
            float y = (height/2) * Mathf.Sin(2 * (m_Radian - Mathf.Deg2Rad * 90));
            float x = (m_Radian / (Mathf.Deg2Rad * 180)) * xAxisRatio;

            ogPos.x += (x * m_AbValue);
            ogPos.y += y;
        }

        m_Radian += spd;
        if (Mathf.Deg2Rad * angleClamp < m_Radian)
            m_ItemAnimation = false;
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            m_PlayerStats = other.GetComponent<PlayerStats>();
            if (!m_PlayerStats)
            {
                print("Player has no player stats!");
                return;
            }

            m_WithinRange = true;
            m_PlayerStats.SetChest(this);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            m_WithinRange = false;
            m_PlayerStats.SetChest(null);
        }
    }

    public void OnInteract()
    {
        if (!m_WithinRange) //If within range or not
            return;

        if (m_Opened)
            return;

        //Check if player has enough coins la
        if (m_PlayerStats.DeductCoin(m_Cost))
        {
            m_Opened = true;
            GetComponent<MeshRenderer>().material.color = Color.red;
            print("CHEST HAS BEEN OPENED");

            //Randomise items and put into player
            Item newItem = new Item();
            newItem.InitialiseStats(m_PlayerStats);
            m_PlayerStats.AddItem(newItem);

            //Start Item animation
            m_ItemAnimation = true;
        } 
        else
        {
            //Show error
            m_GameUI.ShowError("Not enough coins!");
        }
    }
}

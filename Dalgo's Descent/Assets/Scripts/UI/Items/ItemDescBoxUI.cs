using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ItemDescBoxUI : MonoBehaviour
{
    [Header("Objects")]
    [SerializeField] TMP_Text m_Header;
    [SerializeField] TMP_Text m_Body;
    [SerializeField] TMP_Text m_Rarity;
    [SerializeField] Image m_Background;

    protected string m_HeaderTxt = "";
    protected string m_BodyTxt = "";
    protected string m_RarityTxt = "";
    protected Color m_RarityCol = Color.white;

    // Start is called before the first frame update
    void Start()
    {
        m_Header.text = m_HeaderTxt;
        m_Body.text = m_BodyTxt;
        m_Rarity.text = m_RarityTxt;
        m_RarityCol.a = m_Background.color.a;
        m_Background.color = m_RarityCol;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //CALL THIS ONLY IN AWAKE AH, YOU BETTER NOT CB I TELL YOU
    public void Initialise(string header, string body)
    {
        print("Desc box text initialised!");

        m_HeaderTxt = header;
        m_BodyTxt = body;

        if (m_Header)
            m_Header.text = m_HeaderTxt;

        if (m_Body)
            m_Body.text = m_BodyTxt;
    }

    //CALL THIS BEFORE THIS BS SPAWN AH, DONT BASTARD BRO
    public void Initialise(Item item)
    {
        print("Desc box text initialised!");

        m_HeaderTxt = item.GetName();
        m_BodyTxt = item.GetInfo();
        m_RarityTxt = "(" + item.GetRarityText() + ")";
        m_RarityCol = item.GetRarityColor();

        if (m_Header)
            m_Header.text = m_HeaderTxt;

        if (m_Body)
            m_Body.text = m_BodyTxt;

        if (m_Rarity)
            m_Rarity.text = m_RarityTxt;

        if (m_Background)
        {
            m_RarityCol.a = m_Background.color.a;
            m_Background.color = m_RarityCol;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ItemDescBoxUI : MonoBehaviour
{
    [Header("Objects")]
    [SerializeField] TMP_Text m_Header;
    [SerializeField] TMP_Text m_Body;

    protected string m_HeaderTxt = "";
    protected string m_BodyTxt = "";

    // Start is called before the first frame update
    void Start()
    {
        m_Header.text = m_HeaderTxt;
        m_Body.text = m_BodyTxt;
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
    }
}

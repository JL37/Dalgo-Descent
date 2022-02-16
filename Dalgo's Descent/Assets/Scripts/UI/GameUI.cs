using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameUI : MonoBehaviour
{
    [Header("Objects")]
    [SerializeField] TMP_Text m_ErrorText;
    [SerializeField] GameObject m_ItemPanel;
    [SerializeField] GameObject m_TempPanel;

    [Header("Timers")]
    protected float m_CurrErrorTimer = 0f;
    [SerializeField] float m_MaxErrorTimer = 2f;

    [Header("Alpha reduction for error timer")]
    [SerializeField] float m_AlphaReduce = 1.5f;

    // Start is called before the first frame update
    void Start()
    {
        m_ErrorText.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (m_CurrErrorTimer > 0)
        {
            m_CurrErrorTimer -= Time.deltaTime;
        }
        else if (m_ErrorText.gameObject.activeSelf)
        {
            //print("Color alpha be like: " + errorText.color.a);
            Color color = m_ErrorText.color;
            color.a -= m_AlphaReduce * Time.deltaTime;
            //print("Reduce be like: " + alphaReduce * Time.deltaTime);

            m_ErrorText.color = color;
            //print("Color alpha now be like: " + errorText.color.a);
            if (m_ErrorText.color.a <= 0)
            {
                m_ErrorText.gameObject.SetActive(false);
                m_ErrorText.color = new Color(1, 1, 1, 1);
            }
        }
    }

    public Vector2 GetItemPanelLocalPos()
    {
        return m_ItemPanel.gameObject.transform.localPosition;
    }

    public Transform GetItemPanelTransform()
    {
        return m_ItemPanel.gameObject.transform;
    }

    public Transform GetTempPanelTransform()
    {
        return m_TempPanel.gameObject.transform;
    }

    public ObjectPoolManager GetObjectPoolManager()
    {
        return m_TempPanel.GetComponent<ObjectPoolManager>();
    }
    public void ShowError(string str)
    {
        //Error text
        m_ErrorText.text = str;
        m_ErrorText.color = new Color(1, 0, 0, 1);

        //Variables
        m_CurrErrorTimer = m_MaxErrorTimer;

        //Finally show error
        m_ErrorText.gameObject.SetActive(true);
    }
}

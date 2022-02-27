using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CreditsAnimation : MonoBehaviour
{
    [Header("Objects")]
    [SerializeField] TMP_Text m_CreditsText;
    [SerializeField] Image m_Background;

    [Header("Variables to change")]
    [SerializeField] string[] m_CreditArr;
    protected List<string> m_CurrArr;

    [SerializeField] float m_FlashDuration = 0.15f;
    [SerializeField] int m_FlashAmount = 3;

    [SerializeField] float m_MaxTimer = 1.7f;
    protected float m_CurrTimer;

    protected IEnumerator m_Enumerator = null;

    // Start is called before the first frame update
    void Start()
    {
        //Disable
        m_CreditsText.gameObject.SetActive(false);
        m_CurrArr = new List<string>();
        m_CurrTimer = m_MaxTimer;

        enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (m_Enumerator != null)
            return;

        m_CurrTimer -= Time.deltaTime;
        if (m_CurrTimer <= 0)
        {
            m_CurrTimer = m_MaxTimer;
            m_Enumerator = SwitchCredits();

            StartCoroutine(m_Enumerator);
        }
    }

    protected IEnumerator SwitchCredits()
    {
        for (int i = 0; i < m_FlashAmount; ++i)
        {
            //Flip color
            m_CreditsText.color = ToggleColor(m_CreditsText.color);
            m_Background.color = ToggleColor(m_Background.color);

            yield return new WaitForSecondsRealtime(m_FlashDuration);
        }

        if (m_CurrArr.Count >= m_CreditArr.Length)
            m_CurrArr.Clear();

        //Randomise text
        string newText = "";
        do
        {
            newText = m_CreditArr[Random.Range(0, m_CreditArr.Length)];
        }
        while (ArrayContains(newText));

        m_CreditsText.text = newText.Replace("\\n","\n");

        print(m_CreditsText.text);
        m_CurrArr.Add(newText);

        m_Enumerator = null;
    }

    public void Initialise()
    {
        enabled = true;

        m_CreditsText.gameObject.SetActive(true);

        m_CreditsText.color = new Color(1, 1, 1);
        m_Background.color = new Color(0, 0, 0);
    }

    public Color ToggleColor(Color currColor)
    {
        if (currColor.r == 0)
            return new Color(1, 1, 1);
        else
            return new Color(0,0,0);
    }

    bool ArrayContains(string g)
    {
        for (int i = 0; i < m_CurrArr.Count; i++)
        {
            if (m_CurrArr[i] == g) return true;
        }
        return false;
    }
}

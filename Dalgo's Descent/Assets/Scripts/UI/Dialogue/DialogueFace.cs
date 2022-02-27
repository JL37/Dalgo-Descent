using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueFace : MonoBehaviour
{
    [Header("Listeners")]
    [SerializeField] FaceAnimationListener[] m_Listeners;

    [Header("For basic default animation")]
    [SerializeField] Vector3 m_DownOffSet = new Vector3(0,-25,0);

    [Header("For center animations")]
    [SerializeField] Vector3 m_Center = new Vector3(0, -170, 0);
    [SerializeField] Vector2 m_InitialCenterSize = new Vector2(1500, 1500);

    [Header("Variables to adjust")]
    [SerializeField] TMP_Text m_DialogueText;
    [SerializeField] float m_ZoomInSlowSpd = 100f;
    [SerializeField] float m_ScreenShakeDuration = 1f;
    [SerializeField] Vector2 m_ScreenShakeRandom = new Vector2(20, 20);

    [Header("Variables to adjust")]
    [SerializeField] float m_LerpSpd = 0.15f;

    protected Vector2 m_OriginalSize;
    protected Vector3 m_OriginalPos;
    protected Vector3 m_PrevPos;
    protected Vector3 m_DialogueTextPrevPos;

    //Animation
    protected DialogueSystem.ANITYPE m_CurrAnimation = DialogueSystem.ANITYPE.NOTHING;
    protected IEnumerator m_CurrIEnumerator = null;
    protected bool m_Animating = true;

    //Duration of animation and if hardlocked
    protected bool m_HardLocked = false;
    protected float m_Duration = 0f;

    //Object reference
    protected RectTransform m_RectTransform;

    // Start is called before the first frame update
    void Start()
    {
        m_OriginalPos = GetComponent<RectTransform>().localPosition;
        GetComponent<RectTransform>().localPosition += m_DownOffSet;

        m_RectTransform = GetComponent<RectTransform>();
        m_OriginalSize = m_RectTransform.sizeDelta;

        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        switch (m_CurrAnimation)
        {
            case DialogueSystem.ANITYPE.NOTHING:
                UpdateToTarget(m_OriginalPos, m_LerpSpd);
                break;

            case DialogueSystem.ANITYPE.ZOOMIN_SLOW:
                ZoomIn_Slow_Update();
                break;
        }
    }

    protected IEnumerator ZoomIn_Slow_Update()
    {
        float actualSpd = m_ZoomInSlowSpd * Time.deltaTime;

        for (float i = m_Duration; i >= 0; i -= Time.deltaTime)
        {
            Vector2 size = m_RectTransform.sizeDelta;
            size.x += actualSpd;
            size.y = size.x;

            m_RectTransform.sizeDelta = size;
            yield return null;
        }

        m_Animating = false;

        if (m_HardLocked)
            SendDoneToAllSubscribers();
    }

    public void Initialise(Sprite defFace)
    {
        gameObject.SetActive(true);
        GetComponent<Image>().sprite = defFace;
    }

    public void InitialiseAnimation((DialogueSystem.ANITYPE animation, float duration) tuple, bool _Override = true)
    {
        //Overriding
        if (m_Animating)
        {
            if (_Override)
            {
                if (m_CurrIEnumerator != null)
                    StopCoroutine(m_CurrIEnumerator);

                if (m_CurrAnimation == DialogueSystem.ANITYPE.SHAKEWITHTEXT)
                    m_DialogueText.rectTransform.localPosition = m_DialogueTextPrevPos;
            } 
            else
            {
                return;
            }
        }

        RectTransform rectTransform = GetComponent<RectTransform>();
        m_CurrAnimation = tuple.animation;

        m_Animating = true;
        m_HardLocked = false;

        switch (m_CurrAnimation)
        {
            case DialogueSystem.ANITYPE.NOTHING:
                rectTransform.localPosition = m_OriginalPos + m_DownOffSet;
                rectTransform.sizeDelta = m_OriginalSize;

                //Duration changes to lerp spd
                if (tuple.duration <= 1 && tuple.duration > 0)
                    m_LerpSpd = tuple.duration;

                break;

            case DialogueSystem.ANITYPE.ZOOMIN_SLOW:
                rectTransform.localPosition = m_Center;
                rectTransform.sizeDelta = m_InitialCenterSize;

                m_Duration = Mathf.Abs(tuple.duration);
                m_HardLocked = tuple.duration < 0 ? false : true;

                //Coroutine start!
                m_CurrIEnumerator = ZoomIn_Slow_Update();
                StartCoroutine(m_CurrIEnumerator);

                break;

            case DialogueSystem.ANITYPE.CENTER_NOTHING:
                rectTransform.localPosition = m_Center;
                rectTransform.sizeDelta = m_InitialCenterSize;

                m_Duration = Mathf.Abs(tuple.duration);
                m_HardLocked = tuple.duration < 0 ? false : true;

                m_CurrIEnumerator = Center_Nothing_Update();
                StartCoroutine(m_CurrIEnumerator);

                break;

            case DialogueSystem.ANITYPE.SHAKEWITHTEXT:
                m_PrevPos = rectTransform.localPosition;
                m_DialogueTextPrevPos = m_DialogueText.rectTransform.localPosition;
                m_Duration = m_ScreenShakeDuration;

                m_CurrIEnumerator = ShakeWithText_Update();
                StartCoroutine(m_CurrIEnumerator);

                break;
        }
    }

    protected IEnumerator ShakeWithText_Update()
    {
        for (float i = m_Duration; i >= 0; i -= Time.deltaTime)
        {
            //Random shaking of text and sprite
            Vector3 randomOffset = Vector3.zero;
            randomOffset.x = Random.Range(-m_ScreenShakeRandom.x, m_ScreenShakeRandom.x);
            randomOffset.y = Random.Range(-m_ScreenShakeRandom.y, m_ScreenShakeRandom.y);

            //Implementing to various gameobjects
            m_RectTransform.localPosition = m_PrevPos + randomOffset;
            m_DialogueText.rectTransform.localPosition = m_DialogueTextPrevPos + randomOffset;

            yield return null;
        }

        //Reset dialogue text location
        m_DialogueText.rectTransform.localPosition = m_DialogueTextPrevPos;
        m_Animating = false;
    }

    protected IEnumerator Center_Nothing_Update()
    {
        yield return new WaitForSecondsRealtime(m_Duration);

        m_Animating = false;

        if (m_HardLocked)
            SendDoneToAllSubscribers();
    }

    protected void UpdateToTarget(Vector3 pos, float lerp = 0.3f)
    {
        if ((GetComponent<RectTransform>().localPosition - pos).magnitude > 0.01f)
        {
            //Math
            //print("LERP LA");
            Vector3 newPos = GetComponent<RectTransform>().localPosition;
            newPos.x = Mathf.Lerp(newPos.x, pos.x, lerp);
            newPos.y = Mathf.Lerp(newPos.y, pos.y, lerp);
            newPos.z = Mathf.Lerp(newPos.z, pos.z, lerp);

            GetComponent<RectTransform>().localPosition = newPos;
        } 
        else
        {
            GetComponent<RectTransform>().localPosition = pos;
            m_Animating = false;

            //Send signals to other ppl subscribed to it
            //SendDoneToAllSubscribers();
        }
    }

    public bool GetHardLocked()
    {
        return m_Animating && m_HardLocked;
    }

    protected void SendDoneToAllSubscribers()
    {
        for (int i = 0; i < m_Listeners.Length; ++i)
            m_Listeners[i].ReceiveSignal();
    }
}

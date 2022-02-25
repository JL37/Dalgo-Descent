using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DialogueSystem : MonoBehaviour
{
    [Header("Objects")]
    [SerializeField] TMP_Text m_DialogueTextObject;
    [SerializeField] Image m_CurrFace;
    [SerializeField] Image m_Arrow;

    [Header("Adjustable variable")]
    [SerializeField] Sprite m_DefaultFaceSprite;
    [SerializeField] int m_FaceApperanceIdx = 1; //Index in which face appears
    [SerializeField] float m_Timer = 0.1f;
    
    protected List<(string dialogue, float duration)> m_DialogueList;
    protected List<(Sprite img, int specialIdx)> m_FaceExeption;

    protected Sprite m_CurrFaceSprite;

    protected int m_CurrIDx = 0;

    protected bool m_RiggedTimer = false;
    protected bool m_Animating = false;
    protected bool m_AnimationDone = true;

    private void Awake()
    {
        m_DialogueList = new List<(string dialogue, float duration)>();
        m_FaceExeption = new List<(Sprite img, int specialIdx)>();

        m_Arrow.gameObject.SetActive(false);

        m_DialogueList.Add(("Au ah au salakau aiwdjaiowjdoaidjai wojdoajiwjdio awjiodjawjdiaj dioawjoidj??", 0));
        m_DialogueList.Add(("Cibai la salakau", 0));
        m_DialogueList.Add(("YOUR MOTHER IS FROM MY FATHER PUSSY SON DAUGHTER UNCLE!!!!!11!!!", 0));
        m_DialogueList.Add(("FUCK yOU la cibaI dOG", 2.3f));
        m_DialogueList.Add(("Sorry idk what came over me i love you man", 0f));
    }

    // Start is called before the first frame update
    void Start()
    {
        m_CurrFaceSprite = m_DefaultFaceSprite;
    }

    // Update is called once per frame
    void Update()
    {
        if (!m_Animating)
        {
            if (!m_RiggedTimer && m_DialogueList[m_CurrIDx].duration > 0)
                StartCoroutine(WaitThenAnimateNext(m_DialogueList[m_CurrIDx].duration));
            else if (!m_Arrow.gameObject.activeSelf && !m_RiggedTimer)
                m_Arrow.gameObject.SetActive(true);
        }

        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown("space"))
            OnClick();
    }

    protected IEnumerator WaitThenAnimateNext(float duration)
    {
        //print("Man moment");
        m_RiggedTimer = true;
        yield return new WaitForSecondsRealtime(duration);

        m_RiggedTimer = false;
        AnimateNextLine();
    }

    protected void OnClick()
    {
        if (m_DialogueList[m_CurrIDx].duration > 0) //remove control to skip if there is a rigged duration
            return;

        //If text animation not done, force it to finish
        if (!m_AnimationDone)
            m_AnimationDone = true;
        else if (!m_Animating && m_CurrIDx < m_DialogueList.Count - 1) //If done liao, then bruh moment just animate the next line if theres any
            AnimateNextLine();
    }

    public void AnimateNextLine(bool firstLine = false)
    {
        //m_Animating = true;
        //m_AnimationDone = false;
        m_Arrow.gameObject.SetActive(false);

        if (firstLine)
        {
            ResetDialogueText();
            AnimateText();
            return;
        }

        //m_DialogueTextObject.text = ""; //Make next line empty by default
        if (IncreaseIndex())
            AnimateText();
    }

    protected bool IncreaseIndex()
    {
        if (m_CurrIDx + 1 >= m_DialogueList.Count)
            return false;

        m_CurrIDx++;
        return true;
    }

    public void ResetDialogueText()
    {
        m_CurrIDx = 0;
        m_DialogueTextObject.text = "";
    }

    protected void AnimateText()
    {
        //Text animation
        StartCoroutine(I_AnimateText());

        //Face animation
        if (!m_CurrFace.gameObject.activeSelf && m_CurrIDx == m_FaceApperanceIdx)
            m_CurrFace.GetComponent<DialogueFace>().Initialise(m_DefaultFaceSprite);
        else if (m_CurrFace.gameObject.activeSelf)
            m_CurrFace.GetComponent<DialogueFace>().Initialise(GetCurrentSprite(m_CurrIDx));
    }

    protected Sprite GetCurrentSprite(int idx)
    {
        for (int i = 0; i < m_FaceExeption.Count; ++i)
        {
            if (m_FaceExeption[i].specialIdx == idx)
                return m_FaceExeption[i].img;
        }

        return m_DefaultFaceSprite;
    }

    protected IEnumerator I_AnimateText()
    {
        //Reset text
        m_DialogueTextObject.text = "";
        m_Animating = true;
        m_AnimationDone = false;

        //Animate letter by letter
        for (int i = 0; i < m_DialogueList[m_CurrIDx].dialogue.Length; ++i)
        {
            //print("animatin..." + m_DialogueList[m_CurrIDx].dialogue);

            //When left click or space is pressed (To skip)
            if (m_AnimationDone)
            {
                m_DialogueTextObject.text = m_DialogueList[m_CurrIDx].dialogue;
                break;
            }

            yield return new WaitForSecondsRealtime(m_Timer);

            m_DialogueTextObject.text += m_DialogueList[m_CurrIDx].dialogue[i];
        }

        m_AnimationDone = true;
        m_Animating = false;
    }

    public void AddToDialogueList((string name, float duration) tuple)
    {
        m_DialogueList.Add(tuple);
    }

    public void AddFaceException((Sprite img, int specialIdx) tuple)
    {
        m_FaceExeption.Add(tuple);
    }
}

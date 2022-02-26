using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DialogueSystem : MonoBehaviour
{
    //[IN DIALOGUE LIST]positive is must finish animation to continue(Animation goes on for x number of seconds) and negative is animation continues even aft x number of seconds [CANNOT BE 0])
    public enum ANITYPE
    {
        NOTHING, //Default animation, pops up on top of dialogue box
        CENTER_NOTHING, //Does nothing (Player is at center by default)
        ZOOMIN_SLOW, //Slowly zooms in on player at center (Teleports player to center)
        SHAKEWITHTEXT, //Shakes the animated face and text (Hard locks is for aft the text's animation)
    }

    [Header("Objects")]
    [SerializeField] TMP_Text m_DialogueTextObject;
    [SerializeField] TMP_Text m_NameTextObject;
    [SerializeField] Image m_CurrFace;
    [SerializeField] Image m_Arrow;

    [Header("Adjustable variable")]
    [SerializeField] Sprite m_DefaultFaceSprite;
    [SerializeField] int m_FaceApperanceIdx = 1; //Index in which face appears
    [SerializeField] float m_Timer = 0.1f;

    [Header("Subscribers")]
    [SerializeField] DialogueListener[] m_DialogueListeners;

    protected string m_SignalStr = "DEFAULT_END";

    //List for dialogue and stuff
    protected List<(string dialogue, float duration)> m_DialogueList;
    protected List<(Sprite img, int specialIdx)> m_FaceExeption;
    protected List<(ANITYPE animation, int specialIdx)> m_AnimationList;

    //List for triggers
    protected List<(Sprite defaultFace, int specialIdx)> m_DefaultFaceEventList;
    protected List<(string name, int specialIdx)> m_DefaultNameEventList;

    protected FaceAnimationListener m_AnimationListener;
    protected Sprite m_CurrFaceSprite;

    protected int m_CurrIdx = 0;

    protected bool m_RiggedTimer = false;
    protected bool m_Animating = false;
    protected bool m_AnimationDone = true;

    private void Awake()
    {
        m_DialogueList = new List<(string dialogue, float duration)>();
        m_FaceExeption = new List<(Sprite img, int specialIdx)>();
        m_AnimationList = new List<(ANITYPE animation, int specialIdx)>();

        m_DefaultFaceEventList = new List<(Sprite defaultFace, int specialIdx)>();
        m_DefaultNameEventList = new List<(string name, int specialIdx)>();

        m_Arrow.gameObject.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {
        m_CurrFaceSprite = m_DefaultFaceSprite;
        m_AnimationListener = GetComponent<FaceAnimationListener>();
    }

    private void LateUpdate()
    {
        AnimationSignalLateUpdate();
    }

    public void SetSignalText(string text)
    {
        m_SignalStr = text;
    }

    protected void AnimationSignalLateUpdate()
    {
        if (!m_AnimationListener.GetSignalReceived())
            return;

        //Resetting of necessary variables
        m_RiggedTimer = false;
        m_AnimationListener.ResetSignal();
        AnimateNextLine();
    }

    // Update is called once per frame
    void Update()
    {
        if (!m_Animating)
        {
            if (!m_RiggedTimer && m_DialogueList[m_CurrIdx].duration > 0)
                WaitThenAnimateNext();
            else if (!m_Arrow.gameObject.activeSelf && !m_RiggedTimer && m_CurrIdx < m_DialogueList.Count - 1)
                m_Arrow.gameObject.SetActive(true);
        }

        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown("space"))
            OnClick();
    }

    protected void WaitThenAnimateNext()
    {
        m_RiggedTimer = true;

        if (m_CurrFace.GetComponent<DialogueFace>().GetHardLocked())
            return; //Dont set timer

        StartCoroutine(I_WaitThenAnimateNext(m_DialogueList[m_CurrIdx].duration));
    }

    protected IEnumerator I_WaitThenAnimateNext(float duration)
    {
        //print("Man moment");
        yield return new WaitForSecondsRealtime(duration);

        m_RiggedTimer = false;
        AnimateNextLine();
    }

    protected void OnClick()
    {
        if (GetAnimationIdx(m_CurrIdx) >= 0 && m_DialogueList[m_CurrIdx].duration > 0) //Remove control if theres an unskippable animation
            return;

        if (m_DialogueList[m_CurrIdx].duration > 0) //remove control to skip if there is a rigged duration
            return;

        //If text animation not done, force it to finish
        if (!m_AnimationDone)
            m_AnimationDone = true;
        else if (!m_Animating && m_CurrIdx < m_DialogueList.Count - 1) //If done liao, then bruh moment just animate the next line if theres any
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

    protected int GetAnimationIdx(int dialogueIdx)
    {
        for (int i = 0; i < m_AnimationList.Count; ++i)
        {
            if (m_AnimationList[i].specialIdx == dialogueIdx)
                return i;
        }

        return -1;
    }

    protected bool IncreaseIndex()
    {
        if (m_CurrIdx + 1 >= m_DialogueList.Count)
            return false;

        m_CurrIdx++;
        return true;
    }

    public void ResetDialogueText()
    {
        m_CurrIdx = 0;
        m_DialogueTextObject.text = "";
    }

    protected void AnimateText()
    {
        //Events that will run for when entering the next line for the first time
        CheckEvents();

        //Text animation
        StartCoroutine(I_AnimateText());

        //Face animation
        if (!m_CurrFace.gameObject.activeSelf && m_CurrIdx == m_FaceApperanceIdx)
            m_CurrFace.GetComponent<DialogueFace>().Initialise(m_DefaultFaceSprite);
        else if (m_CurrFace.gameObject.activeSelf)
            m_CurrFace.GetComponent<DialogueFace>().Initialise(GetCurrentSprite(m_CurrIdx));
    }

    protected void CheckEvents()
    {
        //Animation event
        int animationIdx = GetAnimationIdx(m_CurrIdx);
        if (animationIdx >= 0)
            ActivateAnimations(animationIdx);


        //Change default face event
        Sprite newDefault = GetNewDefaultSprite(m_CurrIdx);
        if (newDefault)
            m_DefaultFaceSprite = newDefault;

        string newName = GetNewDefaultName(m_CurrIdx);
        if (newName != m_NameTextObject.text)
            m_NameTextObject.text = newName;
    }

    protected string GetNewDefaultName(int specialIdx)
    {
        for (int i = 0; i < m_DefaultNameEventList.Count; ++i)
        {
            if (specialIdx == m_DefaultFaceEventList[i].specialIdx)
                return m_DefaultNameEventList[i].name;
        }

        return m_NameTextObject.text;
    }

    protected Sprite GetNewDefaultSprite(int specialIdx)
    {
        for (int i = 0; i < m_DefaultFaceEventList.Count; ++i)
        {
            if (specialIdx == m_DefaultFaceEventList[i].specialIdx)
                return m_DefaultFaceEventList[i].defaultFace;
        }

        return null;
    }

    protected void ActivateAnimations(int animationIdx)
    {
        (ANITYPE animation, float duration) tuple = (m_AnimationList[animationIdx].animation, m_DialogueList[m_CurrIdx].duration);
        m_CurrFace.GetComponent<DialogueFace>().InitialiseAnimation(tuple);
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
        for (int i = 0; i < m_DialogueList[m_CurrIdx].dialogue.Length; ++i)
        {
            //print("animatin..." + m_DialogueList[m_CurrIDx].dialogue);

            //When left click or space is pressed (To skip)
            if (m_AnimationDone)
            {
                m_DialogueTextObject.text = m_DialogueList[m_CurrIdx].dialogue;
                break;
            }

            yield return new WaitForSecondsRealtime(m_Timer);

            m_DialogueTextObject.text += m_DialogueList[m_CurrIdx].dialogue[i];
        }

        m_AnimationDone = true;
        m_Animating = false;

        if (m_CurrIdx >= m_DialogueList.Count - 1)
            SendSignalToAllSubscribers();
    }

    protected void SendSignalToAllSubscribers()
    {
        foreach (DialogueListener listener in m_DialogueListeners)
            listener.ReceiveSignal(m_SignalStr);
    }

    public int AddToDialogueList((string name, float duration) tuple)
    {
        m_DialogueList.Add(tuple);

        return m_DialogueList.Count - 1;
    }

    public bool AddFaceException((Sprite img, int specialIdx) tuple)
    {
        for (int i = 0; i < m_FaceExeption.Count; ++i)
        {
            if (m_FaceExeption[i].specialIdx == tuple.specialIdx)
            {
                print("Face exception index " + tuple.specialIdx + " already added");
                return false;
            }
        }

        m_FaceExeption.Add(tuple);
        return true;
    }

    public bool AddAnimationSequence((ANITYPE animation, int specialIdx) tuple)
    {
        for (int i = 0; i < m_AnimationList.Count; ++i)
        {
            if (m_AnimationList[i].specialIdx == tuple.specialIdx)
            {
                print("Animation index " + tuple.specialIdx + " already added");
                return false;
            }
        }

        m_AnimationList.Add(tuple);
        return true;
    }

    public bool AddDefaultFaceEvent((Sprite defaultFace, int specialIdx) tuple)
    {
        for (int i = 0; i < m_DefaultFaceEventList.Count; ++i)
        {
            if (m_DefaultFaceEventList[i].specialIdx == tuple.specialIdx)
            {
                print("Default event list index " + tuple.specialIdx + " already added");
                return false;
            }
        }

        m_DefaultFaceEventList.Add(tuple);
        return true;
    }

    public bool AddDefaultNameEvent((string name, int specialIndex) tuple)
    {
        for (int i = 0; i < m_DefaultNameEventList.Count; ++i)
        {
            if (m_DefaultNameEventList[i].specialIdx == tuple.specialIndex)
            {
                print("Default event list index " + tuple.specialIndex + " already added");
                return false;
            }
        }

        m_DefaultNameEventList.Add(tuple);
        return true;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueCanvas : MonoBehaviour
{
    [Header("Objects")]
    [SerializeField] GameObject m_DialogueFolder;

    [Header("Variables")]
    [SerializeField] float m_TimerStart = 0.5f;

    [Header("Sprites")]
    [SerializeField] Sprite m_Happy;
    [SerializeField] Sprite m_SomewhatAnnoyed;
    [SerializeField] Sprite m_Annoyed;
    [SerializeField] Sprite m_Angry;
    [SerializeField] Sprite m_Acceptance;

    protected enum EMOTION
    {
        HAPPY = 0,
        SOMEWHATANNOYED,
        ANNOYED,
        ANGRY,
        ACCEPTANCE,
        KILLEDHIMSELF
    }

    protected PostGameInfo m_PostGameInfo;
    protected int m_CurrIdx;

    // Start is called before the first frame update
    void Start()
    {
        m_DialogueFolder.SetActive(false);

        m_PostGameInfo = PostGameInfo.GetInstance();
        m_CurrIdx = m_PostGameInfo.UpdateCurrIdx();

        AddDialogue();

        StartCoroutine(InitialiseDialogue(m_TimerStart));
    }

    protected void AddDialogue()
    {
        EMOTION currEmotion = (EMOTION)m_CurrIdx;

        switch (currEmotion)
        {
            case EMOTION.HAPPY:
                AddHappyDialogue();
                break;
        }
    }

    protected void AddHappyDialogue()
    {
        int i = 0;
        DialogueSystem d = m_DialogueFolder.GetComponent<DialogueSystem>();

        d.AddToDialogueList((".......... Oh!", 0));
        d.AddToDialogueList(("Hey, you. You're finally awake!", 0));
        d.AddToDialogueList(("Walked right into that imperial ambush, same as the others, and the rabbit over there.", 0));

        i = d.AddToDialogueList(("Damn those bird bastards, the otherworld was fine until they came along.", 0));
        d.AddFaceException((m_Annoyed, i));

        d.AddToDialogueList(("It's alright though, for I am your ticket back to the living world!", 0));
        d.AddToDialogueList(("........... Wait.", 0));
        d.AddToDialogueList(("You do not know who I am?", 0));

        i = d.AddToDialogueList(("", 1f));
        d.AddAnimationSequence((DialogueSystem.ANITYPE.CENTER_NOTHING, i));

        i = d.AddToDialogueList(("", 2.5f));
        d.AddAnimationSequence((DialogueSystem.ANITYPE.ZOOMIN_SLOW, i));

        i = d.AddToDialogueList(("HOW THE HELL COULD YOU NOT KNOW YOUR GOD???", 0));
        d.AddAnimationSequence((DialogueSystem.ANITYPE.SHAKEWITHTEXT, i));
        d.AddDefaultFaceEvent((m_Angry, i));

        d.AddToDialogueList(("LITERALLY I WAS THERE THE MOMENT YOU WERE BORN.", 0));
        d.AddToDialogueList(("THE MOMENT YOU WENT TO SCHOOL.", 0));
        d.AddToDialogueList(("THEN GOT ADOPTED.", 0));

    }

    protected IEnumerator InitialiseDialogue(float time)
    {
        yield return new WaitForSeconds(time);

        m_DialogueFolder.SetActive(true);
        m_DialogueFolder.GetComponent<DialogueSystem>().AnimateNextLine(true);
    }
}

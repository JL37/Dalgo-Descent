using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DialogueCanvas : MonoBehaviour, IEventListener
{
    [Header("Objects")]
    [SerializeField] GameObject m_DialogueFolder;
    [SerializeField] GameObject m_ChoiceFolder;

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

    //Signals
    protected string m_RestartSignal = "RESET";
    protected string m_MenuSignal = "MENU";

    protected string m_HappyIntroSignal = "HAPPY_INTRO";

    // Start is called before the first frame update
    void Start()
    {
        m_DialogueFolder.SetActive(false);

        m_PostGameInfo = PostGameInfo.GetInstance();
        m_CurrIdx = m_PostGameInfo.UpdateCurrIdx();

        AddDialogue();

        InitialiseDialogue(m_TimerStart);
    }

    protected void AddDialogue()
    {
        EMOTION currEmotion = (EMOTION)m_CurrIdx;
        m_DialogueFolder.GetComponent<DialogueSystem>().ResetSystem();

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
        d.SetSignalText(m_HappyIntroSignal);

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
        d.AddToDialogueList(("ONLY TO FIND THE LOVE OF YOUR LIFE THERE AND GET FIVE KIDS TO PAY CHILD SUPPORT FOR FOR THE REST OF YOUR LIFE AND", 0.7f));

        i = d.AddToDialogueList(("-You know what? I think we've started off on the wrong foot and I'm sorry", 0));
        d.AddDefaultFaceEvent((m_Happy, i));
        d.AddAnimationSequence((DialogueSystem.ANITYPE.NOTHING, i));

        d.AddToDialogueList(("Let's- Let's just try this again.", 0));
        d.AddToDialogueList(("My name is Aoshi, and I'm your God.", 0));
        d.AddToDialogueList(("And just in case your brain is so small to the point where you can't remember certain things", 0));

        i = d.AddToDialogueList(("TO THE POINT WHERE YOU FORGOT YOUR OWN CREATOR.", 0));
        d.AddAnimationSequence((DialogueSystem.ANITYPE.SHAKEWITHTEXT, i));
        d.AddFaceException((m_Annoyed, i));

        i = d.AddToDialogueList(("I will just add my name to the top left of that small little box.", 0));
        print("nbruh " + i);
        d.AddDefaultNameEvent(("Aoshi", i));

        i = d.AddToDialogueList(("What even is that small little box anyway? Why's it copying what I'm saying?-", 0));
        d.AddFaceException((m_SomewhatAnnoyed, i));

        d.AddToDialogueList(("-You know what, that's besides the point.", 0));
        d.AddToDialogueList(("Anyway, it seems you have died a really horrible death.", 0));
        d.AddToDialogueList(("Like, your body literally exploded into a million tiny pieces, how does that even happen?", 0));
        d.AddToDialogueList(("It's alright though, for Aoshi the Almighty will unexplode your million tiny pieces!", 0));

        d.AddToDialogueList(("Right now, I will give you a few choices.", 0));
        d.AddToDialogueList(("Option A: I rewind back to the time before you died, and you get a second chance at escaping.", 0));
        d.AddToDialogueList(("And option B: You just die and your wife and kids will never see you again.", 0));
        d.AddToDialogueList(("It is your choice.", 0));
        d.AddToDialogueList(("Also don't ask why I can't just spawn you out of the cage and let you escape.", 0));
        d.AddToDialogueList(("-Because there won't even be a game if I can just do that, and besides,", 0));
        d.AddToDialogueList(("There are no free things in this world, you got to work for your freedom.", 0));
        d.AddToDialogueList(("Now quit whining, and pick your choice.", 0));
    }

    protected void InitialiseDialogue(float time = 0)
    {
        if (time > 0)
        {
            StartCoroutine(I_InitialiseDialogue(time));
        }
        else
        {
            m_DialogueFolder.SetActive(true);
            m_DialogueFolder.GetComponent<DialogueSystem>().AnimateNextLine(true);
        }
    }

    protected IEnumerator I_InitialiseDialogue(float time)
    {
        yield return new WaitForSeconds(time);

        m_DialogueFolder.SetActive(true);
        m_DialogueFolder.GetComponent<DialogueSystem>().AnimateNextLine(true);
    }

    public void ReceiveSignal(string text)
    {
        print("received dialogue done event " + text + "!");

        if (text == m_HappyIntroSignal)
        {
            m_ChoiceFolder.SetActive(true);
        } 
        else if (text == m_RestartSignal)
        {
            SceneManager.LoadScene("Scenes/MainGame");
        } 
        else if (text == m_MenuSignal)
        {
            SceneManager.LoadScene("Scenes/MainMenuScene");
        }
    }

    public void ReceiveChoice(DialogueChoice.CHOICE choice)
    {
        EMOTION currEmotion = (EMOTION)m_CurrIdx;

        switch (currEmotion)
        {
            case EMOTION.HAPPY:
                ReceiveChoice_Happy(choice);
                break;
        }
    }

    protected void ReceiveChoice_Happy(DialogueChoice.CHOICE choice)
    {
        m_DialogueFolder.GetComponent<DialogueSystem>().ResetSystem();

        switch (choice)
        {
            case DialogueChoice.CHOICE.RESTART:
                m_DialogueFolder.GetComponent<DialogueSystem>().SetSignalText(m_RestartSignal);
                HappyChoiceRestart();
                break;

            case DialogueChoice.CHOICE.MENU:
                m_DialogueFolder.GetComponent<DialogueSystem>().SetSignalText(m_MenuSignal);
                HappyChoiceMenu();
                break;
        }

        InitialiseDialogue();
    }

    protected void HappyChoiceMenu()
    {
        int i = 0;
        DialogueSystem d = m_DialogueFolder.GetComponent<DialogueSystem>();
        d.AddToDialogueList(("You made the right ch", 0.2f));

        i = d.AddToDialogueList(("Wait that's what you chose??", 0));
        d.AddDefaultFaceEvent((m_SomewhatAnnoyed, i));

        d.AddToDialogueList(("Seriously????", 0));
        d.AddToDialogueList(("There are things in the world that cannot be fixed again, just like my broken marraige,", 0));
        d.AddToDialogueList(("But I gave you a chance to fix these things, and yet you passed it up?", 0));

        d.AddToDialogueList(("You-", 0));
        d.AddToDialogueList(("I-", 0));

        d.AddToDialogueList(("You- You- Fine, I'll grant your wish!", 0));
        d.AddToDialogueList(("I'll send your family your remains or something, I don't know.", 0));
        d.AddToDialogueList(("Oh wait, you don't have any remains cause you were blown to shreds.", 0));

        i = d.AddToDialogueList(("GOOOOOOOOOOOOOOOOOOOOOOOOOODDAAAAAAMMIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIII", 0.1F));
        d.AddAnimationSequence((DialogueSystem.ANITYPE.SHAKEWITHTEXT, i));
    }

    protected void HappyChoiceRestart()
    {
        DialogueSystem d = m_DialogueFolder.GetComponent<DialogueSystem>();
        d.AddToDialogueList(("You made the right choice.", 0));
        d.AddToDialogueList(("Well, good luck in your journey, this is your last chance to get back to your family.", 0));
        d.AddToDialogueList(("If you die again, it's over.", 0f));
        d.AddToDialogueList(("", 0f));
    }
}

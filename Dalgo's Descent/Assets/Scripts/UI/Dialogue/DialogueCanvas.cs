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

    protected string m_IntroSignal = "INTRO";

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

            case EMOTION.SOMEWHATANNOYED:
                AddSomeWhatAnnoyedDialogue();
                break;

            case EMOTION.ANNOYED:
                AddAnnoyedDialogue();
                break;

            case EMOTION.ANGRY:
                AddAngryDialogue();
                break;

            case EMOTION.ACCEPTANCE:
                AddAcceptanceDialogue();
                break;

            case EMOTION.KILLEDHIMSELF:
                AddKilledHimselfDialogue();
                break;
        }
    }

    protected void AddKilledHimselfDialogue()
    {
        DialogueSystem d = m_DialogueFolder.GetComponent<DialogueSystem>();
        d.SetSignalText(m_IntroSignal);
        d.SetFaceAppearanceIdx(999);

        d.AddToDialogueList(("..............", 0));
    }

    protected void AddAcceptanceDialogue()
    {
        int i = 0;
        DialogueSystem d = m_DialogueFolder.GetComponent<DialogueSystem>();
        d.SetSignalText(m_IntroSignal);

        i = d.AddToDialogueList(("..............", 0));
        d.AddDefaultNameEvent(("Aoshi", i));

        i = d.AddToDialogueList(("You know what?", 0));
        d.SetFaceAppearanceIdx(i);
        d.AddDefaultFaceEvent((m_Acceptance, i));

        d.AddToDialogueList(("I don't care anymore.", 0));
    }

    protected void AddAngryDialogue()
    {
        int i = 0;
        DialogueSystem d = m_DialogueFolder.GetComponent<DialogueSystem>();
        d.SetSignalText(m_IntroSignal);

        d.AddToDialogueList(("Hey, you. You're finally", 0.2f));

        i = d.AddToDialogueList(("<color=red>ARE YOU F###ING SERIOUS RIGHT NOW?</color>", 0));
        d.SetFaceAppearanceIdx(i);
        d.AddDefaultNameEvent(("Almighty", i));
        d.AddAnimationSequence((DialogueSystem.ANITYPE.SHAKEWITHTEXT, i));
        d.AddDefaultFaceEvent((m_Angry, i));

        d.AddToDialogueList(("<color=red>MOTHER###### S#I# ON A G#*(AMN STICK F*&%&N# GODd*^@</color>", 0));
        d.AddToDialogueList(("<color=red>WHY THE *&$&^ IN #&& OF THE EARTH F(*& DAMN IT SH***Y B*&C* C)*( SUCKER</color>", 0));

        i = d.AddToDialogueList(("<color=red>AWUIDJSAUIWHDJIASUHJIWDHJAUWHIJDSOAIHJWDUOIHSAUWIDHHAIUWHDJAIUWDJ</color>", 0));
        d.AddAnimationSequence((DialogueSystem.ANITYPE.SHAKEWITHTEXT, i));
    }

    protected void AddAnnoyedDialogue()
    {
        int i = 0;
        DialogueSystem d = m_DialogueFolder.GetComponent<DialogueSystem>();
        d.SetSignalText(m_IntroSignal);

        d.AddToDialogueList((".......... Oh!", 0));
        d.AddToDialogueList(("Hey, you. You're finally awake-", 0.2f));

        i = d.AddToDialogueList(("OH MY GOD. Are you serious???? How'd you even-", 0));
        d.SetFaceAppearanceIdx(i);
        d.AddDefaultNameEvent(("Almighty", i));
        d.AddAnimationSequence((DialogueSystem.ANITYPE.SHAKEWITHTEXT, i));
        d.AddFaceException((m_Annoyed, i));

        d.AddToDialogueList(("Hah!.... I... I... I can't even believe this!", 0));
        d.AddToDialogueList(("You actually got to be kidding me right now?", 0));

        i = d.AddToDialogueList(("...........", 0));
        d.AddFaceException((m_SomewhatAnnoyed, i));
        d.AddDefaultFaceEvent((m_Annoyed, i + 1));

        d.AddToDialogueList(("Alright, I'll be real with you.", 0));
        d.AddToDialogueList(("No matter how many times you die.", 0));
        d.AddToDialogueList(("You'll always come back.", 0));
        d.AddToDialogueList(("So unlike my broken marriage, you'll have as many chances as you want to fix things.", 0));
        d.AddToDialogueList(("<color=red>Do not take this as an invitation for you to keep dying, though.</color>", 0));

        d.AddToDialogueList(("You caught me red handed real good- I'll give you that, you caught me real good...", 0));
    }

    protected void AddSomeWhatAnnoyedDialogue()
    {
        int i = 0;
        DialogueSystem d = m_DialogueFolder.GetComponent<DialogueSystem>();
        d.SetSignalText(m_IntroSignal);

        d.AddToDialogueList((".......... Oh!", 0));
        d.AddToDialogueList(("Hey, you. You're finally awake-", 0.2f));

        i = d.AddToDialogueList(("Oh you? Didn't I revive you a few moments ago???", 0));
        d.SetFaceAppearanceIdx(i);
        d.AddDefaultNameEvent(("Almighty", i));
        d.AddAnimationSequence((DialogueSystem.ANITYPE.SHAKEWITHTEXT, i));
        d.AddFaceException((m_SomewhatAnnoyed,i));

        d.AddToDialogueList(("You..... Wha- How-", 0));
        d.AddToDialogueList(("Ohh.. I see what this is!", 0));
        d.AddToDialogueList(("Ahah... Ahahahahaha.....", 0));

        i = d.AddToDialogueList(("Ahahahahahahaha!!", 0));
        d.AddAnimationSequence((DialogueSystem.ANITYPE.SHAKEWITHTEXT, i));

        d.AddToDialogueList(("Alright buddy, you caught me!", 0));
        d.AddToDialogueList(("Actually, the previous chance wasn't your last chance.", 0));
        d.AddToDialogueList(("In fact, this will be your very last chance!", 0));

        i = d.AddToDialogueList(("Just make sure you don't die again.", 0));
        d.AddDefaultFaceEvent((m_SomewhatAnnoyed, i));

        d.AddToDialogueList(("Anyway, the usual choice,", 0));
        d.AddToDialogueList(("A: You ressurect, and try again.", 0));
        d.AddToDialogueList(("B: Noone will come to your funeral.", 0));

        d.AddToDialogueList(("So which will it be?", 0));
    }

    protected void AddHappyDialogue()
    {
        int i = 0;
        DialogueSystem d = m_DialogueFolder.GetComponent<DialogueSystem>();
        d.SetSignalText(m_IntroSignal);

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

        i = d.AddToDialogueList(("<color=red>HOW THE HELL COULD YOU NOT KNOW YOUR GOD???</color>", 0));
        d.AddAnimationSequence((DialogueSystem.ANITYPE.SHAKEWITHTEXT, i));
        d.AddDefaultFaceEvent((m_Angry, i));

        d.AddToDialogueList(("<color=red>LITERALLY I WAS THERE THE MOMENT YOU WERE BORN.</color>", 0));
        d.AddToDialogueList(("<color=red>THE MOMENT YOU WENT TO SCHOOL.</color>", 0));
        d.AddToDialogueList(("<color=red>THEN GOT ADOPTED.</color>", 0));
        d.AddToDialogueList(("<color=red>ONLY TO FIND THE LOVE OF YOUR LIFE THERE AND GET FIVE KIDS TO PAY CHILD SUPPORT FOR FOR THE REST OF YOUR LIFE AND</color>", 0.7f));

        i = d.AddToDialogueList(("-You know what? I think we've started off on the wrong foot and I'm sorry", 0));
        d.AddDefaultFaceEvent((m_Happy, i));
        d.AddAnimationSequence((DialogueSystem.ANITYPE.NOTHING, i));

        d.AddToDialogueList(("Let's- Let's just try this again.", 0));
        d.AddToDialogueList(("My name is Almighty, and I'm your God.", 0));
        d.AddToDialogueList(("And just in case your brain is so small to the point where you can't remember certain things", 0));

        i = d.AddToDialogueList(("TO THE POINT WHERE YOU FORGOT YOUR OWN CREATOR.", 0));
        d.AddAnimationSequence((DialogueSystem.ANITYPE.SHAKEWITHTEXT, i));
        d.AddFaceException((m_Annoyed, i));

        i = d.AddToDialogueList(("I will just add my name to the top left of that small little box.", 0));
        d.AddDefaultNameEvent(("Almighty", i));

        i = d.AddToDialogueList(("What even is that small little box anyway? Why's it copying what I'm saying?-", 0));
        d.AddFaceException((m_SomewhatAnnoyed, i));

        d.AddToDialogueList(("-You know what, that's besides the point.", 0));
        d.AddToDialogueList(("Anyway, it seems you have died a really horrible death.", 0));
        d.AddToDialogueList(("Like, your body literally exploded into a million tiny pieces, how does that even happen?", 0));
        d.AddToDialogueList(("It's alright though, for Aoshi the Almighty will unexplode your million tiny pieces!", 0));

        d.AddToDialogueList(("Right now, I will give you a few choices.", 0));
        d.AddToDialogueList(("Option A: I rewind back to the time before you died, and you get a second chance at escaping.", 0));
        d.AddToDialogueList(("Or option B: You just die and your wife and kids will never see you again.", 0));
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

        if (text == m_IntroSignal)
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
            case EMOTION.SOMEWHATANNOYED:
                ReceiveChoice_Happy(choice);
                break;

            case EMOTION.ANNOYED:
                ReceiveChoice_Annoyed(choice);
                break;

            case EMOTION.ANGRY:
                ReceiveChoice_Angry(choice);
                break;

            case EMOTION.ACCEPTANCE:
                ReceiveChoice_Acceptance(choice);
                break;

            case EMOTION.KILLEDHIMSELF:
                ReceiveChoice_KilledHimself(choice);
                break;
        }
    }

    protected void ReceiveChoice_KilledHimself(DialogueChoice.CHOICE choice)
    {
        DialogueSystem d = m_DialogueFolder.GetComponent<DialogueSystem>();
        d.ResetSystem();
        d.SetFaceAppearanceIdx(999);

        switch (choice)
        {
            case DialogueChoice.CHOICE.RESTART:
                d.SetSignalText(m_RestartSignal);
                d.AddToDialogueList(("..............", 0));
                break;

            case DialogueChoice.CHOICE.MENU:
                d.SetSignalText(m_MenuSignal);
                d.AddToDialogueList(("..............", 0));

                break;
            case DialogueChoice.CHOICE.STATS:
                InitialiseStatDialogue();
                break;
        }

        InitialiseDialogue();
    }

    protected void ReceiveChoice_Acceptance(DialogueChoice.CHOICE choice)
    {
        DialogueSystem d = m_DialogueFolder.GetComponent<DialogueSystem>();
        d.ResetSystem();

        switch (choice)
        {
            case DialogueChoice.CHOICE.RESTART:
                d.SetSignalText(m_RestartSignal);
                d.AddToDialogueList(("Just get lost.", 0));
                break;

            case DialogueChoice.CHOICE.MENU:
                d.SetSignalText(m_MenuSignal);
                d.AddToDialogueList(("Just get lost.", 0));

                break;
            case DialogueChoice.CHOICE.STATS:
                InitialiseStatDialogue();
                break;
        }

        InitialiseDialogue();
    }

    protected void ReceiveChoice_Angry(DialogueChoice.CHOICE choice)
    {
        m_DialogueFolder.GetComponent<DialogueSystem>().ResetSystem();

        switch (choice)
        {
            case DialogueChoice.CHOICE.RESTART:
                m_DialogueFolder.GetComponent<DialogueSystem>().SetSignalText(m_RestartSignal);
                AngryChoiceRestart();
                break;

            case DialogueChoice.CHOICE.MENU:
                m_DialogueFolder.GetComponent<DialogueSystem>().SetSignalText(m_MenuSignal);
                AngryChoiceMenu();
                break;
            case DialogueChoice.CHOICE.STATS:
                InitialiseStatDialogue();
                break;
        }

        InitialiseDialogue();
    }

    protected void AngryChoiceMenu()
    {
        int i = 0;
        DialogueSystem d = m_DialogueFolder.GetComponent<DialogueSystem>();

        d.AddToDialogueList(("<color=red>GOOD.</color>", 0));
        d.AddAnimationSequence((DialogueSystem.ANITYPE.SHAKEWITHTEXT, 0));
    }

    protected void AngryChoiceRestart()
    {
        int i = 0;
        DialogueSystem d = m_DialogueFolder.GetComponent<DialogueSystem>();

        d.AddToDialogueList(("<color=red>JUST GET LOST LA.</color>", 0));
        d.AddAnimationSequence((DialogueSystem.ANITYPE.SHAKEWITHTEXT, 0));
    }

    protected void ReceiveChoice_Annoyed(DialogueChoice.CHOICE choice)
    {
        m_DialogueFolder.GetComponent<DialogueSystem>().ResetSystem();

        switch (choice)
        {
            case DialogueChoice.CHOICE.RESTART:
                m_DialogueFolder.GetComponent<DialogueSystem>().SetSignalText(m_RestartSignal);
                AnnoyedChoiceRestart();
                break;

            case DialogueChoice.CHOICE.MENU:
                m_DialogueFolder.GetComponent<DialogueSystem>().SetSignalText(m_MenuSignal);
                AnnoyedChoiceMenu();
                break;
            case DialogueChoice.CHOICE.STATS:
                InitialiseStatDialogue();
                break;
        }

        InitialiseDialogue();
    }

    protected void AnnoyedChoiceMenu()
    {
        int i = 0;
        DialogueSystem d = m_DialogueFolder.GetComponent<DialogueSystem>();

        d.AddToDialogueList(("Huh, I mean... I can see where you're coming from, I guess.", 0));
        d.AddFaceException((m_SomewhatAnnoyed, 0));

        d.AddToDialogueList(("I mean, you've been getting your ass kicked for like, the third time now?", 0));
        d.AddToDialogueList(("So I can understand if you've given up", 0));
        d.AddToDialogueList(("........", 0));

        d.AddToDialogueList(("Well we ain't got much to discuss so.... I'll just send you on your merry way to bunny heaven right now.", 0));
        d.AddToDialogueList(("Bye, I guess?", 0));

        d.AddToDialogueList(("", 0));
    }

    protected void AnnoyedChoiceRestart()
    {
        int i = 0;
        DialogueSystem d = m_DialogueFolder.GetComponent<DialogueSystem>();

        d.AddToDialogueList(("Alright, but I must tell you though,", 0));
        d.AddFaceException((m_SomewhatAnnoyed, 0));

        d.AddToDialogueList(("DO", 0));
        d.AddToDialogueList(("NOT", 0));
        d.AddToDialogueList(("DIE", 0));
        d.AddToDialogueList(("AGAIN.", 0));

        d.AddToDialogueList(("Capische?", 0));

        i = d.AddToDialogueList(("Capische!", 0));
        d.AddDefaultFaceEvent((m_Happy, i));

        d.AddToDialogueList(("", 0));
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
            case DialogueChoice.CHOICE.STATS:
                InitialiseStatDialogue();
                break;
        }

        InitialiseDialogue();
    }

    protected void InitialiseStatDialogue()
    {
        EMOTION currEmotion = (EMOTION)m_CurrIdx;

        if (currEmotion != EMOTION.ANGRY && currEmotion != EMOTION.KILLEDHIMSELF)
            NormalStatsDialogue();
        else if (currEmotion == EMOTION.ANGRY)
            AggroStatsDialogue();
        else
            SummarisedStatsDialogue();

        if (currEmotion == EMOTION.ACCEPTANCE)
        {
            DialogueSystem d = m_DialogueFolder.GetComponent<DialogueSystem>();
            d.AddToDialogueList(("Now please. Just get lost.", 0));
        }
        else if (currEmotion == EMOTION.ANNOYED)
        {
            DialogueSystem d = m_DialogueFolder.GetComponent<DialogueSystem>();
            d.AddToDialogueList(("Piece of advice, don't you dare die again.", 0));
        }
    }

    protected void SummarisedStatsDialogue()
    {
        DialogueSystem d = m_DialogueFolder.GetComponent<DialogueSystem>();

        d.AddToDialogueList(("Time Survived: <color=orange>" + m_PostGameInfo.GetTimePassedText() + "</color>.", 0));

        string dmgText = "Total Damage is <color=orange>" + m_PostGameInfo.GetTotalDmg().ToString() + "</color>, max damage is <color=orange>" + m_PostGameInfo.GetMaxDmg() + "</color>.";
        d.AddToDialogueList((dmgText, 0));

        string enemyText = "Total Enemies is <color=orange> " + m_PostGameInfo.GetTotalEnemies() + "</color>, total bosses is <color=orange>" + m_PostGameInfo.GetTotalBosses() + "</color>.";
        d.AddToDialogueList((enemyText, 0));

        string moneyAndItemText = "Coins earned: <color=orange>" + m_PostGameInfo.GetTotalMoney() + " coins</color>, items collected <color=orange>" + m_PostGameInfo.GetTotalItems() + "</color>.";
        d.AddToDialogueList((moneyAndItemText, 0));

        d.AddToDialogueList(("..............", 0));
    }

    protected void AggroStatsDialogue()
    {
        int i = 0;
        DialogueSystem d = m_DialogueFolder.GetComponent<DialogueSystem>();

        d.AddToDialogueList(("YOU ONLY SURVIVED FOR <color=orange>" + m_PostGameInfo.GetTimePassedText().ToUpper() + "</color>.", 0));

        i = d.AddToDialogueList(("<color=red>MY GRANDMOTHER LIVED LONGER THAN YOU!!!!</color>",0));
        d.AddAnimationSequence((DialogueSystem.ANITYPE.SHAKEWITHTEXT, i));

        string dmgText = "YOU DID <color=orange>" + m_PostGameInfo.GetTotalDmg().ToString() + " TOTAL DAMAGE</color>, AND DID <color=orange>" + m_PostGameInfo.GetMaxDmg() + " MAX DAMAGE</color>.";
        d.AddToDialogueList((dmgText, 0));

        string enemyText = "AND YOU KILLED A TOTAL OF <color=orange> " + m_PostGameInfo.GetTotalEnemies() + " ENEMIES</color>, AND <color=orange>" + m_PostGameInfo.GetTotalBosses() + " OF THEM </color>ARE BOSSES.";
        d.AddToDialogueList((enemyText, 0));

        i = d.AddToDialogueList(("<color=red>MY SON WHO IS IN EIGHTH GRADE HAVE KILLED MORE BIRDS THAN IN YOUR WHOLE SAD ASS LIFE BY THE WAY.</color>", 0));
        d.AddAnimationSequence((DialogueSystem.ANITYPE.SHAKEWITHTEXT, i));

        string moneyAndItemText = "YOU GOT <color=orange>" + m_PostGameInfo.GetTotalMoney() + " COINS</color>, AND COLLECTED <color=orange>" + m_PostGameInfo.GetTotalItems() + " ITEMS</color> IN YOUR WHOLE LIFE.";
        d.AddToDialogueList((moneyAndItemText, 0));

        i = d.AddToDialogueList(("<color=red>NOW I KNOW WHY YOU CAN'T PAY FOR CHILD SUPPORT, YOU SAD POOR BASTARD.</color>", 0));
        d.AddAnimationSequence((DialogueSystem.ANITYPE.SHAKEWITHTEXT, i));

        i = d.AddToDialogueList(("<color=red>GET OUT OF MY SIGHT AND DON'T COME BACK. YOUR FATHER SHOULD HAVE PULLED OUT WHEN HE HAD THE CHANCE.</color>", 0));
        d.AddAnimationSequence((DialogueSystem.ANITYPE.SHAKEWITHTEXT, i));
    }

    protected void NormalStatsDialogue()
    {
        DialogueSystem d = m_DialogueFolder.GetComponent<DialogueSystem>();

        d.AddToDialogueList(("You survived for a total of <color=orange>" + m_PostGameInfo.GetTimePassedText() + "</color>.", 0));

        string dmgText = "You did a total of <color=orange>" + m_PostGameInfo.GetTotalDmg().ToString() + " damage</color>, with <color=orange>" + m_PostGameInfo.GetMaxDmg() + " damage </color>being your maximum.";
        d.AddToDialogueList((dmgText, 0));

        string enemyText = "Before your death, you slain a total of <color=orange> " + m_PostGameInfo.GetTotalEnemies() + " enemies</color>, <color=orange>" + m_PostGameInfo.GetTotalBosses() + " of them </color>being bosses.";
        d.AddToDialogueList((enemyText, 0));

        string moneyAndItemText = "You've accumulated <color=orange>" + m_PostGameInfo.GetTotalMoney() + " coins</color>, and have collected <color=orange>" + m_PostGameInfo.GetTotalItems() + " items</color> throughout your journey.";
        d.AddToDialogueList((moneyAndItemText, 0));
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VictoryDialogue : MonoBehaviour, IEventListener
{
    [Header("Faces to use")]
    [SerializeField] Sprite m_Almighty;
    [SerializeField] Sprite m_AlmightySerious;

    [Header("Object references")]
    [SerializeField] DialogueSystem m_DialogueSystem;
    [SerializeField] CreditsAnimation m_Credits;

    protected string m_SignalEnd = "END";

    // Start is called before the first frame update
    void Start()
    {
        AudioManager.Instance.Play("VictorySfx");
        m_DialogueSystem.gameObject.SetActive(false);
        AddDialogue();

        StartCoroutine(I_InitialiseDialogue(2));
    }

    protected IEnumerator I_InitialiseDialogue(float time)
    {
        yield return new WaitForSeconds(time);

        m_DialogueSystem.gameObject.SetActive(true);
        m_DialogueSystem.AnimateNextLine(true);
    }

    protected void AddDialogue()
    {
        int i = 0;
        DialogueSystem d = m_DialogueSystem;
        d.SetFaceAppearanceIdx(0);
        d.SetSignalText(m_SignalEnd);

        d.AddToDialogueList(("Oh hey! I see you've finally made it through!", 0));
        d.AddToDialogueList(("I got to say, I didn't think you'd really made it through but, here you are! You did it!", 0));

        i = d.AddToDialogueList(("Listen.... I know that I've lost my temper on you a couple of times.", 0));
        d.AddFaceException((m_AlmightySerious, i));

        d.AddToDialogueList(("But I mean well, you know?", 0));
        d.AddToDialogueList(("I wanted you to succeed and here you are! You have done it!", 0));

        d.AddToDialogueList(("You can finally go back to your wife and kids and pay for your kids child support", 0.3f));

        i = d.AddToDialogueList(("Oh shit you need to pay for you kids child support.", 0));
        d.AddDefaultFaceEvent((m_AlmightySerious, i));

        d.AddToDialogueList((" ", 0));
    }

    public void ReceiveSignal(string text)
    {
        if (text == m_SignalEnd)
        {
            AudioManager.Instance.Play("EndingSong");
            AudioManager.Instance.Stop("VictorySfx");

            StartCoroutine(I_StartCredits());
        }
    }

    protected IEnumerator I_StartCredits()
    {
        yield return new WaitForSecondsRealtime(0.5f);
        m_DialogueSystem.gameObject.SetActive(false);
        m_Credits.Initialise();
    }
}

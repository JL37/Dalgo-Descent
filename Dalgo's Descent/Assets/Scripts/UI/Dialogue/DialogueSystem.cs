using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DialogueSystem : MonoBehaviour
{
    [Header("Objects")]
    [SerializeField] TMP_Text m_DialogueTextObject;

    [Header("Adjustable variable")]
    [SerializeField] int m_FaceApperanceIdx = 1; //Index in which face appears
    [SerializeField] float m_Timer = 0.1f;
    
    protected List<(string dialogue, float duration)> m_DialogueList;
    protected List<(Image img, int specialIdx)> m_FaceExeption;

    protected int m_CurrIDx = 0;
    protected bool m_Animating = false;
    protected bool m_AnimationDone = false;

    private void Awake()
    {
        m_DialogueList = new List<(string dialogue, float duration)>();
        m_FaceExeption = new List<(Image img, int specialIdx)>();

        m_DialogueList.Add(("Au ah au salakau", 0));
        m_DialogueList.Add(("Cibai la salakau", 0));
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AnimateText()
    {
        StartCoroutine(I_AnimateText());
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

    public void AddFaceException((Image img, int specialIdx) tuple)
    {
        m_FaceExeption.Add(tuple);
    }
}

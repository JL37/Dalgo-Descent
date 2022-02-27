using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VictoryDialogue : MonoBehaviour
{
    [Header("Faces to use")]
    [SerializeField] Sprite m_Almighty;
    [SerializeField] Sprite m_Bird1;
    [SerializeField] Sprite m_Bird2;
    [SerializeField] Sprite m_Owl1;
    [SerializeField] Sprite m_Owl2;

    [Header("Object references")]
    [SerializeField] DialogueSystem m_DialogueSystem;

    // Start is called before the first frame update
    void Start()
    {
        m_DialogueSystem.gameObject.SetActive(false);
        AddDialogue();

    }

    protected void AddDialogue()
    {
        int i = 0;
        DialogueSystem d = m_DialogueSystem;
        
        //d.AddToDialogueList(("Hey!"))
    }
}

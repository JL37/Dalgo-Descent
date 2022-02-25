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

    // Start is called before the first frame update
    void Start()
    {
        m_DialogueFolder.SetActive(false);

        m_DialogueFolder.GetComponent<DialogueSystem>().AddFaceException((m_SomewhatAnnoyed, 2));
        m_DialogueFolder.GetComponent<DialogueSystem>().AddFaceException((m_Acceptance, 4));

        StartCoroutine(InitialiseDialogue(m_TimerStart));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    protected IEnumerator InitialiseDialogue(float time)
    {
        yield return new WaitForSeconds(time);

        m_DialogueFolder.SetActive(true);
        m_DialogueFolder.GetComponent<DialogueSystem>().AnimateNextLine(true);
    }
}

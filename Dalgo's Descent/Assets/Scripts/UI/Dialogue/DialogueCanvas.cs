using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueCanvas : MonoBehaviour
{
    [Header("Objects")]
    [SerializeField] GameObject m_DialogueFolder;

    [Header("Variables")]
    [SerializeField] float m_TimerStart = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        m_DialogueFolder.SetActive(false);
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
    }
}

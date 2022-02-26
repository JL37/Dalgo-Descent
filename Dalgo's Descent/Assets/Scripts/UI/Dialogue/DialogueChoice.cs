using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueChoice : MonoBehaviour
{
    public enum CHOICE
    {
        RESTART,
        MENU,
        STATS
    }

    [SerializeField] DialogueCanvas m_DialogueCanvas;

    // Start is called before the first frame update
    void Start()
    {
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            m_DialogueCanvas.ReceiveChoice(CHOICE.RESTART);
            gameObject.SetActive(false);
        }
        else if (Input.GetKeyDown(KeyCode.M))
        {
            m_DialogueCanvas.ReceiveChoice(CHOICE.MENU);
            gameObject.SetActive(false);
        }
    }
}

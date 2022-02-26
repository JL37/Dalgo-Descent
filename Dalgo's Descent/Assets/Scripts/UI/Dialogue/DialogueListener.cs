using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueListener : MonoBehaviour
{
    protected string m_CurrReceivedMsg = "";

    public void ReceiveSignal(string str)
    {
        m_CurrReceivedMsg = str;

        GetComponent<IEventListener>().ReceiveSignal();
    }
}

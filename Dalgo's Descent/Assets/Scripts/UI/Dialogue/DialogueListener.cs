using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueListener : MonoBehaviour
{
    public void ReceiveSignal(string str)
    {
        GetComponent<IEventListener>().ReceiveSignal(str);
    }
}

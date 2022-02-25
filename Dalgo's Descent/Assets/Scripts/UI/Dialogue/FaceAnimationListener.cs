using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceAnimationListener : MonoBehaviour
{
    [SerializeField] bool m_ManualOverride = false;
    protected bool m_SignalReceived = false;

    public void ReceiveSignal()
    {
        m_SignalReceived = true;

        if (!m_ManualOverride)
            StartCoroutine(ResetAtEndOfFrame());
    }

    public void ResetSignal()
    {
        m_SignalReceived = false;
    }

    public bool GetSignalReceived() { return m_SignalReceived; }

    protected IEnumerator ResetAtEndOfFrame()
    {
        yield return new WaitForEndOfFrame();
        ResetSignal();
    }
}

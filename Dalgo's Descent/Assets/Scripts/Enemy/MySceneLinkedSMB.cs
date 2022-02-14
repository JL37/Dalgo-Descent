using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MySceneLinkedSMB : SceneLinkedSMB<MyMonoBehaviour>
{
    public override void OnSLStatePostEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        m_MonoBehaviour.someSceneRigidbody.isKinematic = true;
    }
}
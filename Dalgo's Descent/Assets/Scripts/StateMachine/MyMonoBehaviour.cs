using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyMonoBehaviour : MonoBehaviour
{
    public Rigidbody someSceneRigidbody;
    public Animator animator;

    void Start()
    {
        SceneLinkedSMB<MyMonoBehaviour>.Initialise(animator, this);
    }
}
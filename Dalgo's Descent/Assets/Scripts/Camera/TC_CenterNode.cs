using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class TC_CenterNode : MonoBehaviour
{
    [Header("Object to follow")]
    [SerializeField] Transform following;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Set y axis to same as the thingy youre following la
        Vector3 toChangeTo = transform.position;
        toChangeTo.y = following.position.y;
        transform.position = toChangeTo;

        //Change looking direction
        Vector3 diff3D = transform.position - following.position;
        diff3D.y = 0;
        diff3D.Normalize();

        transform.LookAt(transform.position + diff3D);
    }
}

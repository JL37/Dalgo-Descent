using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    void Start()
    {

        Cursor.visible = false;
    }

    void Awake()
    {
        
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.LeftAlt))
        {
            Cursor.visible = true;
        }
        if (Input.GetKeyUp(KeyCode.LeftAlt))
        {
            Cursor.visible = false;
        }

    }
}
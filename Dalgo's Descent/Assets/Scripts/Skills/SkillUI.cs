using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class SkillUI : MonoBehaviour
{
    //Making it as singleton cause why not
    private static SkillUI m_singleInstance;


    public List<GameObject> stuff;
    public List<Image> gm;
    public static SkillUI Get_Instance
    {
        get
        {
            if (m_singleInstance == null)
                m_singleInstance = new SkillUI();
            return m_singleInstance;
        }
    }

    void Update()
    {

    }

    void Start()
    {
       
    }

    
}

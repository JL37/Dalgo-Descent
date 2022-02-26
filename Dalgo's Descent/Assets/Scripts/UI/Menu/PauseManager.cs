using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseManager : MonoBehaviour
{
    public static PauseManager Instance;
    [SerializeField] MenuBase[] Menus; 

    private void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        
    }

    public void OpenMenu(MenuBase menu)
    {
        for (int i = 0; i < Menus.Length; i++)
        {
            Menus[i].gameObject.SetActive(false);
        }

        menu.gameObject.SetActive(true);
    }

    public void OpenMenu(string menuName)
    {
        for (int i = 0; i < Menus.Length; i++)
        {
            if (Menus[i].menuName == menuName)
                Menus[i].gameObject.SetActive(true);
            else
                Menus[i].gameObject.SetActive(false);
        }
    }

}

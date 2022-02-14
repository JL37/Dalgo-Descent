using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; 


public class MenuController : MonoBehaviour
{
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void LoadScene(string sSceneName)
    {
        SceneManager.LoadScene(sSceneName);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

}

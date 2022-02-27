using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    public Image blackFadeImage;
    void Start()
    {
        Cursor.visible = true;
        blackFadeImage.gameObject.SetActive(true);
        AudioManager.Instance.Play("MainMenu");
        FadeOut();
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void LoadScene(string sSceneName)
    {
        SceneManager.LoadScene(sSceneName);
    }

    public void StartGame()
    {
        // AudioManager.Instance.Stop("MainMenu");
        PostGameInfo.ResetPostGameInfo();
        SceneManager.LoadScene("Scenes/MainGame");
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    void FadeOut()
    {
        StartCoroutine(DoFadeOut());
    }
    IEnumerator DoFadeOut()
    {
        yield return new WaitForSeconds(0.1f);
        while (blackFadeImage.color.a > 0)
        {
            blackFadeImage.color = new Color(0, 0, 0, Mathf.Lerp(blackFadeImage.color.a, -0.1f, Time.deltaTime * 1f));
            yield return null;
        }
    }
}

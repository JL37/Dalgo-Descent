using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ImageFadeIn : MonoBehaviour
{
    SpriteRenderer spriteRenderer;
    bool startedFading;
    float timer;
    // Start is called before the first frame update
    void Start()
    {
        timer = 0f;
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.color = new Color(1, 1, 1, 0);
    }

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer > 1f && !startedFading)
        {
            startedFading = true;
            FadeIn();
        }
    }

    void FadeIn()
    {
        StartCoroutine(DoFadeIn());
    }

    void FadeOut()
    {
        StartCoroutine(DoFadeOut());
    }

    IEnumerator DoFadeIn()
    {
        while (spriteRenderer.color.a < 1)
        {
            
            spriteRenderer.color = new Color(1, 1, 1, Mathf.Lerp(spriteRenderer.color.a, 1.1f, Time.deltaTime));
            yield return null;
        }
    }

    IEnumerator DoFadeOut()
    {
        while (spriteRenderer.color.a > 0)
        {
            spriteRenderer.color = new Color(1, 1, 1, Mathf.Lerp(spriteRenderer.color.a, -0.1f, Time.deltaTime));
            yield return null;
        }

        SwitchToMainMenu();
    }

    void SwitchToMainMenu()
    {
        SceneManager.LoadScene(1);
    }
}

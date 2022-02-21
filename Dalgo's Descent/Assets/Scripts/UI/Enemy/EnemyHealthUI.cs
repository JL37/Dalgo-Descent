using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthUI : MonoBehaviour
{
    [Header("Linked components")]
    [SerializeField] GameObject m_HealthFolder;
    [SerializeField] Image m_HealthBar;
    [SerializeField] Image m_BufferBar;
    [SerializeField] Health m_Health;

    [Header("Variables to adjust")]
    [SerializeField] bool m_InWorldSpace = true;
    [SerializeField] float m_MaxBufferTimer = 0.4f;
    [SerializeField] float m_FadeDuration = 0.7f;
    [SerializeField] float m_HealthLerpSpd = 0.25f;
    [SerializeField] float m_BufferLerpSpd = 0.1f;

    protected float m_CurrBufferTimer = 0f;
    protected GameObject m_Player;

    // Start is called before the first frame update
    void Start()
    {
        m_CurrBufferTimer = m_MaxBufferTimer;
        GetComponent<Canvas>().worldCamera = Camera.main;
        m_Player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        //Stop when pause
        if (GameStateManager.Get_Instance.CurrentGameState == GameState.Paused) //Ignore key presses when paused
            return;

        //Updating ui...
        if (m_InWorldSpace)
            m_HealthFolder.transform.LookAt(Camera.main.transform, Camera.main.transform.up);

        //Updating bars...
        UpdateHealthBar();
        UpdateBufferBar();
    }

    protected void UpdateBufferBar()
    {
        //Check if target is reached. If reached, return value
        if (Mathf.Abs(m_BufferBar.transform.localScale.x - m_HealthBar.transform.localScale.x) < 0.01f)
        {
            m_BufferBar.transform.localScale = m_HealthBar.transform.localScale;
            m_CurrBufferTimer = m_MaxBufferTimer;
            return;
        }

        //Timer offset (Bar will linger for awhile before going down in value)
        if (m_CurrBufferTimer > 0)
        {
            m_CurrBufferTimer -= Time.deltaTime;
            return;
        }

        //Lerping animation
        float targetScale = m_HealthBar.transform.localScale.x;
        float currScale = m_BufferBar.transform.localScale.x;

        currScale = Mathf.Lerp(currScale, targetScale, m_BufferLerpSpd);
        m_BufferBar.transform.localScale = new Vector3(currScale, 1, 1);
    }

    public void StartFadeAnimation(bool fadeAway)
    {
        gameObject.SetActive(true);
        StartCoroutine(Cour_FadeAnim(fadeAway));
    }

    protected IEnumerator Cour_FadeAnim(bool fadeAway)
    {
        if (fadeAway)
        {
            for (float i = 1; i >= 0; i -= Time.deltaTime / m_FadeDuration)
            {
                foreach (Transform child in m_HealthFolder.transform)
                {
                    child.gameObject.GetComponent<Image>().color = new Color(1, 1, 1, i);
                }

                yield return null;
            }

            //Set to false after successfully fading out
            gameObject.SetActive(false);
        } 
        else
        {
            for (float i = 0; i <= 1; i+= Time.deltaTime / m_FadeDuration)
            {
                foreach (Transform child in m_HealthFolder.transform)
                {
                    child.gameObject.GetComponent<Image>().color = new Color(1, 1, 1, i);
                }

                yield return null;
            }
        }
    }

    protected void UpdateHealthBar()
    {
        //Updating scaling of health (Using lerp as animation)
        float healthBarScale = Mathf.Lerp(m_HealthBar.transform.localScale.x, m_Health.currentHealth / m_Health.maxHealth, m_HealthLerpSpd);
        m_HealthBar.transform.localScale = new Vector3(healthBarScale, 1, 1);
    }
}

using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class GameUI : MonoBehaviour
{
    [Header("Objects")]
    [SerializeField] TMP_Text m_ErrorText;
    [SerializeField] GameObject m_ItemPanel;
    [SerializeField] GameObject m_TempPanel;
    [SerializeField] EnemyHealthUI m_BossHealth;
    [SerializeField] Image m_GameOverBg;
    [SerializeField] Volume m_Volume;

    [Header("Timers")]
    protected float m_CurrErrorTimer = 0f;
    [SerializeField] float m_MaxErrorTimer = 2f;

    [Header("Alpha reduction for error timer")]
    [SerializeField] float m_AlphaReduce = 1.5f;

    // Start is called before the first frame update
    void Start()
    {
        m_ErrorText.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (m_CurrErrorTimer > 0)
        {
            m_CurrErrorTimer -= Time.deltaTime;
        }
        else if (m_ErrorText.gameObject.activeSelf)
        {
            //print("Color alpha be like: " + errorText.color.a);
            Color color = m_ErrorText.color;
            color.a -= m_AlphaReduce * Time.deltaTime;
            //print("Reduce be like: " + alphaReduce * Time.deltaTime);

            m_ErrorText.color = color;
            //print("Color alpha now be like: " + errorText.color.a);
            if (m_ErrorText.color.a <= 0)
            {
                m_ErrorText.gameObject.SetActive(false);
                m_ErrorText.color = new Color(1, 1, 1, 1);
            }
        }
    }

    public void FadeOutGame(bool victory)
    {
        m_GameOverBg.gameObject.SetActive(true);

        StartCoroutine(I_BlurOut(3f));
        StartCoroutine(I_FadeOut(1.5f, victory));
    }

    protected IEnumerator I_BlurOut(float duration)
    {
        GetComponent<Canvas>().renderMode = RenderMode.ScreenSpaceCamera;
        GetComponent<Canvas>().worldCamera = Camera.main;

        for (float i = 0; i <= 1; i += Time.deltaTime / duration)
        {
            float focal_length = 50 * i;
            DepthOfField tmp;
            if (m_Volume.profile.TryGet(out tmp))
            {
                //print("FOCAL LA DOG");
                tmp.focalLength.value = focal_length;
            }

            yield return null;
        }
    }

    protected IEnumerator I_FadeOut(float duration, bool victory)
    {
        for (float i = 0; i <= 1; i += Time.deltaTime / duration)
        {
            Color ogColor = m_GameOverBg.color;
            ogColor.a = i;
            m_GameOverBg.color = ogColor;

            yield return null;
        }
        SceneManager.LoadScene(victory ? "VictoryScene" : "GameOverScene", LoadSceneMode.Single);
    }

    public void EnableBossUI(Health bossHealth)
    {
        m_BossHealth.gameObject.SetActive(true);
        m_BossHealth.SetHealth(bossHealth);
        m_BossHealth.StartFadeAnimation(false);
    }

    public void DisableBossUI()
    {
        m_BossHealth.StartFadeAnimation(true);
    }

    public Vector2 GetItemPanelLocalPos()
    {
        return m_ItemPanel.gameObject.transform.localPosition;
    }

    public Transform GetItemPanelTransform()
    {
        return m_ItemPanel.gameObject.transform;
    }

    public Transform GetTempPanelTransform()
    {
        return m_TempPanel.gameObject.transform;
    }

    public ObjectPoolManager GetObjectPoolManager()
    {
        return m_TempPanel.GetComponent<ObjectPoolManager>();
    }
    public void ShowError(string str)
    {
        //Error text
        m_ErrorText.text = str;
        m_ErrorText.color = new Color(1, 0, 0, 1);

        //Variables
        m_CurrErrorTimer = m_MaxErrorTimer;

        //Finally show error
        m_ErrorText.gameObject.SetActive(true);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tooltip_Warning : MonoBehaviour
{
    private static Tooltip_Warning getInstance;
    private Camera m_camera;
    private Text tooltipText;
    private Image backgroundImage;
    private RectTransform backgroundRectTransform;
    private float showTimer;
    private float flashTimer;
    private int flashState;
    private void Awake()
    {
        getInstance = this;
        backgroundRectTransform = transform.Find("background").GetComponent<RectTransform>();
        tooltipText = transform.Find("text").GetComponent<Text>();
        backgroundImage = transform.Find("background").GetComponent<Image>();
    }

    private void Update()
    {
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(transform.parent.GetComponent<RectTransform>(), Input.mousePosition, m_camera, out localPoint);
        transform.localPosition = localPoint;

        flashTimer += Time.deltaTime;
        float flashTimerMax = 0.1f;
        if(flashTimer>flashTimerMax)
        {
            flashState++;
            switch(flashState)
            {
                case 1:
                case 3:
                case 5:
                    tooltipText.color = new Color(1, 1, 1, 1);
                    backgroundImage.color = new Color(178.0f/255.0f, 0/255.0f, 0/255.0f, 1.0f);
                    break;
                case 2:
                case 4:
                    tooltipText.color = new Color(178.0f/255.0f, 0/255.0f, 0/255.0f, 1.0f);
                    backgroundImage.color = new Color(1,1,1,1);
                    break;
            }
        }
        showTimer -= Time.deltaTime;
        if(showTimer<=0.0f)
        {
            HideTooltip();
        }
    }


    private void ShowTooltip(string tooltipString, float showTimerMax =2.0f)
    {
        gameObject.SetActive(true);
        transform.SetAsLastSibling();

        tooltipText.text = tooltipString;
        float textPaddingSize = 4.0f;
        Vector2 backgroundsize = new Vector2(tooltipText.preferredWidth + textPaddingSize * 2.0f, tooltipText.preferredHeight + textPaddingSize * 2.0f);
        backgroundRectTransform.sizeDelta = backgroundsize;
        showTimer = showTimerMax;
        flashTimer = 0.0f;
        flashState = 0;
    }

    private void HideTooltip()
    {
        gameObject.SetActive(false);
    }

    public static void ShowTooltip_Static(string tooltipString)
    {
        getInstance.ShowTooltip(tooltipString);
    }

    public static void HideTooltip_Static()
    {
        getInstance.HideTooltip();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tooltip : MonoBehaviour
{
    private static Tooltip getInstance;
    private Camera m_camera;
    private Text tooltipText;
    private RectTransform backgroundRectTransform;

    private void Awake()
    {
        getInstance = this;
        backgroundRectTransform = transform.Find("background").GetComponent<RectTransform>();
        tooltipText = transform.Find("text").GetComponent<Text>();
    }

    private void Update()
    {
        Vector2 localPoint; 
        RectTransformUtility.ScreenPointToLocalPointInRectangle(transform.parent.GetComponent<RectTransform>(), Input.mousePosition, m_camera, out localPoint);
        transform.localPosition = localPoint;
    }


    private void ShowTooltip(string tooltipString)
    {
        gameObject.SetActive(true);
        transform.SetAsLastSibling();

        tooltipText.text = tooltipString;
        float textPaddingSize = 4.0f;
        Vector2 backgroundsize = new Vector2(tooltipText.preferredWidth + textPaddingSize * 2.0f, tooltipText.preferredHeight + textPaddingSize * 2.0f);
        backgroundRectTransform.sizeDelta = backgroundsize;
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
        if (getInstance == null)
            return;
        getInstance.HideTooltip();
    }
}

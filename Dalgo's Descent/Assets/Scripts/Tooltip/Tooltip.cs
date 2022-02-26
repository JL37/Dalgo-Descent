using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Tooltip : MonoBehaviour
{
    private static Tooltip getInstance;
    private Camera m_camera;

    public TMP_Text HeaderText;
    public TMP_Text BodyText;

    private RectTransform backgroundRectTransform;

    private void Awake()
    {
        getInstance = this;
        // backgroundRectTransform = transform.Find("background").GetComponent<RectTransform>();
    }

    private void Update()
    {
        Vector2 localPoint; 
        RectTransformUtility.ScreenPointToLocalPointInRectangle(transform.parent.GetComponent<RectTransform>(), Input.mousePosition, m_camera, out localPoint);
        //Debug.Log("Tooltip update");
        transform.localPosition = localPoint;
    }


    private void ShowTooltip(string header, string body)
    {
        gameObject.SetActive(true);
        transform.SetAsLastSibling();

        HeaderText.text = header;
        BodyText.text = body;
        // float textPaddingSize = 4.0f;
        // Vector2 backgroundsize = new Vector2(tooltipText.preferredWidth + textPaddingSize * 2.0f, tooltipText.preferredHeight + textPaddingSize * 2.0f);
        // backgroundRectTransform.sizeDelta = backgroundsize;
    }

    private void HideTooltip()
    {
        gameObject.SetActive(false);
    }

    public static void ShowTooltip_Static(string header, string body)
    {
        getInstance.ShowTooltip(header, body);
    }

    public static void HideTooltip_Static()
    {
        if (getInstance == null)
            return;
        getInstance.HideTooltip();
    }
}

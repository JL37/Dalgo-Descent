using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TooltipTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    public string details;

    public void OnPointerDown(PointerEventData eventData)
    {
        Tooltip_Warning.ShowTooltip_Static("You cannot use this skill yet!");
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Tooltip.ShowTooltip_Static(details) ;
        //Tooltip_Warning.ShowTooltip_Static(details);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Tooltip.HideTooltip_Static();
        Tooltip_Warning.HideTooltip_Static();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}

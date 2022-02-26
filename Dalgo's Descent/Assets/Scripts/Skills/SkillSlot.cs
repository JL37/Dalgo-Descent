using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using System;

public class SkillSlot : TooltipTrigger, IDropHandler, IPointerDownHandler
{
    // public DragDrop dragdrop;
    public SkillObject AnchoredSkill;
    public string keycode;
    public Sprite baseImage;
    private bool startListen = false;

    public Image SkillBorderImage;
    public TMP_Text SkillBorderTMP;

    public void Awake()
    {
        SkillBorderTMP.text = "";
    }
    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log("OnDrop");
        // Debug.Log(dragdrop.getCurrentRect());

        if(eventData.pointerDrag != null) //if you are dragging a game object
        {
            if(AnchoredSkill != null)
            AnchoredSkill.RebindKey("");
            AnchoredSkill = eventData.pointerDrag.GetComponent<Skill>().SkillScriptable;

            // Rebind Skill
            string keyCodeStore;
            keyCodeStore = (keycode == "LMB" || keycode == "RMB") ? "<Mouse>/" + keycode : "<Keyboard>/" + keycode;
            AnchoredSkill.RebindKey(keyCodeStore);
            Debug.Log("Rebinded \"" + AnchoredSkill.SkillName + "\" to " + keycode);

            UpdateToolTipInfo();

            // update skillslot image
            GetComponent<Image>().sprite = eventData.pointerDrag.GetComponent<Image>().sprite;

            SkillSlotManager.Instance.UpdateSkillSlots(this);
        }
    }

    public void RebindSkill()
    {
        //skill1_text.text = "Press a button";
        SkillBorderImage.color = new Color(0.6f, 0.6f, 0.6f);
        SkillBorderTMP.text = keycode;
        startListen = true;
    }

    public void ResetSkill()
    {
        GetComponent<Image>().sprite = baseImage;
        AnchoredSkill.RebindKey("");
        AnchoredSkill = null;
    }

    void Update()
    {
        TriggerActive = (AnchoredSkill != null);
        
    }

    private void UpdateToolTipInfo()
    {
        // Update tooltip info
        header = AnchoredSkill.SkillName + " [" + keycode + "]";
        body = AnchoredSkill.SkillDescription;
        SkillBorderImage.color = new Color(1.0f, 1.0f, 1.0f);
        SkillBorderTMP.text = "";
    }

    public void OnGUI() //keybind listener
    {
        if (this.startListen)
        {
            Event events = Event.current;
            if (events.isKey && events.keyCode.ToString() != "LeftAlt")
            {
                string key = events.keyCode.ToString();
                SkillBorderTMP.text = key;
                //skill1_text.text = key;
                keycode = key;
                AnchoredSkill.RebindKey("<Keyboard>/" + keycode);
                UpdateToolTipInfo();
                this.startListen = false;
            }
            else if (events.isMouse)
            {
                string mouse = events.button.ToString();
                switch (mouse)
                {
                    case "0":
                        AnchoredSkill.RebindKey("<Mouse>/leftButton");
                        keycode = "LMB";
                        break;
                    case "1":
                        AnchoredSkill.RebindKey("<Mouse>/rightButton");
                        keycode = "RMB";
                        break;
                    default:
                        AnchoredSkill.RebindKey("<Mouse>/leftButton");
                        keycode = "LMB";
                        break;

                }
                AnchoredSkill.RebindKey(mouse);
                UpdateToolTipInfo();
                this.startListen = false;
            }
        }

    }

}

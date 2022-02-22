using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponVisibility : MonoBehaviour
{
    private MeshRenderer m_Renderer;
    public bool visible;
    public float visiblitySpeed = 1f;

    void Awake()
    {
        m_Renderer = GetComponent<MeshRenderer>();
        foreach (Material m in m_Renderer.materials)
        {
            m.SetFloat("_CutoffHeight", visible ? 1.8f : -1f);
        }
    }

    void Update()
    {
        foreach (Material m in m_Renderer.materials) {
            float currentCutoff = m.GetFloat("_CutoffHeight");
            float newCutoff = visible ? currentCutoff + Time.deltaTime * visiblitySpeed : currentCutoff - Time.deltaTime * visiblitySpeed;
            newCutoff = Mathf.Clamp(newCutoff, -1f, 1.8f);
            m.SetFloat("_CutoffHeight", newCutoff);
        }
    }

    public void SetVisible(bool visibility)
    {
        visible = visibility;
    }
}

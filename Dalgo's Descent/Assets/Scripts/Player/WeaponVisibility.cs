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
        visible = true;
        m_Renderer = GetComponent<MeshRenderer>();
    }

    void Update()
    {
        foreach (Material m in m_Renderer.materials) {
            float currentCutoff = m.GetFloat("_CutoffHeight");
            float newCutoff = visible ? currentCutoff + Time.deltaTime * visiblitySpeed : currentCutoff - Time.deltaTime * visiblitySpeed;
            Debug.Log(newCutoff);
            newCutoff = Mathf.Clamp(newCutoff, -1f, 1.2f);
            m.SetFloat("_CutoffHeight", newCutoff);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class DynamicCamera : MonoBehaviour
{
    [Header("Adjustable variables")]
    [SerializeField] float m_NonCombatDistance = 5f;
    [SerializeField] float m_CombatDistance = 10f;
    [SerializeField] float m_LerpSpd = 0.05f;

    protected CinemachineFreeLook m_Camera;

    protected float m_CurrDistance = 5f;
    protected float m_ScaleTop = 1f;
    protected float m_ScaleBot = 1f;

    private void Awake()
    {
        m_ScaleTop = 4f / 5f;
        m_ScaleBot = 1.3f / 5f;
    }

    // Start is called before the first frame update
    void Start()
    {
        m_Camera = GetComponent<CinemachineFreeLook>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameManager.Instance)
            return;

        //Update camera
        if (GameManager.Instance.GetInCombat() ? m_CurrDistance != m_CombatDistance : m_CurrDistance != m_NonCombatDistance)
            UpdateCameraRadius();
    }

    protected void UpdateCameraRadius()
    {
        float targetDist = GameManager.Instance.GetInCombat() ? m_CombatDistance : m_NonCombatDistance;
        m_CurrDistance = Mathf.Lerp(m_CurrDistance, targetDist, m_LerpSpd);

        if (Mathf.Abs(targetDist - m_CurrDistance) < 0.01f)
            m_CurrDistance = targetDist;

        //Set camera distance as per current distance
        m_Camera.m_Orbits[0].m_Radius = m_CurrDistance * m_ScaleTop; //Top
        m_Camera.m_Orbits[1].m_Radius = m_CurrDistance; //Middle
        m_Camera.m_Orbits[2].m_Radius = m_CurrDistance * m_ScaleBot; //Bottom
    }
}

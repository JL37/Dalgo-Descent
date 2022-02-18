using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class DynamicCamera : MonoBehaviour
{
    [Header("Adjustable variables")]
    [SerializeField] float m_NonCombatDistance = 5f;
    [SerializeField] float m_CombatDistance = 10f;
    [SerializeField] float m_LerpSpd = 0.25f;

    protected CinemachineFreeLook m_Camera;

    protected float m_CurrDistance = 5f;
    protected float m_ScaleTop = 1f;
    protected float m_BotTop = 1f;

    protected bool m_InCombat = false;

    private void Awake()
    {
        m_ScaleTop = 4f / 5f;
        m_BotTop = 1.3f / 5f;
    }

    // Start is called before the first frame update
    void Start()
    {
        m_Camera = GetComponent<CinemachineFreeLook>();
    }

    // Update is called once per frame
    void Update()
    {
        bool reachedCombat = m_InCombat && m_CurrDistance == m_CombatDistance;
        bool reachedNonCombat = !m_InCombat && m_CurrDistance == m_NonCombatDistance;

        if (!reachedCombat || !reachedNonCombat)
            UpdateCameraRadius();
    }

    protected void UpdateCameraRadius()
    {
        
    }

    public void SetInCombat(bool combat) { m_InCombat = combat; }
}

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

    protected float currDistance = 5f;
    protected float scaleTop = 1f;
    protected float botTop = 1f;

    protected bool m_InCombat = false;

    private void Awake()
    {
        scaleTop = 4f / 5f;
        botTop = 1.3f / 5f;
    }

    // Start is called before the first frame update
    void Start()
    {
        m_Camera = GetComponent<CinemachineFreeLook>();
    }

    // Update is called once per frame
    void Update()
    {
        //Math
    }

    public void SetInCombat(bool combat) { m_InCombat = combat; }
}

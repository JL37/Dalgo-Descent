using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Animations.Rigging;

public class FieldOfView : MonoBehaviour
{
    public float m_radius;
    public float m_viewRadius;
    [Range(0,360)]
    public float m_angle;

    public GameObject m_playerRef;

    public bool m_canSeeTarget;

    void Start()
    {
        m_playerRef = GameObject.FindGameObjectWithTag("Player");
        StartCoroutine(DoFOVCheck());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator DoFOVCheck()
    {
        WaitForSeconds wait = new WaitForSeconds(0.2f);

        while (true)
        {
            yield return wait;
            m_canSeeTarget = CanSeeTarget(transform, m_playerRef.transform, m_angle, m_viewRadius);
        }
    }

    private bool CanSeeTarget(Transform enemy, Transform target, float viewAngle, float viewRange)
    {
        Vector3 toTarget = target.position - enemy.transform.position;
        if (toTarget.sqrMagnitude < m_radius * m_radius)
            return true;

        if (Vector3.Angle(enemy.transform.forward, toTarget) <= viewAngle)
        {
            if (Physics.Raycast(enemy.transform.position, toTarget, out RaycastHit hit, viewRange))
            {
                if (hit.transform.root == target)
                    return true;
            }
        }
        return false;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{
    public float m_radius;
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
            m_canSeeTarget = CanSeeTarget(transform, m_playerRef.transform, m_angle, m_radius);
        }
    }

    private bool CanSeeTarget(Transform enemy, Transform target, float viewAngle, float viewRange)
    {
        Vector3 toTarget = target.position - enemy.transform.position;
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

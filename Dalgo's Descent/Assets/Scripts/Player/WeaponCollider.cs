using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponCollider : MonoBehaviour
{
    protected List<Collider> m_collisionEvents;

    private void Awake()
    {
        m_collisionEvents = new List<Collider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        m_collisionEvents.Add(other);
    }

    private void OnTriggerExit(Collider other)
    {
        m_collisionEvents.Remove(other);
    }

    public List<Collider> collisionEvents
    {
        get { return m_collisionEvents; }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossGroundWaveCollision : MonoBehaviour
{
    public ParticleSystem part;
    public List<ParticleCollisionEvent> collisionEvents;

    void Start()
    {
        part = GetComponent<ParticleSystem>();
        collisionEvents = new List<ParticleCollisionEvent>();
    }

    void OnParticleCollision(GameObject other)
    {
        int numCollisionEvents = part.GetCollisionEvents(other, collisionEvents);

        int i = 0;

        while (i < numCollisionEvents)
        {
            if (other.gameObject.tag == "Player")
            {
                Vector3 direction = (other.transform.position - part.transform.position).normalized;
                other.gameObject.GetComponent<PlayerController>().AddImpact(new Vector3(direction.x, direction.y, direction.z).normalized, 40f);
                Debug.Log("Player Hit");
            }
            i++;

        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossGroundWaveCollision : MonoBehaviour
{
    public ParticleSystem part;
    public List<ParticleCollisionEvent> collisionEvents;
    private static bool hasBeenHitBySmash = false;
    private float m_timeElapsed;

    public BossAI boss;

    void Start()
    {
        m_timeElapsed = 0f;
        part = GetComponent<ParticleSystem>();
        collisionEvents = new List<ParticleCollisionEvent>();
    }

    private void Update()
    {
        if (hasBeenHitBySmash)
        {
            m_timeElapsed += Time.deltaTime;
            if (m_timeElapsed > 3f)
            {
                hasBeenHitBySmash = false;
                m_timeElapsed = 0f;
            }
        }
    }

    void OnParticleCollision(GameObject other)
    {
        int numCollisionEvents = part.GetCollisionEvents(other, collisionEvents);

        int i = 0;

        while (i < numCollisionEvents)
        {
            if (other.gameObject.tag == "Player" && !hasBeenHitBySmash)
            {
                hasBeenHitBySmash = true;
                Vector3 direction = (other.transform.position - part.transform.position).normalized;
                other.gameObject.GetComponent<PlayerController>().AddImpact(new Vector3(direction.x, direction.y, direction.z).normalized, 40f);
                other.gameObject.GetComponent<PlayerStats>().Received_Damage((int)(boss.enemyStats.FinalDamage() * boss.groundSlamModifier));
                Debug.Log("Player Hit");
            }
            i++;

        }
    }
}

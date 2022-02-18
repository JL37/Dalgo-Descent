using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Animations.Rigging;

[RequireComponent(typeof(NavMeshAgent))]
[DefaultExecutionOrder(1)]
public class BossAI : MonoBehaviour
{
    [Header("References")]
    public GameObject playerRef;
    public NavMeshAgent agent;
    public Collider damageCollider;
    public Health health;
    public Vector3 targetPoint = new Vector3();

    [Header("Throwable Object Attack")]
    public GameObject[] woodPrefabs;
    public Transform projectileHolder;
    public Transform projectileThrownHolder;

    [Header("Ground Slam Attack")]
    public Transform centerOfRoom;
    public ParticleSystem shockwaveParticleSystem;

    [Header("Animation")]
    public Animator animator;
    public Rig rig;


    [HideInInspector] public bool m_inAttackRange = false;
    [HideInInspector] public float m_bossTimer;
    public float m_bossAttackIntervals;
    private bool m_rigActive = true;

    void Awake()
    {
        playerRef = GameObject.FindGameObjectWithTag("Player");
        health = GetComponent<Health>();
    }

    public void AttackPlayer()
    {
        if (m_inAttackRange)
        {
            Debug.Log("Player Hit");
        }
    }

    public void MoveTo(Vector3 position)
    {
        targetPoint = position;
        agent.SetDestination(targetPoint);
    }

    void Update()
    {
        rig.weight = m_rigActive ? rig.weight + Time.deltaTime * 2f : rig.weight - Time.deltaTime * 2f;
        if (Input.GetKeyDown(KeyCode.O))
        {
            Damage(10);
        }
    }
    public void Damage(float amount)
    {
        if (health.currentHealth <= 0)
            return;

        health.TakeDamage(amount);
    }

    public void Die()
    {
        health.DieAnimation();
    }

    public void GrabWood()
    {
        Instantiate(woodPrefabs[Random.Range(0, woodPrefabs.Length - 1)], projectileHolder);
    }

    public void TossWood()
    {
        Transform projectile = projectileHolder.GetChild(0);
        projectile.parent = projectileThrownHolder;
        projectile.GetComponent<Projectile>().directionVelocity = (playerRef.transform.position - transform.position).normalized;
    }

    public void GroundSlam()
    {
        shockwaveParticleSystem.Play(); 
        // shake camera or smth
    }

    public void SetRigActive(bool active)
    {
        m_rigActive = active;
    }
}

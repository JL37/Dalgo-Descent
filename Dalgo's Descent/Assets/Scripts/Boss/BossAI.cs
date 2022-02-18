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

    [Header("Attack Choice")]
    [HideInInspector] public int attackChoice;

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

    private GameManager m_GameManager;
    private bool m_rigActive = true;
    private bool m_AggroActivated = false;

    void Awake()
    {
        playerRef = GameObject.FindGameObjectWithTag("Player");
        health = GetComponent<Health>();
        // m_GameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
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

        //Check for any signs of aggression, if have then bruh update the gamemanager
        if (!m_AggroActivated && IsAggro())
        {
            //Add to the gamemanager to say got enemy here
            m_AggroActivated = true;
            // m_GameManager.AddToEnemyArray(gameObject);
        }
    }

    public bool IsAggro()
    {
        return !animator.GetCurrentAnimatorStateInfo(0).IsName("Idle") && !animator.GetCurrentAnimatorStateInfo(0).IsName("Spawn");
    }

    public void Damage(float amount)
    {
		if (health.currentHealth <= 0)
			return;
		
        if (!m_AggroActivated)
        {
            //Add to the gamemanager to say got enemy here
            m_AggroActivated = true;
            // m_GameManager.AddToEnemyArray(gameObject);
        }

        health.TakeDamage(amount);

        //if (health.currentHealth <= 0)
            // m_GameManager.RemoveFromEnemyArray(gameObject);
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
        projectile.GetComponent<Projectile>().directionVelocity = (new Vector3(playerRef.transform.position.x, 0.5f, playerRef.transform.position.z) - projectile.transform.position).normalized;
    }

    public void GroundSlam()
    {
        shockwaveParticleSystem.Play(); 
        // shake camera or smth
    }

    public void ChooseAttack()
    {
        attackChoice = (health.currentHealth < health.maxHealth * 0.5) ? Random.Range(1, 5) : Random.Range(1, 3);
        attackChoice = 2;
        Debug.Log(attackChoice);
    }

    public void SetRigActive(bool active)
    {
        m_rigActive = active;
    }
}

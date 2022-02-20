using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Animations.Rigging;

[RequireComponent(typeof(NavMeshAgent))]
[DefaultExecutionOrder(1)]
public class BossAI : AI
{
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
    public Rig rig;
    private bool m_rigActive = true;

    [HideInInspector] public float m_bossTimer;
    public float m_bossAttackIntervals;
    

    protected override void Awake()
    {
        base.Awake();
        // m_GameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    protected override void Update()
    {
        base.Update();

        rig.weight = m_rigActive ? rig.weight + Time.deltaTime * 2f : rig.weight - Time.deltaTime * 2f;
        if (Input.GetKeyDown(KeyCode.O))
        {
            Damage(10);
        }
    }

    public override void Damage(float amount)
    {
		if (health.currentHealth <= 0)
			return;
		
        if (!aggroActivated)
        {
            //Add to the gamemanager to say got enemy here
            aggroActivated = true;
            // m_GameManager.AddToEnemyArray(gameObject);
        }

        health.TakeDamage(amount);

        //if (health.currentHealth <= 0)
            // m_GameManager.RemoveFromEnemyArray(gameObject);
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
        attackChoice = 3;
        Debug.Log(attackChoice);
    }

    public void SetRigActive(bool active)
    {
        m_rigActive = active;
    }

    public override bool IsAggro()
    {
        return !animator.GetCurrentAnimatorStateInfo(0).IsName("Idle") && !animator.GetCurrentAnimatorStateInfo(0).IsName("Spawn");
    }

}

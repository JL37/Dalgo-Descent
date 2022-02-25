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
    public float bossAttackIntervals;

    [Header("Boss Attack Modifiers")]
    public float woodThrowModifier = 1f;
    public float groundSlamModifier = 1f;

    protected override void Awake()
    {
        base.Awake();
        aiType = AI_TYPE.AI_TYPE_BOSS;
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
		if (enemyStats.health.currentHealth <= 0)
			return;
		
        if (!aggroActivated)
            AddAggroToGameManager();

        enemyStats.health.TakeDamage(amount);
        PostGameInfo.GetInstance().UpdateDamage((int)amount);

        if (enemyStats.health.currentHealth <= 0)
        {
            PostGameInfo.GetInstance().UpdateEnemy(true);
            RemoveFromGameManager();
        }
    }

    public void GrabWood()
    {
        Instantiate(woodPrefabs[Random.Range(0, woodPrefabs.Length - 1)], projectileHolder).GetComponent<Projectile>().Init((int)(enemyStats.FinalDamage() * woodThrowModifier));
    }

    public void TossWood()
    {
        Transform projectile = projectileHolder.GetChild(0);
        projectile.parent = projectileThrownHolder;
        projectile.GetComponent<Projectile>().directionVelocity = (new Vector3(playerRef.transform.position.x, transform.position.y, playerRef.transform.position.z) - projectile.transform.position).normalized;
    }

    public void GroundSlam()
    {
        shockwaveParticleSystem.Play(); 
        // shake camera or smth
    }

    public void ChooseAttack()
    {
        attackChoice = (enemyStats.health.currentHealth < enemyStats.health.maxHealth * 0.5) ? Random.Range(1, 5) : Random.Range(1, 3);
        // attackChoice = 3;
        Debug.Log(attackChoice);
    }

    public void SetRigActive(bool active)
    {
        m_rigActive = active;
    }

    protected override void AddAggroToGameManager()
    {
        base.AddAggroToGameManager();
        GameManager.Instance.EnableBossHealthUI(GetComponent<Health>());
    }

    protected override void RemoveFromGameManager()
    {
        base.RemoveFromGameManager();
        GameManager.Instance.DisableBossHealthUI();
    }

    public override bool IsAggro()
    {
        return !animator.GetCurrentAnimatorStateInfo(0).IsName("Idle") && !animator.GetCurrentAnimatorStateInfo(0).IsName("Spawn");
    }

}

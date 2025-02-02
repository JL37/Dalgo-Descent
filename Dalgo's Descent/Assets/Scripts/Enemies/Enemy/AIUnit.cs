using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[DefaultExecutionOrder(1)]
public class AIUnit : AI
{
    public bool isMiniboss;

    private float enemySize = 1f;

    private new Rigidbody rigidbody;
    public LayerMask groundLayer;
    public Texture2D[] enemyTextures;
    SkinnedMeshRenderer[] mr;

    //Reference
    private EnemyHealthUI m_HealthUI;

    [Header("Sounds")]
    public AudioSource audioSource;
    public AudioClip hurt;
    public AudioClip attack;

    protected override void Awake()
    {
        base.Awake();

        audioSource = GetComponent<AudioSource>();
        aiType = AI_TYPE.AI_TYPE_ENEMY;
        rigidbody = GetComponent<Rigidbody>();

        mr = GetComponentsInChildren<SkinnedMeshRenderer>();
        int randomSkin = Random.Range(0, enemyTextures.Length);
        foreach (SkinnedMeshRenderer smr in mr)
        {
            smr.materials[0].SetFloat("_CutoffHeight", -1);
            smr.materials[0].SetTexture("_DiffuseTexture", enemyTextures[randomSkin]);
        }
    }

    private void Start()
    {
        
    }

    public void Init(float size, float strength, bool isMiniboss) 
    {
        enemySize = size;
        this.isMiniboss = isMiniboss;
        aiStrength = strength;
        transform.localScale = new Vector3(size, size, size);
        enemyStats.Init(strength);
     
    }

    protected override void Update()
    {
        //Debug.Log(IsAggro());
        base.Update();
        foreach (SkinnedMeshRenderer smr in mr)
        {
            smr.materials[0].SetFloat("_CutoffHeight", smr.materials[0].GetFloat("_CutoffHeight") + Time.deltaTime * 2f);
            smr.materials[0].SetFloat("_CutoffHeight", Mathf.Clamp(smr.materials[0].GetFloat("_CutoffHeight"), -1f, 3f));
        }

#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.P))
        {
            EnemyKnockup(20);
        }

        if (Input.GetKeyDown(KeyCode.O))
        {
            EnemyHit(200, 100f);
        }
#endif
    }

    public float GetSize() { return enemySize; }

    public override void Damage(float amount)
    {
        audioSource.PlayOneShot(hurt, 1f);

        animator.speed = 1f;
        rigidbody.isKinematic = false;
        agent.enabled = false;
        if (rigidbody.velocity.y < -100)
        {
            rigidbody.velocity = Vector3.zero;
        }

        if (!aggroActivated)
            AddAggroToGameManager();

        enemyStats.health.TakeDamage(amount);
        PostGameInfo.GetInstance().UpdateDamage((int)amount);

        if (enemyStats.health.currentHealth <= 0)
        {
            PostGameInfo.GetInstance().UpdateEnemy(isMiniboss);
            GetComponent<Collider>().enabled = false;
            rigidbody.isKinematic = true;
            RemoveFromGameManager();
        }
    }

    public override void AttackPlayer()
    {
        base.AttackPlayer();
        if (m_inAttackRange && playerRef)
        {
            audioSource.PlayOneShot(attack, 1f);
        }
    }

    public void EnemyHit(float damage, float force) 
    {
        if (enemyStats.health.currentHealth <= 0.0f)
            return;

        Damage(damage);
        animator.SetTrigger("Hit");
        // m_animator.SetBool("IsHit", true);
        Vector3 directionFromPlayer = Vector3.Normalize(transform.position - playerRef.transform.position);
        rigidbody.AddForce(directionFromPlayer * force);
    }

    public void EnemyKnockup(float damage)
    {
        if (animator.GetBool("IsAirborne"))
            return;

        Damage(damage);
        rigidbody.isKinematic = false;
        agent.enabled = false;
        rigidbody.velocity = Vector3.zero;
        rigidbody.AddForce(new Vector3(0, 500, 0));
        if (enemyStats.health.currentHealth <= 0)
            return;

        animator.SetTrigger("Knockup");
    }

    public override bool IsAggro()
    {
        return animator.GetCurrentAnimatorStateInfo(0).IsName("PreparingAttack") || animator.GetCurrentAnimatorStateInfo(0).IsName("Attacking") || animator.GetCurrentAnimatorStateInfo(0).IsName("Hit") || animator.GetCurrentAnimatorStateInfo(0).IsName("Knockup") || animator.GetCurrentAnimatorStateInfo(0).IsName("Pushback");
    }



    protected override void AddAggroToGameManager()
    {
        base.AddAggroToGameManager();
        if (isMiniboss)
            GameManager.Instance.EnableBossHealthUI(GetComponent<Health>());
        else
            m_HealthUI = GameManager.Instance.ActivateEnemyHealthUI(GetComponent<Health>());
    }

    protected override void RemoveFromGameManager()
    {
        base.RemoveFromGameManager();

        //Basic enemy: 1-2 coins, boss : 3-5 coins
        if (isMiniboss)
        {
            GameManager.Instance.DisableBossHealthUI();
            Factory.CreateCoins(transform.position, Random.Range(3, 6));
        }
        else
        {
            Factory.CreateCoins(transform.position, Random.Range(1, 3));
            m_HealthUI.StartFadeAnimation(true);
        }
    }
}
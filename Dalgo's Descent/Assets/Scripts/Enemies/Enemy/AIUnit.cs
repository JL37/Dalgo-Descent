using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[DefaultExecutionOrder(1)]
public class AIUnit : AI
{
    public bool isMiniboss;

    private new Rigidbody rigidbody;
    public LayerMask groundLayer;
    public Texture2D[] enemyTextures;
    SkinnedMeshRenderer[] mr;

    //Reference
    private EnemyHealthUI m_HealthUI;

    protected override void Awake()
    {
        base.Awake();
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

    public void Init(float strength) 
    {
        transform.localScale = new Vector3(strength, strength, strength);
        enemyStats.Init(strength);
    }

    protected override void Update()
    {
        //Debug.Log(IsAggro());
        base.Update();
        foreach (SkinnedMeshRenderer smr in mr)
        {
            smr.materials[0].SetFloat("_CutoffHeight", smr.materials[0].GetFloat("_CutoffHeight") + Time.deltaTime * 2f);
            smr.materials[0].SetFloat("_CutoffHeight", Mathf.Clamp(smr.materials[0].GetFloat("_CutoffHeight"), -1f, 1.8f));
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            EnemyKnockup(20);
        }

        if (Input.GetKeyDown(KeyCode.O))
        {
            EnemyHit(10, 100f);
        }
    }

    public override void Damage(float amount)
    {
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
        if (enemyStats.health.currentHealth <= 0)
        {
            PostGameInfo.GetInstance().UpdateEnemy(isMiniboss);
            RemoveFromGameManager();
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
        if (isMiniboss)
            GameManager.Instance.DisableBossHealthUI();
        else
            m_HealthUI.StartFadeAnimation(true);
    }
}
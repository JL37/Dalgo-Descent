using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[DefaultExecutionOrder(1)]
public class AIUnit : AI
{
    private new Rigidbody rigidbody;
    public LayerMask groundLayer;
    public Texture2D[] enemyTextures;
    SkinnedMeshRenderer[] mr;
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

    protected override void Update()
    {
        //Debug.Log(IsAggro());
        base.Update();
        foreach (SkinnedMeshRenderer smr in mr)
        {
            smr.materials[0].SetFloat("_CutoffHeight", smr.materials[0].GetFloat("_CutoffHeight") + Time.deltaTime * 2f);
            smr.materials[0].SetFloat("_CutoffHeight", Mathf.Clamp(smr.materials[0].GetFloat("_CutoffHeight"), -1f, 1.4f));
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            EnemyKnockup(20);
        }

        if (Input.GetKeyDown(KeyCode.O))
        {
            EnemyHit(10);
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
            RemoveFromGameManager();
    }

    public void EnemyHit(float damage) 
    {
        if (enemyStats.health.currentHealth <= 0.0f)
            return;

        animator.SetTrigger("Hit");
        // m_animator.SetBool("IsHit", true);
        Damage(damage);
        Vector3 directionFromPlayer = Vector3.Normalize(transform.position - playerRef.transform.position);
        rigidbody.AddForce(directionFromPlayer * 100f);
    }

    public void EnemyKnockup(float damage)
    {
        if (animator.GetBool("IsAirborne") || enemyStats.health.currentHealth <= 0)
            return;

        Damage(damage);
        animator.speed = 1f;
        animator.SetTrigger("Knockup");
        rigidbody.isKinematic = false;
        agent.enabled = false;
        rigidbody.velocity = Vector3.zero;
        rigidbody.AddForce(new Vector3(0, 500, 0));
    }

    public override bool IsAggro()
    {
        return animator.GetCurrentAnimatorStateInfo(0).IsName("PreparingAttack") || animator.GetCurrentAnimatorStateInfo(0).IsName("Attacking") || animator.GetCurrentAnimatorStateInfo(0).IsName("Hit") || animator.GetCurrentAnimatorStateInfo(0).IsName("Knockup");
    }
}
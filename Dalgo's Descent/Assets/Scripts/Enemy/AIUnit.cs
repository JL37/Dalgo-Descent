using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[DefaultExecutionOrder(1)]
public class AIUnit : MonoBehaviour
{
    [Header("Objects and variables")]
    public Animator animator;
    public GameObject playerRef;
    public Rigidbody rigidbody;
    public NavMeshAgent agent;

    public Vector3 targetPoint = new Vector3();
    public LayerMask groundLayer;

    public Collider damageCollider;
    public bool inAttackRange = false;

    [Header("Prefabs")]
    [SerializeField] GameObject damageTextPrefab;

    private Health m_Health;
    private GameManager m_GameManager;
    private bool aggroActivated = false;

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        m_Health = GetComponent<Health>();
        rigidbody = GetComponent<Rigidbody>();
        agent = GetComponent<NavMeshAgent>();
        playerRef = GameObject.FindGameObjectWithTag("Player");
        m_GameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    private void Start()
    {

    }

    public void Update()
    {
        //Debug.Log(IsAggro());

        if (Input.GetKeyDown(KeyCode.P))
        {
            EnemyKnockup();
        }

        if (Input.GetKeyDown(KeyCode.O))
        {
            EnemyHit();
        }

        if (!aggroActivated && IsAggro())
        {
            //Add to the gamemanager to say got enemy here
            aggroActivated = true;
            m_GameManager.AddToEnemyArray(gameObject);
        }
    }

    public void MoveTo(Vector3 Position)
    {
        agent.SetDestination(Position);
    }

    public void AttackPlayer()
    {
        if (inAttackRange)
        {
            Debug.Log("Player Hit");
        }
    }

    public void Damage(float amount)
    {
        animator.speed = 1f;
        rigidbody.isKinematic = false;
        agent.enabled = false;
        // m_aiUnit.m_rigidbody.velocity = Vector3.zero;

        if (!aggroActivated)
        {
            //Add to the gamemanager to say got enemy here
            aggroActivated = true;
            m_GameManager.AddToEnemyArray(gameObject);
        }

        m_Health.TakeDamage(amount);
        if (m_Health.currentHealth <= 0)
            m_GameManager.RemoveFromEnemyArray(gameObject);
    }

    public void EnemyHit(/*Skill enum or smth idk*/)
    {
        if (m_Health.currentHealth <= 0.0f)
            return;

        animator.SetTrigger("Hit");
        // m_animator.SetBool("IsHit", true);
        Damage(10);
        Vector3 directionFromPlayer = Vector3.Normalize(transform.position - playerRef.transform.position);
        rigidbody.AddForce(directionFromPlayer * 100f);
    }

    public void EnemyKnockup()
    {
        if (animator.GetBool("IsAirborne"))
            return;

        Damage(10);
        animator.speed = 1f;
        animator.SetTrigger("Knockup");
        rigidbody.isKinematic = false;
        agent.enabled = false;
        rigidbody.velocity = Vector3.zero;
        rigidbody.AddForce(new Vector3(0, 500, 0));
    }

    public void Die()
    {
        m_Health.DieAnimation();
    }

    public bool IsAggro()
    {
        return animator.GetCurrentAnimatorStateInfo(0).IsName("PreparingAttack") || animator.GetCurrentAnimatorStateInfo(0).IsName("Attacking") || animator.GetCurrentAnimatorStateInfo(0).IsName("Hit") || animator.GetCurrentAnimatorStateInfo(0).IsName("Knockup");
    }
}
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[DefaultExecutionOrder(1)]
public class AIUnit : MonoBehaviour
{
    [Header("Objects and variables")]
    public Animator m_animator;
    public GameObject m_playerRef;
    public Rigidbody m_rigidbody;
    public NavMeshAgent m_agent;

    public Vector3 m_targetPoint = new Vector3();
    public LayerMask m_groundLayer;

    public Collider m_damageCollider;
    public bool m_inAttackRange = false;

    [Header("Prefabs")]
    [SerializeField] GameObject m_damageTextPrefab;

    private Health m_Health;
    private GameManager m_GameManager;
    private bool aggroActivated = false;

    private void Awake()
    {
        m_animator = GetComponentInChildren<Animator>();
        m_Health = GetComponent<Health>();
<<<<<<< HEAD
        rigidbody = GetComponent<Rigidbody>();
        agent = GetComponent<NavMeshAgent>();
        playerRef = GameObject.FindGameObjectWithTag("Player");
        m_GameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
=======
        m_rigidbody = GetComponent<Rigidbody>();
        m_agent = GetComponent<NavMeshAgent>();
        m_playerRef = GameObject.FindGameObjectWithTag("Player");
>>>>>>> parent of 19d8647 (Merge branch 'main' into BasicUI-and-SceneSetup)
    }

    private void Start()
    {
        
    }

    public void Update()
    {
<<<<<<< HEAD
        //Debug.Log(IsAggro());

=======
>>>>>>> parent of 19d8647 (Merge branch 'main' into BasicUI-and-SceneSetup)
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
        m_agent.SetDestination(Position);
    }

    public void AttackPlayer()
    {
        if (m_inAttackRange)
        {
            Debug.Log("Player Hit");
        }
    }

    public void Damage(float amount)
    {
        m_animator.speed = 1f;
        m_rigidbody.isKinematic = false;
        m_agent.enabled = false;
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

        m_animator.SetTrigger("Hit");
        // m_animator.SetBool("IsHit", true);
        Damage(10);
        Vector3 directionFromPlayer = Vector3.Normalize(transform.position - m_playerRef.transform.position);
        m_rigidbody.AddForce(directionFromPlayer * 100f);
    }

    public void EnemyKnockup()
    {
        if (m_animator.GetBool("IsAirborne"))
            return;

        Damage(10);
        m_animator.speed = 1f;
        m_animator.SetTrigger("Knockup");
        m_rigidbody.isKinematic = false;
        m_agent.enabled = false;
        m_rigidbody.velocity = Vector3.zero;
        m_rigidbody.AddForce(new Vector3(0, 500, 0));
    }

    public void Die()
    {
        m_Health.DieAnimation();
    }
<<<<<<< HEAD

    public bool IsAggro()
    {
        return animator.GetCurrentAnimatorStateInfo(0).IsName("PreparingAttack") || animator.GetCurrentAnimatorStateInfo(0).IsName("Attacking") || animator.GetCurrentAnimatorStateInfo(0).IsName("Hit") || animator.GetCurrentAnimatorStateInfo(0).IsName("Knockup");
    }
=======
>>>>>>> parent of 19d8647 (Merge branch 'main' into BasicUI-and-SceneSetup)
}
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
    private ObjectPoolManager m_UIPoolManager;

    private void Awake()
    {
        m_animator = GetComponentInChildren<Animator>();
        m_Health = GetComponent<Health>();
        m_rigidbody = GetComponent<Rigidbody>();
        m_agent = GetComponent<NavMeshAgent>();
        m_playerRef = GameObject.FindGameObjectWithTag("Player");
    }

    private void Start()
    {
        m_UIPoolManager = GameObject.FindGameObjectWithTag("HUD").GetComponent<GameUI>().GetObjectPoolManager();
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            EnemyKnockup();
        }

        if (Input.GetKeyDown(KeyCode.O))
        {
            EnemyHit();
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

    public void EnemyHit(/*Skill enum or smth idk*/) 
    {
        m_animator.SetTrigger("Hit");
        // m_animator.SetBool("IsHit", true);
        m_Health.TakeDamage(10);
		
        Vector3 directionFromPlayer = Vector3.Normalize(transform.position - m_playerRef.transform.position);
        m_rigidbody.AddForce(directionFromPlayer * 100f);

        SpawnText(10.ToString());
    }

    protected void SpawnText(string txt)
    {
        txt = "<color=red>" + txt + "</color>";
        GameObject obj = m_UIPoolManager.GetFromPool();

        //Initialisation
        obj.GetComponent<DamageTextUI>().Initialise(transform, txt, 1f);
    }

    public void EnemyKnockup()
    {
        if (m_animator.GetBool("IsAirborne"))
            return;

        m_Health.TakeDamage(10);
        m_animator.speed = 1f;
        m_animator.SetTrigger("Knockup");
        m_rigidbody.isKinematic = false;
        m_agent.enabled = false;
        m_rigidbody.velocity = Vector3.zero;
        m_rigidbody.AddForce(new Vector3(0, 500, 0));
    }
}
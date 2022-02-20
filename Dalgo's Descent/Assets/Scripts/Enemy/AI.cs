using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[DefaultExecutionOrder(1)]
public class AI : MonoBehaviour
{
    private GameManager m_GameManager;

    protected Animator m_Animator;
    protected Health m_Health;
    protected NavMeshAgent m_Agent;
    protected Vector3 m_TargetPosition;
    protected GameObject m_PlayerRef;
    public Collider damageCollider;

    [Header("Prefabs")]
    [SerializeField] GameObject damageTextPrefab;

    protected bool m_inAttackRange = false;
    protected bool m_AggroActivated = false;

    protected virtual void Awake()
    {
        m_Health = GetComponent<Health>();
        m_Animator = GetComponentInChildren<Animator>();
        m_Agent = GetComponent<NavMeshAgent>();
        m_PlayerRef = GameObject.FindGameObjectWithTag("Player");
        m_GameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        //Check for any signs of aggression, if have then bruh update the gamemanager
        if (!m_AggroActivated && IsAggro())
        {
            //Add to the gamemanager to say got enemy here
            m_AggroActivated = true;
            // m_GameManager.AddToEnemyArray(gameObject);
        }
    }

    public virtual bool IsAggro()
    {
        return false;
    }

    public virtual void Damage(float amount)
    {

    }

    public void MoveTo(Vector3 position)
    {
        m_TargetPosition = position;
        agent.SetDestination(m_TargetPosition);
    }

    public void AttackPlayer()
    {
        if (m_inAttackRange)
        {
            Debug.Log("Player Hit");
        }
    }

    public void Die()
    {
        health.DieAnimation();
    }


    public Animator animator { get { return m_Animator; } }
    public Health health { get { return m_Health; } }
    public NavMeshAgent agent { get { return m_Agent; } }
    public Vector3 targetPosition { 
        get { return m_TargetPosition; } 
        set { m_TargetPosition = value; } 
    }
    public GameObject playerRef { get { return m_PlayerRef; } }
    public bool inAttackRange { 
        get { return m_inAttackRange; } 
        set { m_inAttackRange = value;}
    }

    public GameManager gameManager { get { return m_GameManager; } }
    public bool aggroActivated { 
        get { return m_AggroActivated; } 
        set { m_AggroActivated = value; }
    }

}

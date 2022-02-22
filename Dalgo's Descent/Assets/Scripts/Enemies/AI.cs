using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(EnemyStats))]
[DefaultExecutionOrder(1)]
public class AI : MonoBehaviour
{
    public enum AI_TYPE
    {
        AI_TYPE_NONE = 0,
        AI_TYPE_ENEMY,
        AI_TYPE_BOSS,
    }

    public AI_TYPE aiType;
    //public EnemyHealthUI m_EnemyHealthUI;

    protected GameManager m_GameManager;

    protected EnemyStats m_EnemyStats;
    protected Animator m_Animator;
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
        aiType = AI_TYPE.AI_TYPE_NONE;
        m_EnemyStats = GetComponent<EnemyStats>();
        m_Animator = GetComponentInChildren<Animator>();
        m_Agent = GetComponent<NavMeshAgent>();
        m_PlayerRef = GameObject.FindGameObjectWithTag("Player");
        m_GameManager = GameObject.Find("GameManager").GetComponent<GameManager>();


    }

    // Update is called once per frame
    protected virtual void Update()
    {
        //Check for any signs of aggression, if have then bruh update the gamemanager
        if (!aggroActivated && IsAggro())
            AddAggroToGameManager();
    }

    protected virtual void AddAggroToGameManager()
    {
        //Add to the gamemanager to say got enemy here
        aggroActivated = true;
        m_GameManager.AddToEnemyArray(gameObject);
    }

    protected virtual void RemoveFromGameManager()
    {
        m_GameManager.RemoveFromEnemyArray(gameObject);
        //m_EnemyHealthUI.StartFadeAnimation(true);
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
            playerRef.GetComponent<PlayerStats>().Received_Damage(enemyStats.FinalDamage());
            Debug.Log("Player Hit");
        }
    }

    public void Die()
    {
        m_EnemyStats.health.DieAnimation();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Slash"))
        {
            if (aiType == AI_TYPE.AI_TYPE_ENEMY)
            {
                ((AIUnit)this).EnemyHit(playerRef.GetComponent<PlayerStats>().BasicAtk);
            }
            if (aiType == AI_TYPE.AI_TYPE_BOSS)
            {
                ((BossAI)this).Damage(playerRef.GetComponent<PlayerStats>().BasicAtk);
            }
        }
    }

    public EnemyStats enemyStats { get { return m_EnemyStats; } }
    public Animator animator { get { return m_Animator; } }
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

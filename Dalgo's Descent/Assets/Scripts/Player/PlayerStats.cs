using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [Header("Prefabs")]
    [SerializeField] GameObject m_ItemUIPrefab;

    //Stats
    public Health m_Health;

    protected int m_BaseHealth = 100;
    protected int m_HealthAdd = 0;

    protected float m_AtkSpd = 1f;
    protected float m_LifeSteal = 0f;

    protected int m_BaseBasicAtk = 15;
    protected int m_BasicAtkAddOn = 0;

    protected float m_DmgTakenMultiplier = 1f;

    protected float m_BaseCoolDown = 1f;
    protected float m_BaseSkillDmg = 1f;

    protected float m_MovementSpd = 1f;

    //Inventory
    protected int m_coin = 0;
    protected List<Chest> m_ChestArr;
    protected List<ItemUI> m_ItemArr;

    public event EventHandler onHealthChanged;

    //Event handler for player icon
    public event EventHandler onHealthy;
    public event EventHandler onHalfHealth;
    public event EventHandler onCriticalHealth;
    public event EventHandler onDead;

    //Player skill
    private PlayerSkills m_playerskills;

    [Header("Skill References")]
    public SkillObject CleaveSkill;
    public SkillObject ShovelCutSkill;
    public SkillObject SlamDunkSkill;

    private void Awake()
    {
        m_playerskills = new PlayerSkills(); //create a new instance of playerskill
        m_playerskills.OnSkillUnlocked += PlayerSkills_OnSkillUnlocked;
    }

    private void Start()
    {
        m_ItemArr = new List<ItemUI>();
        m_ChestArr = new List<Chest>();
        m_BaseHealth = (int)m_Health.maxHealth;
    }

    public void Received_Damage(int damageAmount)
    {
        if (m_Health.currentHealth <= 0.0f)
            return;

        //m_Health -= damageAmount;
        m_Health.TakeDamage(damageAmount);
        UpdatePlayerIcon();

        if (onHealthChanged != null)
            onHealthChanged(this, EventArgs.Empty);

    }
    public void Replenish_Health(int amount)
    {
        //m_Health += amount;
        m_Health.currentHealth += amount;
        UpdatePlayerIcon();
        //if (m_Health >= 100)
        if (m_Health.currentHealth >= 100)
        {
            //m_Health = 100;
            m_Health.currentHealth = 100;
        }
        if (onHealthChanged != null)
            onHealthChanged(this, EventArgs.Empty);

    }

    public void SetMaxHealth(int amount)
    {
        m_Health.maxHealth += amount;
        Debug.Log("Max health now is : " + m_Health.maxHealth);
    }


    public void UpdatePlayerIcon()
    {
        //if (m_Health > 50)
        if (m_Health.currentHealth > 50)
        {
            if (onHealthy != null)
                onHealthy(this, EventArgs.Empty);
        }
        //if (m_Health <= 50)
        if (m_Health.currentHealth <= 50)
        {
            if (onHalfHealth != null)
                onHalfHealth(this, EventArgs.Empty);

        }
        //if (m_Health <= 25)
        if (m_Health.currentHealth <= 25)
        {
            if (onCriticalHealth != null)
                onCriticalHealth(this, EventArgs.Empty);
        }
        //if (m_Health <= 0)
        if (m_Health.currentHealth <= 0)
        {
            m_Health.currentHealth = 0;
            if (onDead != null)
                onDead(this, EventArgs.Empty);
        }
    }

    #region OBSOLETE
    public bool CanUseSkill1()
    {
        return m_playerskills.IsSkillUnlocked(PlayerSkills.SkillType.Skill_1);
    }

    public bool CanUseSkill2()
    {
        return m_playerskills.IsSkillUnlocked(PlayerSkills.SkillType.Skill_2);
    }

    public bool CanUseSkill3()
    {
        return m_playerskills.IsSkillUnlocked(PlayerSkills.SkillType.Skill_3);
    }

    public bool CanUseSkill4()
    {
        return m_playerskills.IsSkillUnlocked(PlayerSkills.SkillType.Skill_4);
    }

    public PlayerSkills GetPlayerSkills()
    {
        return m_playerskills;
    }

    private void PlayerSkills_OnSkillUnlocked(object sender, PlayerSkills.OnSkillUnlockedEventArgs e)
    {
        switch(e.skilltype)
        {
            case PlayerSkills.SkillType.Health_Upgrade:
                SetMaxHealth(10);
                m_playerskills.RemoveSkill(e.skilltype);
                break;
        }
    }
    #endregion

    protected void AdjustHealth()
    {
        float ogMaxHealth = m_Health.maxHealth;
        m_Health.maxHealth = m_BaseHealth + m_HealthAdd;
        float ratio = m_Health.maxHealth / (float)ogMaxHealth;
        m_Health.currentHealth *= ratio;
    }

    public int CalculateHealthAdd(float perc) //Value from 0 to 1
    {
        return Mathf.RoundToInt(m_BaseHealth * perc);
    }

    public int BaseHealth
    {
        get { return m_BaseHealth; }
        set {
            m_BaseHealth = value;
            AdjustHealth();
        }
    }
    public int HealthAdd
    {
        get { return m_HealthAdd; }
        set { 
            m_HealthAdd = value;
            AdjustHealth();
        }
    }

    public float DamageTakenMultiplier
    {
        get { return m_DmgTakenMultiplier; }
        set { m_DmgTakenMultiplier = value; }
    }

    public float SkillCD
    {
        get { return m_BaseCoolDown; }
        set { m_BaseCoolDown = value; }
    }

    public float SkillDmg
    {
        get { return m_BaseSkillDmg; }
        set { m_BaseSkillDmg = value; }
    }

    public float AtkSpd
    {
        get { return m_AtkSpd; }
        set { m_AtkSpd = value; }
    }

    public float LifeSteal
    {
        get { return m_LifeSteal; }
        set { m_LifeSteal = value; }
    }

    public float MovementSpd
    {
        get { return m_MovementSpd; }
        set { m_MovementSpd = value; }
    }

    public int BasicAtk
    {
        get { return m_BaseBasicAtk + m_BasicAtkAddOn; }
    }

    public int BaseBasicAtk
    {
        get { return m_BaseBasicAtk; }
        set { m_BaseBasicAtk = value; }
    }

    public void AddPercToBasicAtk(float perc) //Must be 0 to 1
    {
        m_BasicAtkAddOn += Mathf.RoundToInt(m_BaseBasicAtk * perc);
    }

    public void AddToAllStats(float perc) //Must be between 0 to 1
    {
        m_HealthAdd += CalculateHealthAdd(perc);

        m_AtkSpd += perc;
        m_LifeSteal += perc;

        AddPercToBasicAtk(perc);

        m_DmgTakenMultiplier -= perc;

        m_BaseCoolDown -= perc;
        m_BaseSkillDmg += perc;

        m_MovementSpd += perc;
}

    public float GetHealthPerc()
    {
        //return m_Health / m_MaxHealth;
        return m_Health.currentHealth / m_Health.maxHealth;
    }

    //Coins bruh
    public void AddCoin(int num = 1)
    {
        m_coin += num;
    }

    public bool DeductCoin(int num = 1)
    {
        if (m_coin >= num)
        {
            m_coin -= num;
            return true;
        }

        return false;
    }

    //Chest variables bruh
    public void AddChest(Chest chest)
    {
        m_ChestArr.Add(chest);
    }

    public void RemoveChest(Chest chest)
    {
        for (int i = 0; i < m_ChestArr.Count; ++i)
        {
            if (m_ChestArr[i] == chest)
            {
                m_ChestArr.RemoveAt(i);
                break;
            }
        }
    }

    public Chest GetChest()
    {
        return m_ChestArr.Count > 0 ? m_ChestArr.Last() : null;
    }

    //Items la if not what?
    public void AddItem(Item item, bool animated = false)
    {
        print("Item added to inventory");

        //Animation (If needed)
        GameObject itemUI = Instantiate(m_ItemUIPrefab);
        itemUI.GetComponent<ItemUI>().Initialise(item, 0, animated);
        itemUI.transform.SetParent(GameObject.FindGameObjectWithTag("HUD").GetComponent<GameUI>().GetItemPanelTransform());

        //Shifts the existing items
        for (int i = 0; i < m_ItemArr.Count; ++i)
        {
            m_ItemArr[i].AddToIndex(1);
            m_ItemArr[i].UpdatePositionFromIndex();
        }

        item.AffectStats(this);
        m_ItemArr.Insert(0,itemUI.GetComponent<ItemUI>());
    }

    public int GetSlashDamage(SLASH_TYPE slashType)
    {
        switch (slashType)
        {
            case SLASH_TYPE.SLASH_1:
                return (int)(BasicAtk * 0.5f + ((LevelWindow.Instance.m_levelSystem.GetCurrentLevel() + 1) * 1.4f));
            case SLASH_TYPE.SLASH_2:
                return (int)(BasicAtk * 0.7f + ((LevelWindow.Instance.m_levelSystem.GetCurrentLevel() + 1) * 1.4f));
            case SLASH_TYPE.SLASH_3:
                return (int)(BasicAtk * 1.2f + ((LevelWindow.Instance.m_levelSystem.GetCurrentLevel() + 1) * 1.4f));
            case SLASH_TYPE.CLEAVE:
                return (int)(BasicAtk * 2.5f * CleaveSkill.CurrentSkillPoints + ((LevelWindow.Instance.m_levelSystem.GetCurrentLevel() + 1) * 1.4f));
            case SLASH_TYPE.SHOVEL_CUT:
                return (int)(BasicAtk * 2f * ShovelCutSkill.CurrentSkillPoints + ((LevelWindow.Instance.m_levelSystem.GetCurrentLevel() + 1) * 1.4f));
            case SLASH_TYPE.SLAM_DUNK:
                return (int)(BasicAtk * 3f * SlamDunkSkill.CurrentSkillPoints + ((LevelWindow.Instance.m_levelSystem.GetCurrentLevel() + 1) * 1.4f));
            default:
                return BasicAtk;
        }
    }

    public float GetKnockbackForce(SLASH_TYPE slashType)
    {
        switch (slashType)
        {
            case SLASH_TYPE.SLASH_1:
                return 70f;
            case SLASH_TYPE.SLASH_2:
                return 100f;
            case SLASH_TYPE.SLASH_3:
                return 150f;
            case SLASH_TYPE.CLEAVE:
                return 300f;
            default:
                return 100f;
        }
    }
}
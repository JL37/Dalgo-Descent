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

/*    protected float m_Health = 100;
    protected float m_MaxHealth = 100;*/

    protected float m_AtkSpd = 1f;
    protected float m_LifeSteal = 0f;
    protected float m_CritChance = 0.03f;
    protected int m_BasicAtk = 15;

    //Inventory
    protected int m_coin = 0;
    protected List<Chest> m_ChestArr;
    protected List<ItemUI> m_ItemArr;

    public event EventHandler onHealthChanged;

    public event EventHandler onHealthy;
    public event EventHandler onHalfHealth;
    public event EventHandler onCriticalHealth;
    public event EventHandler onDead;

    private void Start()
    {
        m_ItemArr = new List<ItemUI>();
        m_ChestArr = new List<Chest>();
    }

    public void Received_Damage(int damageAmount)
    {
        //m_Health -= damageAmount;
        m_Health.currentHealth -= damageAmount;
        UpdatePlayerIcon();
/*        if (m_Health.currentHealth <= 0)
        {
            m_Health = 0;
            m_Health.currentHealth = 0;
        }*/
        if (m_Health.currentHealth <= 0)
        {
            m_Health.currentHealth = 0;
        }
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



    public float Health
    {
        get { return m_Health.currentHealth; }
        set { m_Health.currentHealth = value; }
    }
    public float MaxHealth
    {
        get { return m_Health.currentHealth; }
        set { m_Health.currentHealth = value; }
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

    public float CritChance
    {
        get { return m_CritChance; }
        set { m_CritChance = value; }
    }

    public int BasicAtk
    {
        get { return m_BasicAtk; }
        set { m_BasicAtk = value; }
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
}
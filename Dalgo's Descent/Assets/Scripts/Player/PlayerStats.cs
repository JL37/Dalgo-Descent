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
    protected float m_Health = 100;
    protected float m_MaxHealth = 100;
    protected float m_AtkSpd = 1f;
    protected float m_LifeSteal = 0f;
    protected float m_CritChance = 0.03f;
    protected int m_BasicAtk = 15;

    //Inventory
    protected int m_coin = 0;
    protected List<Chest> m_ChestArr = new List<Chest>();
    protected List<Item> m_ItemArr = new List<Item>();

    public event EventHandler onHealthChanged;

    public void Damage(int damageAmount)
    {
        m_Health -= damageAmount;
        if (m_Health < 0)
            m_Health = 0;
        if (onHealthChanged != null)
            onHealthChanged(this, EventArgs.Empty);

    }

    public float Health
    {
        get { return m_Health; }
        set { m_Health = value; }
    }
    public float MaxHealth
    {
        get { return m_MaxHealth; }
        set { m_Health = value; }
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
        return m_Health / m_MaxHealth;
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
        for (int i = 0; i < m_ChestArr.Count;++i)
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
        if (m_ItemArr.Count < 16) //This part is only for the front part (Ofc, there will be more than 16 but they'll have to access the tab to see them all).
        {
            GameObject itemUI = Instantiate(m_ItemUIPrefab);
            itemUI.GetComponent<ItemUI>().Initialise(item, m_ItemArr.Count, animated);
            itemUI.transform.SetParent(GameObject.FindGameObjectWithTag("HUD").GetComponent<GameUI>().GetItemPanelTransform());
        }

        item.AffectStats(this);
        m_ItemArr.Add(item);
    }
}

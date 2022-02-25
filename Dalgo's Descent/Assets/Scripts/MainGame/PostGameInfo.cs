using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PostGameInfo
{
    protected static PostGameInfo m_Instance = null;

    protected float m_TimePassed = 0f;

    protected int m_MaxDmg = 0;
    protected int m_TotalDmgDealt = 0;

    protected int m_MoneyEarned = 0;
    protected int m_TotalItems = 0;

    protected int m_TotalEnemies = 0;
    protected int m_TotalBosses = 0;

    //Dialogue system
    protected int m_CurrIdx = 0;
    protected bool m_LoseFirstTime = true;

    public static PostGameInfo GetInstance()
    {
        if (null == m_Instance)
            m_Instance = new PostGameInfo();

        return m_Instance;
    }

    protected PostGameInfo()
    {
        //Doing nothing la bastard if not what
    }

    public int UpdateCurrIdx()
    {
        if (m_LoseFirstTime)
        {
            m_LoseFirstTime = false;
            return m_CurrIdx;
        }

        return m_CurrIdx < 5 ? ++m_CurrIdx : m_CurrIdx;
    }

    public void Reset()
    {
        m_TimePassed = 0f;
        m_MaxDmg = 0;
        m_TotalDmgDealt = 0;
        m_MoneyEarned = 0;
        m_TotalItems = 0;
        m_TotalEnemies = 0;
        m_TotalBosses = 0;
    }

    public void UpdateTime(float delta)
    {
        m_TimePassed += delta;
    }

    public void UpdateDamage(int currDmg)
    {
        if (currDmg > m_MaxDmg)
            m_MaxDmg = currDmg;

        m_TotalDmgDealt += currDmg;
    }

    public void UpdateMoney(int money)
    {
        m_MoneyEarned += money;
    }

    public void UpdateItem(int item)
    {
        m_TotalItems += item;
    }

    public void UpdateEnemy(bool isBoss)
    {
        m_TotalEnemies++;
        if (isBoss)
            m_TotalBosses++;
    }
}

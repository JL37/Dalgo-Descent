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
    protected int m_CurrIdx = 2;
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

    public string GetTimePassedText()
    {
        float seconds = (int)m_TimePassed % 60;
        float minutes = (int)m_TimePassed / 60;

        string str = "";

        str += minutes + (minutes == 1 ? " minute" : " minutes");
        str += " and " + seconds + (seconds == 1 ? " second" : " seconds");

        return str;
    }

    public int GetTotalDmg() { return m_TotalDmgDealt; }
    public int GetMaxDmg() { return m_MaxDmg; }

    public int GetTotalEnemies() { return m_TotalEnemies; }
    public int GetTotalBosses() { return m_TotalBosses; }

    public int GetTotalMoney() { return m_MoneyEarned; }
    public int GetTotalItems() { return m_TotalItems; }

    public static PostGameInfo ResetPostGameInfo()
    {
        m_Instance = null;
        return GetInstance();
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

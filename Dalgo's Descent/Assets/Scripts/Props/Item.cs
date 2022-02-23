using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Random = UnityEngine.Random;

public class Item
{
    public enum TYPE
    {
        F_COMMON = 0,
        CARROTHELMET,
        TIGERCLAW,
        FEATHERBOOT,
        F_UNCOMMON,
        VAMPIRICSCYTHE,
        BIRDARMOR,
        F_RARE,
        RUNICSCYTHE,
        CHRONOSCYTHE,
        F_EPIC,
        DOUBEEDGEDSCYTHE,
        F_LEGENDARY,
        ADDERALL,
        F_TOTAL
    };

    public enum RARITY
    {
        COMMON = 40, //40% chance
        UNCOMMON = 65, //25% chance
        RARE = 85, //20% chance
        EPIC = 95, //10% chance
        LEGENDARY = 101 //5% chance
    }

    protected TYPE m_CurrType = TYPE.CARROTHELMET;
    protected RARITY m_CurrRarity = RARITY.COMMON;

    protected int m_EffectVal = 0;
    protected int m_SideEffectsVal = 0;

    protected Texture m_CurrTexture;

    public Item()
    {
        //Randomising rarity first
        InitialiseRarity();

        //Randomising item based on given rarity
        InitialiseRandomItem();
    }

    public Color GetRarityColor()
    {
        switch (m_CurrRarity)
        {
            case (RARITY.COMMON):
                return Color.white;

            case (RARITY.UNCOMMON):
                return Color.green;

            case (RARITY.RARE):
                return Color.blue;

            case (RARITY.EPIC):
                return Color.magenta;

            case (RARITY.LEGENDARY):
                return Color.yellow;
        }

        return Color.white;
    }

    public string GetRarityText()
    {
        switch (m_CurrRarity)
        {
            case RARITY.COMMON:
                return "COMMON";
            case RARITY.UNCOMMON:
                return "UNCOMMON";
            case RARITY.RARE:
                return "RARE";
            case RARITY.EPIC:
                return "EPIC";
            case RARITY.LEGENDARY:
                return "LEGENDARY";
        }

        return "DONT HAVE LA";
    }

    protected void InitialiseRandomItem()
    {
        switch (m_CurrRarity)
        {
            case RARITY.COMMON:
                m_CurrType = (TYPE)Random.Range((int)TYPE.F_COMMON + 1, (int)TYPE.F_UNCOMMON - 1);
                break;
            case RARITY.UNCOMMON:
                m_CurrType = (TYPE)Random.Range((int)TYPE.F_UNCOMMON + 1, (int)TYPE.F_RARE - 1);
                break;
            case RARITY.RARE:
                m_CurrType = (TYPE)Random.Range((int)TYPE.F_RARE + 1, (int)TYPE.F_EPIC - 1);
                break;
            case RARITY.EPIC:
                m_CurrType = (TYPE)Random.Range((int)TYPE.F_EPIC + 1, (int)TYPE.F_LEGENDARY - 1);
                break;
            case RARITY.LEGENDARY:
                m_CurrType = (TYPE)Random.Range((int)TYPE.F_LEGENDARY + 1, (int)TYPE.F_TOTAL - 1);
                break;
            default:
                Debug.Log("Current rarity set is invalid! Cannot initialise random item!");
                break;
        }
    }

    protected void InitialiseRarity()
    {
        //Randomising rarity first
        int randRarity = Random.Range(0, 100);

        foreach (RARITY currRarity in Enum.GetValues(typeof(RARITY)))
        {
            if (randRarity < (int)currRarity)
            {
                m_CurrRarity = currRarity;
                break;
            }
        }

        //Debug.Log("RARITY VALUE IS " + (int)m_CurrRarity);
    }

    public void SetCurrTexture(Texture texture) { m_CurrTexture = texture; }

    public Texture GetCurrTexture() { return m_CurrTexture; }

    public void AffectStats(PlayerStats stat)
    {
        switch (m_CurrType)
        {
            case TYPE.CARROTHELMET: //INCREASE HP BY 20 (13 - 26 RANDOM)
                stat.BaseHealth += m_EffectVal;
                break;

            case TYPE.TIGERCLAW: //INCREASE ATK SPD BY 10% (7 - 12% RANDOM)
                stat.AtkSpd += m_EffectVal / 100f;
                break;
            case TYPE.FEATHERBOOT: //MOVEMENT SPD BY 7% (5-12% RANDOM)
                stat.MovementSpd += m_EffectVal / 100f;
                break;
            case TYPE.VAMPIRICSCYTHE:
                stat.LifeSteal += m_EffectVal / 100f;
                break;
            case TYPE.BIRDARMOR:
                stat.HealthAdd += stat.CalculateHealthAdd(m_EffectVal / 100f);
                break;
            case TYPE.RUNICSCYTHE:
                stat.SkillDmg += m_EffectVal / 100f;
                break;
            case TYPE.CHRONOSCYTHE:
                stat.SkillCD -= m_EffectVal / 100f;
                break;
            case TYPE.DOUBEEDGEDSCYTHE: //INCREASE DAMAGE TAKEN BY 10%, INCREASE DAMAGE DEALT BY 15%
                stat.DamageTakenMultiplier += m_SideEffectsVal / 100f;
                stat.AddPercToBasicAtk(m_EffectVal / 100f);
                break;
            case TYPE.ADDERALL: //INCREASE ALL STATS BY 10%
                stat.AddToAllStats(m_EffectVal/100f);
                break;
        }

        Debug.Log("Stats have been affected!");
    }

    public string GetInfo()
    {
        switch (m_CurrType)
        {
            case TYPE.CARROTHELMET: //Carrot ring provides higher attack speed (4-12% boost)
                return "Increases <color=yellow>Base Health</color> by <color=yellow>" + m_EffectVal + "</color> points.";

            case TYPE.TIGERCLAW:
                return "Increases <color=yellow>Attack Speed</color> by <color=yellow>" + m_EffectVal + "</color>%.";

            case TYPE.FEATHERBOOT:
                return "Increases <color=yellow>Movement Speed</color> by <color=yellow>" + m_EffectVal + "</color>%.";

            case TYPE.VAMPIRICSCYTHE:
                return "Increases <color=yellow>Lifesteal</color> by <color=yellow>" + m_EffectVal + "</color>%.";

            case TYPE.BIRDARMOR:
                return "Increases <color=yellow>Health</color> by <color=yellow>" + m_EffectVal + "</color>%.";

            case TYPE.RUNICSCYTHE:
                return "Increases <color=yellow>Skill Damage</color> by <color=yellow>" + m_EffectVal + "</color>%.";

            case TYPE.CHRONOSCYTHE:
                return "Decreases <color=yellow>Skill Cooldown</color> by <color=yellow>" + m_EffectVal + "</color>%.";

            case TYPE.DOUBEEDGEDSCYTHE:
                return "Increases <color=red>Damage Taken</color> by <color=red>" + m_SideEffectsVal + "</color>%, but increase <color=yellow>Damage Dealt</color> by <color=yellow>" + m_EffectVal.ToString() + "</color>%.";

            case TYPE.ADDERALL:
                return "Adderall";
        }

        return "BODY LA IF NOT WHAT";
    }

    public string GetName()
    {
        switch (m_CurrType)
        {
            case TYPE.CARROTHELMET: //Carrot ring provides higher attack speed (4-12% boost)
                return "Carrot Helmet";

            case TYPE.TIGERCLAW:
                return "Tiger's Claw";

            case TYPE.FEATHERBOOT:
                return "Feather Boots";

            case TYPE.VAMPIRICSCYTHE:
                return "Vampiric Scythe";

            case TYPE.BIRDARMOR:
                return "Bird Armor";

            case TYPE.RUNICSCYTHE:
                return "Runic Scythe";

            case TYPE.CHRONOSCYTHE:
                return "Chrono Scythe";

            case TYPE.DOUBEEDGEDSCYTHE:
                return "D.Edged Scythe";

            case TYPE.ADDERALL:
                return "Adderall";
        }

        return "BRUH MOMENT";
    }

    public void InitialiseRandomStats()
    {
        switch (m_CurrType)
        {
            case TYPE.CARROTHELMET: //INCREASE HP BY 20 (13 - 26 RANDOM)
                m_EffectVal = Random.Range(13, 26);
                break;

            case TYPE.TIGERCLAW: //INCREASE ATK SPD BY 10% (8 - 17% RANDOM)
                m_EffectVal = Random.Range(8, 17);
                break;
            case TYPE.FEATHERBOOT: //MOVEMENT SPD BY 7% (5-12% RANDOM)
                m_EffectVal = Random.Range(5, 12);
                break;
            case TYPE.VAMPIRICSCYTHE: //INCREASE LIFESTEAL BY 2% (2-11% RANDOM)
                m_EffectVal = Random.Range(2, 11);
                break;
            case TYPE.BIRDARMOR: //INCREASE HEALTH BY 10% (8- 14% RANDOM)
                m_EffectVal = Random.Range(8, 14);
                break;
            case TYPE.RUNICSCYTHE: //INCREASE SKILL DMG BY 10% (6-16% RANDOM)
                m_EffectVal = Random.Range(6, 16);
                break;
            case TYPE.CHRONOSCYTHE: //DECREASE SKILL CD BY 10% (4-14% RANDOM)
                m_EffectVal = Random.Range(4, 14);
                break;
            case TYPE.DOUBEEDGEDSCYTHE: //INCREASE DAMAGE TAKEN BY 10% (5-17% RANDOM), INCREASE DAMAGE DEALT BY 15% (8-24% RANDOM)
                m_SideEffectsVal = Random.Range(5, 17);
                m_EffectVal = Random.Range(8, 24);
                break;
            case TYPE.ADDERALL: //Increase all stats by 10% (8-16%)
                m_EffectVal = Random.Range(8, 16);
                break;
        }

        if (m_EffectVal > 0)
            Debug.Log("Item initialisation success with Index " + (int)m_CurrType + "!");
        else
            Debug.Log("Item initialisation failed!");
    }
}

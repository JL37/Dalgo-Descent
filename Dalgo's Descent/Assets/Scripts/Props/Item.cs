using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item
{
    public enum TYPE
    {
        CARROTRING = 0,
        GOLDRING,
        YELLOWRING,
        PURPLERING,
        REDRING
    };

    protected TYPE m_CurrType = TYPE.CARROTRING;
    protected int m_EffectVal = 0;
    protected Texture m_CurrTexture;

    public Item()
    {
        //Randomisation start
        System.Array A = System.Enum.GetValues(typeof(TYPE));
        m_CurrType = (TYPE)A.GetValue(Random.Range(0, A.Length));
    }

    public void SetCurrTexture(Texture texture) { m_CurrTexture = texture; }

    public Texture GetCurrTexture() { return m_CurrTexture; }

    public void AffectStats(PlayerStats stat)
    {
        switch (m_CurrType)
        {
            case TYPE.CARROTRING: //Carrot ring provides higher attack speed (4-12% boost)
                {
                    stat.AtkSpd += m_EffectVal / 100f;

                    break;
                }

            case TYPE.GOLDRING: //Chariot cup provides 10-25% of additional lifesteal (Capped to 65%)
                {
                    stat.LifeSteal += m_EffectVal / 100f;
                    if (stat.LifeSteal > 0.65f)
                        stat.LifeSteal = 0.65f;

                    break;
                }

            case TYPE.YELLOWRING: //Talon necklace provides a boost of 10 - 18% additional health
                {
                    int effect = (int)(stat.Health * (m_EffectVal / 100f));
                    stat.Health += effect;

                    break;
                }

            case TYPE.PURPLERING: //Rabbit bible increases crit chance by 5- 12% (Capped at 65%)
                {
                    stat.CritChance += m_EffectVal/100f;
                    if (stat.CritChance > 0.65f)
                        stat.CritChance = 0.65f;

                    break;
                }

            case TYPE.REDRING: //Blade boost basic attack damage by 15 - 40%
                {
                    float effect;
                    effect = (int)(stat.BasicAtk * m_EffectVal / 100f);
                    stat.BasicAtk += (int)effect;

                    break;
                }
        }

        Debug.Log("Stats have been affected!");
    }

    public string GetText()
    {
        switch (m_CurrType)
        {
            case TYPE.CARROTRING: //Carrot ring provides higher attack speed (4-12% boost)
                return "<color=yellow>Attack Speed: </color> +" + m_EffectVal + "%";

            case TYPE.GOLDRING: //Chariot cup provides 10-25% of additional lifesteal (Capped to 65%)
                return "<color=yellow>Lifesteal: </color> +" + m_EffectVal + "%";

            case TYPE.YELLOWRING: //Talon necklace provides a boost of 10 - 18% additional health
                return "<color=yellow>Health: </color> +" + m_EffectVal + "%";

            case TYPE.PURPLERING: //Rabbit bible increases crit chance by 5- 12% (Capped at 65%)
                return "<color=yellow>Critical Chance: </color> +" + m_EffectVal + "%";

            case TYPE.REDRING: //Blade boost basic attack damage by 15 - 40%
                return "<color=yellow>Basic Attack: </color> +" + m_EffectVal + "%";
        }

        return "BRUH MOMENT";
    }

    public void InitialiseRandomStats()
    {
        switch (m_CurrType)
        {
            case TYPE.CARROTRING: //Carrot ring provides higher attack speed (4-12% boost)
                {
                    float effect = Random.Range(0.04f, 0.12f);
                    m_EffectVal = (int)(effect * 100);

                    break;
                }

            case TYPE.GOLDRING: //Chariot cup provides 10-25% of additional lifesteal (Capped to 65%)
                {
                    float effect = Random.Range(0.1f, 0.25f);
                    m_EffectVal = (int)(effect * 100);

                    break;
                }

            case TYPE.YELLOWRING: //Talon necklace provides a boost of 10 - 18% additional health
                {
                    int effect = Random.Range(10, 18);
                    m_EffectVal = effect;

                    break;
                }

            case TYPE.PURPLERING: //Rabbit bible increases crit chance by 5- 12% (Capped at 65%)
                {
                    float effect = Random.Range(0.05f, 0.12f);
                    m_EffectVal = (int)(effect * 100);

                    break;
                }

            case TYPE.REDRING: //Blade boost basic attack damage by 15 - 40%
                {
                    float effect = Random.Range(0.15f, 0.4f);
                    m_EffectVal = (int)(effect * 100);

                    break;
                }
        }

        if (m_EffectVal > 0)
            Debug.Log("Item initialisation success with Index " + (int)m_CurrType + "!");
        else
            Debug.Log("Item initialisation failed!");
    }
}

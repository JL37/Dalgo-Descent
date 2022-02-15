using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item
{
    public enum TYPE
    {
        CARROTRING = 0,
        CHARIOTCUP,
        TALONNECKLACE,
        RABBITBIBLE,
        BLADE
    };

    [SerializeField] TYPE m_CurrType = TYPE.CARROTRING;
    protected int m_EffectVal = 0;

    public Item()
    {
        //Randomisation start
        System.Array A = System.Enum.GetValues(typeof(TYPE));
        m_CurrType = (TYPE)A.GetValue(Random.Range(0, A.Length));
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void InitialiseStats(PlayerStats stat)
    {
        switch (m_CurrType)
        {
            case TYPE.CARROTRING: //Carrot ring provides higher attack speed (4-12% boost)
                {
                    float effect = Random.Range(0.04f, 0.12f);
                    stat.AtkSpd += effect;
                    m_EffectVal = (int)(effect * 100);

                    break;
                }

            case TYPE.CHARIOTCUP: //Chariot cup provides 10-25% of additional lifesteal (Capped to 65%)
                {
                    float effect = Random.Range(0.1f, 0.25f);
                    m_EffectVal = (int)(effect * 100);
                    stat.LifeSteal += effect;
                    if (stat.LifeSteal > 0.65f)
                        stat.LifeSteal = 0.65f;

                    break;
                }

            case TYPE.TALONNECKLACE: //Talon necklace provides a boost of 10 - 18% additional health
                {
                    int effect = Random.Range(10, 18);
                    m_EffectVal = effect;
                    effect = (int)(stat.Health * (effect / 100f));
                    stat.Health += effect;

                    break;
                }

            case TYPE.RABBITBIBLE: //Rabbit bible increases crit chance by 5- 12% (Capped at 65%)
                {
                    float effect = Random.Range(0.05f, 0.12f);
                    m_EffectVal = (int)(effect * 100);
                    stat.CritChance += effect;
                    if (stat.CritChance > 0.65f)
                        stat.CritChance = 0.65f;

                    break;
                }

            case TYPE.BLADE: //Blade boost basic attack damage by 15 - 40%
                {
                    float effect = Random.Range(0.15f, 0.4f);
                    m_EffectVal = (int)(effect * 100);
                    effect = (int)(stat.BasicAtk * effect);
                    stat.BasicAtk += (int)effect;

                    break;
                }
        }

        if (m_EffectVal > 0)
            Debug.Log("Item initialisation success with Index " + (int)m_CurrType + "!");
        else
            Debug.Log("Item initialisation failed!");
    }
}

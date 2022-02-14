using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public enum TYPE
    {
        CARROTRING = 0,
        CHARIOTCUP,
        TALONNECKLACE,
        RABBITBIBLE,
        BLADE,
        TOTAL
    };

    [SerializeField] TYPE m_CurrType = TYPE.CARROTRING;
    protected int m_EffectVal = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void InitialiseStats(ref int stat)
    {
        switch (m_CurrType)
        {
            case TYPE.CARROTRING: //Carrot ring provides higher attack speed (4-12% boost)
                m_EffectVal = Random.Range(4, 12);
                stat = (int)(stat * (float)((100f + m_EffectVal) / 100f));

                break;

            case TYPE.CHARIOTCUP: //Chariot cup provides 10-25% of additional lifesteal (Capped to 65%)
                m_EffectVal = Random.Range(10, 25);
                stat += m_EffectVal;
                if (stat > 65)
                    stat = 65;

                break;

            case TYPE.TALONNECKLACE: //Talon necklace provides a boost of 10 - 18% additional health
                m_EffectVal = Random.Range(10, 18);
                stat = (int)(stat * (float)((100f + m_EffectVal) / 100f));

                break;

            case TYPE.RABBITBIBLE: //Rabbit bible Reduces crit chance by 5- 12% (Capped at 40%)
                m_EffectVal = Random.Range(5, 12);
                stat -= m_EffectVal;
                if (stat < 45)
                    stat = 45;

                break;

            case TYPE.BLADE: //Blade boost damage done by 15 - 30%
                m_EffectVal = Random.Range(15, 30);
                stat = (int)(stat * (float)((100f + m_EffectVal) / 100f));

                break;
        }
    }
}

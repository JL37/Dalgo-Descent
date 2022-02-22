using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimationEvent : MonoBehaviour
{
    AIUnit aiUnit;

    void Start()
    {
        aiUnit = transform.parent.GetComponent<AIUnit>();
    }

    public void AttackPlayer()
    {
        aiUnit.AttackPlayer();
    }

    public void Die()
    {
        aiUnit.Die();
    }
}

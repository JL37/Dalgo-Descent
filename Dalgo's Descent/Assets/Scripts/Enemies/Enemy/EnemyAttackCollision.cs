using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackCollision : MonoBehaviour
{
    public AIUnit aiUnit;

    private void OnTriggerEnter(Collider other)
    {
        // Debug.Log("Hit Player");
        if (other.gameObject.tag == "Player")
        {
            aiUnit.inAttackRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Debug.Log("Hit Player");
        if (other.gameObject.tag == "Player")
        {
            aiUnit.inAttackRange = false;
        }
    }
}

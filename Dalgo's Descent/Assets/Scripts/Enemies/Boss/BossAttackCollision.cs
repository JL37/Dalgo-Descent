using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAttackCollision : MonoBehaviour
{
    public BossAI bossAI;

    private void OnTriggerEnter(Collider other)
    {
        // Debug.Log("Hit Player");
        if (other.gameObject.tag == "Player")
        {
            bossAI.inAttackRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Debug.Log("Hit Player");
        if (other.gameObject.tag == "Player")
        {
            bossAI.inAttackRange = false;
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGameScript : MonoBehaviour
{
    [SerializeField] EnemyManager enemyManager;
    private MeshRenderer barrierMesh;
    private Collider barrierCollider;

    private void Awake()
    {
        barrierMesh = GetComponent<MeshRenderer>();
        barrierCollider = GetComponent<Collider>(); 
    }
    private void Update()
    {
        if (enemyManager != null)
        {
            if (enemyManager.BossKilled)
            {
                barrierMesh.enabled = false;
                barrierCollider.isTrigger = true;
            }
            else
            {
                barrierMesh.enabled = true;
                barrierCollider.isTrigger = false;
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            AudioManager.Instance.Stop("BossMusic");
            GameManager.Instance.Victory();
        }
    }
}

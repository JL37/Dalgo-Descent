using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Projectile : MonoBehaviour
{
    public Vector3 directionVelocity;

    private void Start()
    {
        directionVelocity = new Vector3();
    }

    private void Update()
    {
        transform.position = transform.position + directionVelocity;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Debug.Log("Projectile Hit");
            // deal damage and knockback
        }

        Destroy(gameObject);

    }

}

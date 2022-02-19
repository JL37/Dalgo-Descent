using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Projectile : MonoBehaviour
{
    public Vector3 directionVelocity;
    public ParticleSystem woodThrowParticleSystemPrefab;

    private void Start()
    {
        directionVelocity = new Vector3();
    }

    private void Update()
    {
        transform.position = transform.position + directionVelocity * Time.deltaTime * 10f;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag != "ProjCollidable")
            return;

        if (directionVelocity == Vector3.zero)
            return;

        Debug.Log("Particle Collided");
        Instantiate(woodThrowParticleSystemPrefab, transform.position, Quaternion.identity);
        Collider[] colliders = Physics.OverlapSphere(transform.position, 4f);
        foreach (Collider c in colliders)
        {
            if (c.gameObject.tag == "Player")
            {
                // deal damage
            }
        }

        Destroy(gameObject);
    }

}

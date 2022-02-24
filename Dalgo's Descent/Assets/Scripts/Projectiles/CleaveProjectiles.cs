using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CleaveProjectiles : MonoBehaviour
{
    void Start()
    {
        
        gameObject.SetActive(false);
    }

    void OnParticleSystemStopped()
    {
        Destroy(gameObject);
    }
}

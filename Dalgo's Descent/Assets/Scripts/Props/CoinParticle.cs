using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinParticle : MonoBehaviour
{
    private void OnParticleSystemStopped()
    {
        //print("Particle finished playing");
        if (!transform.parent.parent.GetComponent<ObjectPoolManager>())
            Destroy(transform.parent.gameObject);

        transform.parent.gameObject.SetActive(false); //Set back to false
        transform.parent.GetComponent<Coin>().GetModel().SetActive(true); //Reset coin model visibility
    }
}

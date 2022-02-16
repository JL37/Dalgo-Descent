using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //GetComponent<MeshRenderer>().material.color = Color.black;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            //Pick up coin la if not what bro?
            PlayerStats playerStats = other.GetComponent<PlayerStats>();
            playerStats.AddCoin();

            //Delete self from world
            Destroy(gameObject);
        }
    }
}

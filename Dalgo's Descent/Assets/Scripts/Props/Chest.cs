using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    protected bool m_WithinRange = false;
    protected bool m_Opened = false;
    protected int m_Cost = 1;

    [SerializeField] PlayerStats m_PlayerStats;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<MeshRenderer>().material.color = Color.yellow;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
            m_WithinRange = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
            m_WithinRange = false;
    }

    protected void OnInteract()
    {
        if (!m_WithinRange) //If within range or not
            return;

        if (m_Opened)
            return;

        //Check if player has enough coins la
        if (m_PlayerStats.DeductCoin(m_Cost))
        {
            m_Opened = true;
            GetComponent<MeshRenderer>().material.color = Color.red;
            print("CHEST HAS BEEN OPENED");
        }
    }
}

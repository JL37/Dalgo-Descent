using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{
    [Header("Linked components")]
    [SerializeField] GameObject m_HealthFolder;
    [SerializeField] Image m_HealthBar;
    [SerializeField] Health m_Health;

    protected GameObject m_Player;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Canvas>().worldCamera = Camera.main;
        m_Player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        //Updating ui...
        m_HealthFolder.transform.LookAt(Camera.main.transform, Camera.main.transform.up);

        //Updating scaling of health
        m_HealthBar.transform.localScale = new Vector3(m_Health.currentHealth / m_Health.maxHealth, 1, 1);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    [Header("Linked objects")]
    [SerializeField] Transform m_ModelTransform;

    [Header("Variables to adjust")]
    [SerializeField] float m_RotationSpd = 280f;
    [SerializeField] float m_Duration = 0.75f;
    [SerializeField] float m_MaxHeight = 0.3f;


    //variables to reset la if not what
    protected float m_CurrRadians = 0;

    private void Update()
    {
        UpdateRotate();
        UpdatePosition();
    }

    public void UpdatePosition()
    {
        float spd = (Time.deltaTime / m_Duration) * Mathf.PI;

        m_CurrRadians += spd;
        if (m_CurrRadians > 2 * Mathf.PI)
            m_CurrRadians -= 2 * Mathf.PI;

        float newY = Mathf.Sin(m_CurrRadians) * m_MaxHeight;
        m_ModelTransform.localPosition = new Vector3(0, newY, 0);
    }

    public void UpdateRotate()
    {
        //Animation
        float rotSpd = m_RotationSpd * Time.deltaTime;
        m_ModelTransform.Rotate(new Vector3(1, 0, 0) * rotSpd);
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

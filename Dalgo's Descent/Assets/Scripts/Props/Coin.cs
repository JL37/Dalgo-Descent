using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour, IObjectPooling
{
    [Header("Linked objects")]
    [SerializeField] Transform m_ModelTransform;
    [SerializeField] ParticleSystem m_ParticleSystem;
    [SerializeField] Animator m_Animator;

    [Header("Variables to adjust")]
    [SerializeField] float m_RotationSpd = 280f;
    [SerializeField] float m_Duration = 0.75f;
    [SerializeField] float m_MaxHeight = 0.3f;


    //variables to reset la if not what
    protected float m_CurrRadians = 0;

    public GameObject GetModel()
    {
        return m_ModelTransform.gameObject;
    }

    private void Update()
    {
        UpdateRotate();
        UpdatePosition();
    }

    public void Initialise()
    {
        m_Animator.Play("Nothing");

        //Random ass position test
        Vector3 pos = new Vector3(Random.Range(0f, 7f), -2.627f, Random.Range(0f, 7f));
        transform.position = pos;
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
        m_ModelTransform.Rotate(new Vector3(0, 1, 0) * rotSpd);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" && m_ModelTransform.gameObject.activeSelf)
        {
            //Pick up coin la if not what bro?
            PlayerStats playerStats = other.GetComponent<PlayerStats>();
            playerStats.AddCoin();

            m_ParticleSystem.Play();
            m_Animator.SetTrigger("PickedUp");

            StartCoroutine(Disable(0.3f));
        }
    }

    protected IEnumerator Disable(float time)
    {
        yield return new WaitForSeconds(time);

        if (transform.parent.GetComponent<ObjectPoolManager>())
            gameObject.SetActive(false);
        else
            Destroy(gameObject);
    }
}

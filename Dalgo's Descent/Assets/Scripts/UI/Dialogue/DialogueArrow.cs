using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueArrow : MonoBehaviour
{
    [SerializeField] float m_Duration = 0.2f;
    [SerializeField] float m_MaxHeight = 0.15f;

    protected float m_CurrRadians = 0;

    protected Vector3 ogPos;
    protected Transform m_Transform;

    // Start is called before the first frame update
    void Start()
    {
        m_Transform = GetComponent<RectTransform>();
        ogPos = m_Transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        UpdatePosition();
    }

    public void UpdatePosition()
    {
        float spd = (Time.deltaTime / m_Duration) * Mathf.PI;

        m_CurrRadians += spd;
        if (m_CurrRadians > 2 * Mathf.PI)
            m_CurrRadians -= 2 * Mathf.PI;

        float newY = Mathf.Sin(m_CurrRadians) * m_MaxHeight;
        m_Transform.position = new Vector3(ogPos.x, ogPos.y + newY, ogPos.z);
    }
}

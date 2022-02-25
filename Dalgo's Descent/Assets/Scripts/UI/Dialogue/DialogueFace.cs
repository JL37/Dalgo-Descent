using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueFace : MonoBehaviour
{
    [Header("Variables to adjust")]
    [SerializeField] Vector3 m_DownOffSet = new Vector3(0,-25,0);

    protected Vector3 originalPos;

    // Start is called before the first frame update
    void Start()
    {
        originalPos = GetComponent<RectTransform>().position;
        GetComponent<RectTransform>().position += m_DownOffSet;

        gameObject.SetActive(false);
        print("Face start la");
    }

    // Update is called once per frame
    void Update()
    {
        UpdateToTarget(originalPos,0.09f);
    }

    public void Initialise(Sprite defFace)
    {
        gameObject.SetActive(true);
        GetComponent<Image>().sprite = defFace;
    }

    protected void UpdateToTarget(Vector3 pos, float lerp = 0.3f)
    {
        if ((GetComponent<RectTransform>().position - pos).magnitude > 0.01f)
        {
            //Math
            print("LERP LA");
            Vector3 newPos = GetComponent<RectTransform>().position;
            newPos.x = Mathf.Lerp(newPos.x, pos.x, lerp);
            newPos.y = Mathf.Lerp(newPos.y, pos.y, lerp);
            newPos.z = Mathf.Lerp(newPos.z, pos.z, lerp);

            GetComponent<RectTransform>().position = newPos;
        } 
        else
        {
            GetComponent<RectTransform>().position = pos;
        }
    }
}

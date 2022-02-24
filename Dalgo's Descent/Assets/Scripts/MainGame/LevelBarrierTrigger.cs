using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelBarrierTrigger : MonoBehaviour
{
    public bool IsNextLevel;
    public Collider TriggerCollider;
    public Collider BarrierCollider;
    MeshRenderer BarrierMesh;
    // Start is called before the first frame update
    void Start()
    {
        BarrierMesh = GetComponent<MeshRenderer>();

        BarrierMesh.enabled = false;
        BarrierCollider.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerExit(Collider other)
    {
        BarrierMesh.enabled = true;
        BarrierCollider.enabled = true;

        if(IsNextLevel)
        {
            GetComponentInParent<LevelExteriorManager>().OnLevelExit();
        }
        else
        {
            GetComponentInParent<LevelExteriorManager>().OnLevelEnter();
        }
    }

}

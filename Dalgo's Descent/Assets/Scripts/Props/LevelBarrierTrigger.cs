using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelBarrierTrigger : MonoBehaviour
{
    public bool Activated;
    public bool IsNextLevel;
    public Collider TriggerCollider;
    public Collider BarrierCollider;
    MeshRenderer BarrierMesh;


    void Start()
    {
        BarrierMesh = GetComponent<MeshRenderer>();

        BarrierMesh.enabled = false;
        BarrierCollider.enabled = false;
    }

    void Update()
    {
        SetActivation(Activated);    
    }
    public void SetActivation(bool active)
    {
        Activated = active;
        if (active)
        {
            if (BarrierMesh != null)
                BarrierMesh.enabled = true;
            BarrierCollider.enabled = true;
        }
        else
        {
            if (BarrierMesh != null)
                BarrierMesh.enabled = false;
            BarrierCollider.enabled = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (Activated || other.tag != "Player")
            return;

        Activated = true;
        BarrierMesh.enabled = true;
        BarrierCollider.enabled = true;

        if (IsNextLevel)
            GameLevelManager.Instance.OnNextLevelEnter();
        else
            GameLevelManager.Instance.OnCurrentLevelExit();
    }

}

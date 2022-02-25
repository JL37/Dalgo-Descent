using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelStructure : MonoBehaviour
{
    public LevelBarrierTrigger ExitBarrier;
    public LevelBarrierTrigger EntranceBarrier;
    public GameObject ExteriorLayer;
    public GameObject ExteriorColliders;
    public Transform ExteriorCheckpoints;
    public Transform NextLocation;
    private void Awake()
    {
        ExteriorLayer.SetActive(false);

        for (int i = 0; i < ExteriorCheckpoints.childCount; i++)
            ExteriorCheckpoints.GetChild(i).GetComponent<MeshRenderer>().enabled = false;
    }


    public void OnNextLevelTransition()
    {
        ExteriorLayer.SetActive(true);
        CameraController.Instance.SetCinemachineMode(CameraController.CMMode.Exterior);
    }

    public void OnNextLevelEnter()
    {
        ExteriorLayer.SetActive(false);
        GameLevelManager.Instance.OnLevelNext();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            var playerpos2D = new Vector3(other.transform.position.x, 0, other.transform.position.z);

            Transform closestpoint = null;
            foreach (Transform t in ExteriorCheckpoints)
            {
                var checkpointLoc2D = new Vector3(t.position.x, 0, t.position.z);

                if (closestpoint == null)
                    closestpoint = t;
                else if ((checkpointLoc2D - playerpos2D).sqrMagnitude < (checkpointLoc2D - new Vector3(closestpoint.position.x, 0, closestpoint.position.z)).sqrMagnitude)
                    closestpoint = t;
            }
            other.GetComponent<CharacterController>().Move((closestpoint.position + new Vector3(0, 5, 0)) - other.transform.position);
        }
    }
}

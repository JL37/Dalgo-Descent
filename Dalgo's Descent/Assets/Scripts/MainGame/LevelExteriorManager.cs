using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Cinemachine;
public class LevelExteriorManager : MonoBehaviour
{
    [Range(0, 10)] public float CamHeightOffset;
    public GameObject ExteriorLayer;
    public Transform LookAtTarget;
    public Collider BottomTrigger;
    public GameObject Checkpoints;
    private List<Transform> CheckpointLocations;
    private CinemachineFreeLook ExteriorVCam;
    // Start is called before the first frame update
    void Start()
    {
        CheckpointLocations = Checkpoints.transform.Cast<Transform>().ToList();
    }

    // Update is called once per frame
    void Update()
    {
        if(ExteriorVCam)
        {
            var height = GetComponent<GameLevelManager>().Player.transform.position.y + CamHeightOffset;
            LookAtTarget.position = new Vector3(LookAtTarget.position.x, height, LookAtTarget.position.z);

            var direction = ExteriorVCam.Follow.position - ExteriorVCam.LookAt.position;
            direction.y = 0;

            var angle = Vector3.Angle(direction, Vector3.forward);
            float angle2 = Vector3.Angle(direction, Vector3.right);

            if (angle2 > 90)
            {
                angle = 360 - angle;
            }
            ExteriorVCam.m_XAxis.Value = angle;
        }
    }

    public void OnLevelExit()
    {
        enabled = true;
        ExteriorLayer.SetActive(true);
        ExteriorVCam = GetComponent<GameLevelManager>().SetCinemachine(GameLevelManager.CMMode.Exterior);
    }

    public void OnLevelEnter()
    {
        ExteriorLayer.SetActive(false);
        ExteriorVCam = GetComponent<GameLevelManager>().SetCinemachine(GameLevelManager.CMMode.Player);
        enabled = false;
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            var playerpos2D = new Vector3(other.transform.position.x, 0, other.transform.position.z);

            Transform closestpoint = null;
            foreach(Transform t in CheckpointLocations)
            {
                var checkpointLoc2D = new Vector3(t.position.x, 0, t.position.z);

                if(closestpoint == null)
                    closestpoint = t;
                else if ((checkpointLoc2D - playerpos2D).sqrMagnitude < (checkpointLoc2D - new Vector3(closestpoint.position.x, 0, closestpoint.position.z)).sqrMagnitude)
                    closestpoint = t;
            }
            other.GetComponent<CharacterController>().Move( (closestpoint.position + new Vector3(0,5,0))- other.transform.position);
        }
    }
}

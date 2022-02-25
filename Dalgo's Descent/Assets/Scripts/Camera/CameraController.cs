using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class CameraController : Singleton<CameraController>
{
    [Header("Camera Setup")]
    public CMMode Mode;
    public CinemachineFreeLook ActiveCM;
    public CinemachineFreeLook PlayerCM;
    public CinemachineFreeLook ExteriorCM;

    [Header("Exterior Camera Settings")]
    [Range(0,10)]public float CameraHeightOffset;
    public Transform ExteriorFollowPoint;
    // Start is called before the first frame update
    void Start()
    {
        SetCinemachineMode(Mode);
    }

    // Update is called once per frame
    void Update()
    {
        if(Mode == CMMode.Exterior)
        {
            var height = GameLevelManager.Instance.Player.transform.position.y + CameraHeightOffset;
            ExteriorFollowPoint.position = new Vector3(ExteriorFollowPoint.position.x, height, ExteriorFollowPoint.position.z);

            var direction = ExteriorCM.Follow.position - ExteriorCM.LookAt.position;
            direction.y = 0;

            var angle = Vector3.Angle(direction, Vector3.forward);
            float angle2 = Vector3.Angle(direction, Vector3.right);

            if (angle2 > 90)
            {
                angle = 360 - angle;
            }
            ExteriorCM.m_XAxis.Value = angle;
        }
    }

    public CinemachineFreeLook SetCinemachineMode(CMMode mode)
    {
        Mode = mode;
        ActiveCM = GetCinemachineFromMode(mode);

        foreach (var cm in FindObjectsOfType<CinemachineFreeLook>(true))
        {
            if (cm != ActiveCM)
                cm.gameObject.SetActive(false);
            else
                cm.gameObject.SetActive(true);
        }

        return ActiveCM;
    }

    private CinemachineFreeLook GetCinemachineFromMode(CMMode mode)
    {
        switch (mode)
        {
            case CMMode.Player: return PlayerCM;
            case CMMode.Exterior: return ExteriorCM;
        }

        Debug.LogError("Invalid Camera Mode!");
        return null;
    }
    public enum CMMode
    {
        Player,
        Exterior
    }
}

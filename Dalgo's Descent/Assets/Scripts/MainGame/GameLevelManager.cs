using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Cinemachine;
public class GameLevelManager : Singleton<GameLevelManager>
{
    public CMMode Mode;
    public CinemachineFreeLook ActiveCM;
    
    public CinemachineFreeLook PlayerCM;
    public CinemachineFreeLook ExteriorCM;

    public PlayerController Player;

    void Start()
    {
        SetCinemachine(Mode);
    }
    public CinemachineFreeLook SetCinemachine(CMMode mode)
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
            case CMMode.Player:         return PlayerCM;
            case CMMode.Exterior:       return ExteriorCM;
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Cinemachine;
public class GameLevelManager : Singleton<GameLevelManager>
{
    [Header("Level Generation")]
    public int TotalLevels;
    public int CurrentLevel;
    public float LevelOffset;
    public GameObject LevelContainer;
    public GameObject StartLevel;
    public GameObject LastLevel;
    public List<GameObject> LevelPrefabs;
    [Header("Camera Setup")]
    public CMMode Mode;
    public CinemachineFreeLook ActiveCM;
    public CinemachineFreeLook PlayerCM;
    public CinemachineFreeLook ExteriorCM;

    public PlayerController Player;

    void Start()
    {
        SetCinemachine(Mode);
        CreateLevel();
    }

    void CreateLevel()
    {
        if (CurrentLevel > TotalLevels)
            return;

        CurrentLevel++;
        if(CurrentLevel == 1)
        {
            Instantiate(StartLevel, new Vector3(0, 0, 0), Quaternion.identity, LevelContainer.transform); //Spawn Start
            Instantiate(LevelPrefabs[0], new Vector3(0,-LevelOffset, 0), Quaternion.identity, LevelContainer.transform); //Spawn Next Stage
        }
        else if (CurrentLevel == TotalLevels)
        {

        }
        else
        {

        }

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

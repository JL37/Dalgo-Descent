using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Cinemachine;
using UnityEngine.AI;

public class GameLevelManager : Singleton<GameLevelManager>
{
    [Header("Level Generation")]
    public int TotalLevels;
    public int CurrentLevel;
    public float LevelOffset;
    public GameObject LevelContainer;
    public LevelStructure StartLevel;
    public LevelStructure LastLevel;
    public List<LevelStructure> LevelPrefabs;
    
    public PlayerController Player;
    public BossAI BossObject;

    public delegate void OnNextLevelListenDelegate();
    public event OnNextLevelListenDelegate OnNextLevelEnterListener;

    public delegate void OnCurrentLevelListenDelegate();
    public event OnCurrentLevelListenDelegate OnCurrentLevelExitListener;

    private LevelStructure PlayArea;
    void Start()
    {
        CreateNextLevel();
    }
   
    public void OnNextLevelEnter()
    { 
        CreateNextLevel();

        if(OnNextLevelEnterListener != null)OnNextLevelEnterListener.Invoke();
    }

    public void OnCurrentLevelExit()
    {
        if (OnCurrentLevelExitListener != null) OnCurrentLevelExitListener.Invoke();
    }

    void CreateNextLevel()
    {
        if (CurrentLevel > TotalLevels)
            return;

        CurrentLevel++;
        if (CurrentLevel == 1)
        {
            PlayArea = Instantiate(StartLevel, new Vector3(0, 0, 0), Quaternion.identity, LevelContainer.transform); //Spawn Start
        }
        else if (CurrentLevel == TotalLevels)
        {
            var newArea = Instantiate(LastLevel, PlayArea.transform.position + new Vector3(0, -11 * 2, 0), PlayArea.NextLocation.rotation, LevelContainer.transform); //Spawn Start
            DestroyPlayLevel();
            PlayArea = newArea;
        }
        else
        {
            var newArea = Instantiate(LevelPrefabs[0], PlayArea.transform.position + new Vector3(0, -11 * 2, 0), PlayArea.NextLocation.rotation, LevelContainer.transform); //Spawn Start
            DestroyPlayLevel();
            PlayArea = newArea;
        }
        PlayArea.LevelNum = CurrentLevel;
    }

    void DestroyPlayLevel()
    {
        Destroy(PlayArea.gameObject);
        PlayArea = null;
    }

    public bool IsLastLevel() { return CurrentLevel == TotalLevels; }
}

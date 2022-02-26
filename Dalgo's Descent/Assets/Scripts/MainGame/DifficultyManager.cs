using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DifficultyManager : MonoBehaviour
{
    public static DifficultyManager Instance;
    public float difficultyScaling;
    public float currentRunTimeElapsed;

    public int NumWaves;
    public int NumEnemiesPerWave;

    private void Awake()
    {
        if (Instance)
            Destroy(Instance.gameObject);

        Instance = this;
        currentRunTimeElapsed = 0;
    }

    private void Start()
    {
        NumWaves = 3;
        NumEnemiesPerWave = 5;
    }

    // Update is called once per frame
    void Update()
    {
        currentRunTimeElapsed += Time.deltaTime;
        difficultyScaling = Mathf.Exp(1 + currentRunTimeElapsed * 0.0015f) - 1.7f; 

        NumWaves = (int)(2 + Mathf.Pow(difficultyScaling, 1.2f));
        NumEnemiesPerWave = (int)(5 + Mathf.Pow(difficultyScaling, 1.5f));
    }
}

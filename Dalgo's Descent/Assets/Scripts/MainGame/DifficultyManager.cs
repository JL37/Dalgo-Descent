using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DifficultyManager : MonoBehaviour
{
    public static DifficultyManager Instance;
    public float difficultyScaling;
    public float currentRunTimeElapsed;

    private void Awake()
    {
        if (Instance)
            Destroy(Instance.gameObject);

        Instance = this;
        currentRunTimeElapsed = 0;
    }

    // Update is called once per frame
    void Update()
    {
        currentRunTimeElapsed += Time.deltaTime;
        difficultyScaling = Mathf.Exp(1 + currentRunTimeElapsed * 0.0015f) - 1.7f; 
        Debug.Log(difficultyScaling);
    }
}

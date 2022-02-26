using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavigationBaker : MonoBehaviour
{
    NavMeshSurface[] surfaces;
    void Start()
    {
        surfaces = GetComponentsInChildren<NavMeshSurface>();
        for (int i = 0; i < surfaces.Length; i++)
        {
            surfaces[i].BuildNavMesh();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

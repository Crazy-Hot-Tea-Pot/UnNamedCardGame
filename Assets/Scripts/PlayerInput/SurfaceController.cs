using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;

/// <summary>
/// Handles stuff for surfaces.
/// Example updating the Nav mash to be dynamic
/// </summary>
public class SurfaceController : MonoBehaviour
{
    public NavMeshSurface navMeshSurface;

    // Start is called before the first frame update
    void Start()
    {
        navMeshSurface.BuildNavMesh();
    }

    // Update is called once per frame
    void Update()
    {
        navMeshSurface.BuildNavMesh();
    }
}

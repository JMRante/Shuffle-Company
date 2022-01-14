using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JaggedStageMeshDefinition : StageMeshDefinition
{
    public JaggedStageMeshDefinition() : base()
    {
        name = "Jagged";

        GameObject stageMeshObject = Resources.Load<GameObject>("Models/StageMeshes/StageMeshJagged");

        MeshFilter[] childMeshFilters = stageMeshObject.GetComponentsInChildren<MeshFilter>();

        foreach (MeshFilter meshFilter in childMeshFilters)
        {
            stageMeshes.Add(meshFilter.gameObject.name, meshFilter.sharedMesh);
        }
    }
}

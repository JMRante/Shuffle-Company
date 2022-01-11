using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubicStageMeshDefinition : StageMeshDefinition
{
    public CubicStageMeshDefinition() : base()
    {
        GameObject stageMeshObject = Resources.Load<GameObject>("Models/StageMeshes/StageMeshCubic");

        edgeAboveLeft = stageMeshObject.transform.Find("CubicEdgeAboveLeft").GetComponent<MeshFilter>().sharedMesh;
        edgeAboveRight = stageMeshObject.transform.Find("CubicEdgeAboveRight").GetComponent<MeshFilter>().sharedMesh;
        edgeBelowLeft = stageMeshObject.transform.Find("CubicEdgeBelowLeft").GetComponent<MeshFilter>().sharedMesh;
        edgeBelowRight = stageMeshObject.transform.Find("CubicEdgeBelowRight").GetComponent<MeshFilter>().sharedMesh;

        outerCornerAboveLeft = stageMeshObject.transform.Find("CubicOuterCornerAboveLeft").GetComponent<MeshFilter>().sharedMesh;
        outerCornerAboveRight = stageMeshObject.transform.Find("CubicOuterCornerAboveRight").GetComponent<MeshFilter>().sharedMesh;
        outerCornerBelowLeft = stageMeshObject.transform.Find("CubicOuterCornerBelowLeft").GetComponent<MeshFilter>().sharedMesh;
        outerCornerBelowRight = stageMeshObject.transform.Find("CubicOuterCornerBelowRight").GetComponent<MeshFilter>().sharedMesh;

        edgeLeftCap = stageMeshObject.transform.Find("CubicEdgeLeftCap").GetComponent<MeshFilter>().sharedMesh;
        edgeRightCap = stageMeshObject.transform.Find("CubicEdgeRightCap").GetComponent<MeshFilter>().sharedMesh;
        outerCornerCap = stageMeshObject.transform.Find("CubicOuterCornerCap").GetComponent<MeshFilter>().sharedMesh;
    }
}

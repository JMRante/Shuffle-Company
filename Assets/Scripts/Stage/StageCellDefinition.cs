using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StageCellDefinition
{
    public string name;

    public int geometryType;

    public StageTextureDefinition textureDefinitionTop;
    public StageTextureDefinition textureDefinitionBottom;
    public StageTextureDefinition textureDefinitionLeft;
    public StageTextureDefinition textureDefinitionRight;
    public StageTextureDefinition textureDefinitionForward;
    public StageTextureDefinition textureDefinitionBack;

    public bool acceptEdges;
    public int edgePriority;

    public StageTextureDefinition edgeTop;
    public StageTextureDefinition edgeBottom;
    public StageTextureDefinition edgeLeft;
    public StageTextureDefinition edgeRight;
    public StageTextureDefinition edgeForward;
    public StageTextureDefinition edgeBack;

    public StageGeometryType GetGeometryType()
    {
        return (StageGeometryType)geometryType;
    }

    public void SetGeometryType(StageGeometryType geometryType)
    {
        this.geometryType = (int)geometryType;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StageCellDefinition
{
    public string name;

    public byte geometryType;

    public byte textureDefinitionTop;
    public byte textureDefinitionBottom;
    public byte textureDefinitionLeft;
    public byte textureDefinitionRight;
    public byte textureDefinitionForward;
    public byte textureDefinitionBack;

    public byte edgeTop;
    public byte edgeBottom;
    public byte edgeLeft;
    public byte edgeRight;
    public byte edgeForward;
    public byte edgeBack;

    public StageGeometryType GetGeometryType()
    {
        return (StageGeometryType)geometryType;
    }

    public void SetGeometryType(StageGeometryType geometryType)
    {
        this.geometryType = (byte)geometryType;
    }
}

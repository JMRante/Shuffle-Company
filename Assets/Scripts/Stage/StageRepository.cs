using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StageGeometryType
{
    Square,
    SmallBevel,
    LargeBevel,
    SmallCurve,
    LargeCurve,
    WeakJagged,
    StrongJagged,
    WeakWave,
    StrongWave
}

public enum StageTextureSheetType
{
    stageTexture256,
    stageTexture512,
    stageTexture1024,
    stageTextureBlob,
    stageTextureEdge,
    stageTextureDecal
}

public class StageRepository : MonoBehaviour
{
    private Dictionary<string, Mesh>[] geometryRepo;
    private Texture2DArray[] textureRepo;
    private StageCellDefinition[] cellRepo;
    private GameObject[] propRepo;

    void Start()
    {
        geometryRepo = new Dictionary<string, Mesh>[Enum.GetNames(typeof(StageGeometryType)).Length];
        textureRepo = new Texture2DArray[Enum.GetNames(typeof(StageTextureSheetType)).Length];

        foreach (StageGeometryType type in Enum.GetValues(typeof(StageGeometryType)))
        {
            LoadStageGeometry(type);
        }
    }

    void Update()
    {
        
    }

    public void LoadStageTheme(string themeName)
    {
        foreach (StageTextureSheetType type in Enum.GetValues(typeof(StageTextureSheetType)))
        {
            LoadStageTexture(type, themeName);
        }

        LoadStageCellDefinition(themeName);
        LoadStageProps(themeName);
    }

    public Mesh GetStageMeshPart(StageGeometryType geometryType, string partName)
    {
        string key = Enum.GetName(typeof(StageGeometryType), geometryType) + partName;

        if (geometryRepo[(int) geometryType].ContainsKey(key))
        {
            return geometryRepo[(int) geometryType][key];
        }
        else
        {
            return null;
        }
    }

    private void LoadStageGeometry(StageGeometryType geometryType)
    {
        string name = Enum.GetName(typeof(StageGeometryType), geometryType);
        GameObject stageMeshObject = Resources.Load<GameObject>("Models/StageMeshes/" + name);

        if (stageMeshObject != null)
        {
            geometryRepo[(int)geometryType] = new Dictionary<string, Mesh>();
            MeshFilter[] childMeshFilters = stageMeshObject.GetComponentsInChildren<MeshFilter>();

            foreach (MeshFilter meshFilter in childMeshFilters)
            {
                geometryRepo[(int)geometryType].Add(meshFilter.gameObject.name, meshFilter.sharedMesh);
            }
        }
    }

    private void LoadStageTexture(StageTextureSheetType sheetType, string themeName)
    {
        string name = Enum.GetName(typeof(StageTextureSheetType), sheetType);
        Texture2DArray textureArray = Resources.Load<Texture2DArray>("Textures/StageTextures/" + name + themeName);
    }

    private void LoadStageCellDefinition(string themeName)
    {
        TextAsset definitionDataFile = Resources.Load<TextAsset>("Data/StageCells/StageCellDefinitions" + themeName + ".json");
    }

    private void LoadStageProps(string themeName)
    {

    }
}

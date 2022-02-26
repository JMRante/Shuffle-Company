using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    Albedo,
    Props,
    Normal
}

public class StageRepository : MonoBehaviour
{
    private Dictionary<string, Mesh>[] geometryRepo;
    private Texture2DArray[] textureRepo;
    private StageThemeDefinition themeRepo;
    private GameObject[] propRepo;

    private static readonly Dictionary<StageGeometryType, StageTextureDefinition> stageNormalOriginTable = new Dictionary<StageGeometryType, StageTextureDefinition>
    {
        { StageGeometryType.Square,       new StageTextureDefinition(0, 0, TexturePatternType.Blob) },
        { StageGeometryType.SmallBevel,   new StageTextureDefinition(0, 8, TexturePatternType.Blob) },
        { StageGeometryType.LargeBevel,   new StageTextureDefinition(0, 16, TexturePatternType.Blob) },
        { StageGeometryType.SmallCurve,   new StageTextureDefinition(0, 24, TexturePatternType.Blob) },
        { StageGeometryType.LargeCurve,   new StageTextureDefinition(8, 0, TexturePatternType.Blob) },
        // { StageGeometryType.WeakJagged,   new StageTextureDefinition(8, 8, TexturePatternType.Blob) },
        { StageGeometryType.WeakJagged,   new StageTextureDefinition(0, 0, TexturePatternType.Blob) },
        { StageGeometryType.StrongJagged, new StageTextureDefinition(8, 16, TexturePatternType.Blob) },
        { StageGeometryType.WeakWave,     new StageTextureDefinition(8, 24, TexturePatternType.Blob) },
        { StageGeometryType.StrongWave,   new StageTextureDefinition(16, 0, TexturePatternType.Blob) }
    };

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

        LoadStageThemeDefinition(themeName);
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
        string typeName = Enum.GetName(typeof(StageTextureSheetType), sheetType);
        Texture2DArray textureArray = Resources.Load<Texture2DArray>("Textures/StageTextures/" + themeName + "/" + themeName + typeName);
        textureRepo[(int)sheetType] = textureArray;
    }

    public void LoadStageTexturesToMaterial(Material material)
    {
        if (textureRepo != null && textureRepo.Length > 0)
        {
            for (int i = 0; i < textureRepo.Length; i++)
            {
                material.SetTexture("Main" + Enum.GetName(typeof(StageTextureSheetType), i), textureRepo[i]);
            }
        }
    }

    private void LoadStageThemeDefinition(string themeName)
    {
        TextAsset definitionDataFile = Resources.Load<TextAsset>("Data/StageThemes/" + themeName);
        themeRepo = StageThemeDefinition.createFromJSON(definitionDataFile.text);

        GameObject creatorDropdownObject = GameObject.Find("CellDropdown");

        if (creatorDropdownObject != null)
        {
            List<string> cellDefinitionNames = new List<string>();

            foreach (StageCellDefinition scd in themeRepo.stageCellDefinitions)
            {
                cellDefinitionNames.Add(scd.name);
            }

            Dropdown creatorDropdown = creatorDropdownObject.GetComponent<Dropdown>();
            creatorDropdown.ClearOptions();
            creatorDropdown.AddOptions(cellDefinitionNames);
        }
    }

    public StageThemeDefinition GetStageThemeDefinition()
    {
        return themeRepo;
    }

    private void LoadStageProps(string themeName)
    {

    }

    public StageTextureDefinition GetStageNormalTextureDefinition(StageGeometryType stageGeometryType)
    {
        return stageNormalOriginTable[stageGeometryType];
    }
}

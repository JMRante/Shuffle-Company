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
    stage256,
    stage512,
    stage1024,
    stageBlob1,
    stageBlob2,
    stageBlob3,
    stageBlob4,
    stageEdge,
    stageDecal
}

public class StageRepository : MonoBehaviour
{
    private Dictionary<string, Mesh>[] geometryRepo;
    private Texture2DArray[] textureRepo;
    private StageThemeDefinition themeRepo;
    private GameObject[] propRepo;

    private static readonly Dictionary<int, int> stageNormalTable = new Dictionary<int, int>
    {
        { 0, 0 },
        { 4, 1 },
        { 92, 2 },
        { 124, 3 },
        { 116, 4 },
        { 80, 5 },

        { 16, 8 },
        { 20, 9 },
        { 87, 10 },
        { 223, 11 },
        { 241, 12 },
        { 21, 13 },
        { 64, 14 },

        { 29, 16 },
        { 117, 17 },
        { 85, 18 },
        { 71, 19 },
        { 221, 20 },
        { 125, 21 },
        { 112, 22 },

        { 31, 24 },
        { 253, 25 },
        { 113, 26 },
        { 28, 27 },
        { 127, 28 },
        { 247, 29 },
        { 209, 30 },

        { 23, 32 },
        { 199, 33 },
        { 213, 34 },
        { 95, 35 },
        { 255, 36 },
        { 245, 37 },
        { 81, 38 },

        { 5, 40 },
        { 84, 41 },
        { 93, 42 },
        { 119, 43 },
        { 215, 44 },
        { 193, 45 },
        { 17, 46 },

        { 1, 49 },
        { 7, 50 },
        { 197, 51 },
        { 69, 52 },
        { 68, 53 },
        { 65, 54 }
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
        string name = Enum.GetName(typeof(StageTextureSheetType), sheetType);
        Texture2DArray textureArray = Resources.Load<Texture2DArray>("Textures/StageTextures/" + themeName + "/" + name + "Albedo" + themeName);
        textureRepo[(int)sheetType] = textureArray;
    }

    public void LoadStageTexturesToMaterial(Material material)
    {
        if (textureRepo != null && textureRepo.Length > 0)
        {
            for (int i = 0; i < textureRepo.Length; i++)
            {
                Debug.Log(Enum.GetName(typeof(StageTextureSheetType), i) + "Albedo  = " + textureRepo[i]);
                material.SetTexture(Enum.GetName(typeof(StageTextureSheetType), i) + "Albedo", textureRepo[i]);
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

    public int CalculateStageNormalIndex(Vector3Int cellPosition, Chunk chunk, Vector3Int direction)
    {
        Vector3Int north = Vector3Int.zero;
        Vector3Int east = Vector3Int.zero;

        if (direction == Vector3Int.up)
        {
            north = Vector3Int.forward;
            east = Vector3Int.right;
        }
        else if (direction == Vector3Int.down)
        {
            north = Vector3Int.forward;
            east = Vector3Int.right;
        }
        else if (direction == Vector3Int.forward)
        {
            north = Vector3Int.up;
            east = Vector3Int.left;
        }
        else if (direction == Vector3Int.back)
        {
            north = Vector3Int.up;
            east = Vector3Int.right;
        }
        else if (direction == Vector3Int.right)
        {
            north = Vector3Int.up;
            east = Vector3Int.forward;
        }
        else if (direction == Vector3Int.left)
        {
            north = Vector3Int.up;
            east = Vector3Int.back;
        }

        int stageNormalBlobIndex = 0;

        bool n = chunk.GetChunkCell(cellPosition + north).IsFilled();
        bool ne = chunk.GetChunkCell(cellPosition + north + east).IsFilled();
        bool e = chunk.GetChunkCell(cellPosition + east).IsFilled();
        bool se = chunk.GetChunkCell(cellPosition + -north + east).IsFilled();
        bool s = chunk.GetChunkCell(cellPosition + -north).IsFilled();
        bool sw = chunk.GetChunkCell(cellPosition + -north + -east).IsFilled();
        bool w = chunk.GetChunkCell(cellPosition + -east).IsFilled();
        bool nw = chunk.GetChunkCell(cellPosition + north + -east).IsFilled();

        // N
        if (n)
        {
            stageNormalBlobIndex += 1;
        }

        // NE
        if (ne && n && e)
        {
            stageNormalBlobIndex += 2;
        }

        // E
        if (e)
        {
            stageNormalBlobIndex += 4;
        }

        // SE
        if (se && s && e)
        {
            stageNormalBlobIndex += 8;
        }

        // S
        if (s)
        {
            stageNormalBlobIndex += 16;
        }

        // SW
        if (sw && s && w)
        {
            stageNormalBlobIndex += 32;
        }

        // W
        if (w)
        {
            stageNormalBlobIndex += 64;
        }

        // NW
        if (nw && n && w)
        {
            stageNormalBlobIndex += 128;
        }

        if (stageNormalTable.ContainsKey(stageNormalBlobIndex))
        {
            return stageNormalTable[stageNormalBlobIndex];
        }
        else
        {
            return 0;
        }
    }
}

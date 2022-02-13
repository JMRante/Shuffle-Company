using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageMeshCreator
{
    private ChunkCell emptyCell = new ChunkCell(0, 0);

    private Dictionary<int, Vector3Int[]> quadrantChecks;
    
    private StageRepository repo;

    public StageMeshCreator(StageRepository repo)
    {
        this.repo = repo;

        quadrantChecks = new Dictionary<int, Vector3Int[]>();
        quadrantChecks.Add(1, new Vector3Int[] { Vector3Int.forward, Vector3Int.right });
        quadrantChecks.Add(2, new Vector3Int[] { Vector3Int.forward, Vector3Int.left });
        quadrantChecks.Add(3, new Vector3Int[] { Vector3Int.back, Vector3Int.left });
        quadrantChecks.Add(4, new Vector3Int[] { Vector3Int.back, Vector3Int.right });
    }

    public List<CombineInstance> GetCellMesh(Chunk chunk, Vector3Int cell)
    {
        List<CombineInstance> combineList = new List<CombineInstance>();
        Matrix4x4 translationMatrix = Matrix4x4.Translate(cell);
        Matrix4x4 rotationMatrix = Matrix4x4.Rotate(Quaternion.AngleAxis(-90f, Vector3.up)) * Matrix4x4.Rotate(Quaternion.AngleAxis(-90f, Vector3.right));
        Matrix4x4 transformationMatrix = translationMatrix * rotationMatrix;

        ChunkCell cellDef = chunk.GetChunkCell(cell);
        StageGeometryType cellGeometryType = cellDef.GetGeometryType();

        for (int i = 1; i <= 4; i++)
        {
            string quadrantType = "";
            string insetType = "";

            Vector3Int[] quadrantCheck = quadrantChecks[i];

            bool quadrantZCheck = chunk.GetChunkCell(cell + quadrantCheck[0]).IsFilled();
            bool quadrantXCheck = chunk.GetChunkCell(cell + quadrantCheck[1]).IsFilled();

            bool insetZCheck = (chunk.GetChunkCell(cell).IsInsetForward() && quadrantCheck[0] == Vector3Int.forward) 
                || (chunk.GetChunkCell(cell).IsInsetBack() && quadrantCheck[0] == Vector3Int.back);
            bool insetXCheck = (chunk.GetChunkCell(cell).IsInsetLeft() && quadrantCheck[1] == Vector3Int.left) 
                || (chunk.GetChunkCell(cell).IsInsetRight() && quadrantCheck[1] == Vector3Int.right);

            bool insetAdjacentZCheck = quadrantZCheck
                && ((chunk.GetChunkCell(cell + quadrantCheck[0]).IsInsetLeft() && (i == 2 || i == 3) && !chunk.GetChunkCell(cell).IsInsetLeft())
                    || (chunk.GetChunkCell(cell + quadrantCheck[0]).IsInsetRight() && (i == 1 || i == 4) && !chunk.GetChunkCell(cell).IsInsetRight()));
            bool insetAdjacentXCheck = quadrantXCheck 
                && ((chunk.GetChunkCell(cell + quadrantCheck[1]).IsInsetForward() && (i == 1 || i == 2) && !chunk.GetChunkCell(cell).IsInsetForward())
                    || (chunk.GetChunkCell(cell + quadrantCheck[1]).IsInsetBack() && (i == 3 || i == 4) && !chunk.GetChunkCell(cell).IsInsetBack()));

            if (insetAdjacentZCheck)
            {
                CombineInstance ci = new CombineInstance();
                ci.mesh = CreateMeshPart(repo.GetStageMeshPart(cellGeometryType, "EdgeZ_W" + i + "_IS"), cellDef, cell, chunk, quadrantCheck[0]);
                ci.transform = transformationMatrix;
                combineList.Add(ci);
            }

            if (insetAdjacentXCheck)
            {
                CombineInstance ci = new CombineInstance();
                ci.mesh = CreateMeshPart(repo.GetStageMeshPart(cellGeometryType, "EdgeX_W" + i + "_IS"), cellDef, cell, chunk, quadrantCheck[1]);
                ci.transform = transformationMatrix;
                combineList.Add(ci);
            }

            if (quadrantZCheck && !quadrantXCheck)
            {
                quadrantType = "EdgeX";

                if (insetXCheck)
                {
                    insetType = "_I";
                }

                CombineInstance ci = new CombineInstance();
                ci.mesh = CreateMeshPart(repo.GetStageMeshPart(cellGeometryType, quadrantType + "_W" + i + insetType), cellDef, cell, chunk, quadrantCheck[1]);
                ci.transform = transformationMatrix;
                combineList.Add(ci);
            }
            else if (!quadrantZCheck && quadrantXCheck)
            {
                quadrantType = "EdgeZ";

                if (insetZCheck)
                {
                    insetType = "_I";
                }

                CombineInstance ci = new CombineInstance();
                ci.mesh = CreateMeshPart(repo.GetStageMeshPart(cellGeometryType, quadrantType + "_W" + i + insetType), cellDef, cell, chunk, quadrantCheck[0]);
                ci.transform = transformationMatrix;
                combineList.Add(ci);
            }
            else if (!quadrantZCheck && !quadrantXCheck)
            {
                quadrantType = "Corner";

                if (insetXCheck && insetZCheck)
                {
                    insetType = "_IXZ";
                }
                else if (insetXCheck)
                {
                    insetType = "_IZ";
                }
                else if (insetZCheck)
                {
                    insetType = "_IX";
                }

                CombineInstance ciL = new CombineInstance();
                ciL.mesh = CreateMeshPart(repo.GetStageMeshPart(cellGeometryType, quadrantType + "_W" + i + "L" + insetType), cellDef, cell, chunk, (i == 2 || i == 4) ? quadrantCheck[1] : quadrantCheck[0]);
                ciL.transform = transformationMatrix;
                combineList.Add(ciL);

                CombineInstance ciR = new CombineInstance();
                ciR.mesh = CreateMeshPart(repo.GetStageMeshPart(cellGeometryType, quadrantType + "_W" + i + "R" + insetType), cellDef, cell, chunk, (i == 2 || i == 4) ? quadrantCheck[0] : quadrantCheck[1]);
                ciR.transform = transformationMatrix;
                combineList.Add(ciR);
            }
            else
            {
                quadrantType = "Center";
            }

            if (!chunk.GetChunkCell(cell + Vector3Int.up).IsFilled() ||
                (!chunk.GetChunkCell(cell + Vector3Int.up + quadrantCheck[0]).IsFilled() && chunk.GetChunkCell(cell + quadrantCheck[0]).IsFilled()) ||
                (!chunk.GetChunkCell(cell + Vector3Int.up + quadrantCheck[1]).IsFilled() && chunk.GetChunkCell(cell + quadrantCheck[1]).IsFilled()) ||
                (!chunk.GetChunkCell(cell + Vector3Int.up + quadrantCheck[0] + quadrantCheck[1]).IsFilled() && chunk.GetChunkCell(cell + quadrantCheck[0] + quadrantCheck[1]).IsFilled()) ||
                (chunk.GetChunkCell(cell + Vector3Int.up).IsInsetForward() && (i == 1 || i == 2)) ||
                (chunk.GetChunkCell(cell + Vector3Int.up).IsInsetBack() && (i == 3 || i == 4)) ||
                (chunk.GetChunkCell(cell + Vector3Int.up).IsInsetLeft() && (i == 2 || i == 3)) ||
                (chunk.GetChunkCell(cell + Vector3Int.up).IsInsetRight() && (i == 1 || i == 4)))
            {
                CombineInstance ci = new CombineInstance();
                ci.mesh = CreateMeshPart(repo.GetStageMeshPart(cellGeometryType, quadrantType + "_T" + i + insetType), cellDef, cell, chunk, Vector3Int.up);
                ci.transform = transformationMatrix;
                combineList.Add(ci);
            }

            if (!chunk.GetChunkCell(cell + Vector3Int.down).IsFilled() ||
                (!chunk.GetChunkCell(cell + Vector3Int.down + quadrantCheck[0]).IsFilled() && chunk.GetChunkCell(cell + quadrantCheck[0]).IsFilled()) ||
                (!chunk.GetChunkCell(cell + Vector3Int.down + quadrantCheck[1]).IsFilled() && chunk.GetChunkCell(cell + quadrantCheck[1]).IsFilled()) ||
                (!chunk.GetChunkCell(cell + Vector3Int.down + quadrantCheck[0] + quadrantCheck[1]).IsFilled() && chunk.GetChunkCell(cell + quadrantCheck[0] + quadrantCheck[1]).IsFilled()) ||
                (chunk.GetChunkCell(cell + Vector3Int.down).IsInsetForward() && (i == 1 || i == 2)) ||
                (chunk.GetChunkCell(cell + Vector3Int.down).IsInsetBack() && (i == 3 || i == 4)) ||
                (chunk.GetChunkCell(cell + Vector3Int.down).IsInsetLeft() && (i == 2 || i == 3)) ||
                (chunk.GetChunkCell(cell + Vector3Int.down).IsInsetRight() && (i == 1 || i == 4)))
            {
                CombineInstance ci = new CombineInstance();
                ci.mesh = CreateMeshPart(repo.GetStageMeshPart(cellGeometryType, quadrantType + "_B" + i + insetType), cellDef, cell, chunk, Vector3Int.down);
                ci.transform = transformationMatrix;
                combineList.Add(ci);
            }
        }

        return combineList;
    }

    private Mesh CreateMeshPart(Mesh meshPrototype, ChunkCell cellDef, Vector3Int cellPosition, Chunk chunk, Vector3Int direction)
    {
        int layer = 0;
        float textureIndex = 0;
        Mesh mesh = new Mesh();

        List<Vector3> vertices = new List<Vector3>();
        foreach (Vector3 vertex in meshPrototype.vertices)
        {
            vertices.Add(vertex);
        }
        mesh.SetVertices(vertices);

        List<int> triangles = new List<int>();
        foreach (int triangle in meshPrototype.triangles)
        {
            triangles.Add(triangle);
        }
        mesh.SetTriangles(triangles, 0);

        List<Vector2> uvs = new List<Vector2>();
        foreach (Vector2 uv in meshPrototype.uv)
        {
            uvs.Add(uv);
        }
        mesh.SetUVs(0, uvs);

        List<Vector4> uvs2 = new List<Vector4>();
        foreach (Vector2 uv in meshPrototype.uv)
        {
            uvs2.Add(new Vector4(textureIndex, 0f, 0f, 0f));
        }
        mesh.SetUVs(1, uvs2);

        List<Vector4> uvs3 = new List<Vector4>();
        foreach (Vector2 uv in meshPrototype.uv)
        {
            uvs3.Add(new Vector4(0f, 0f, 0f, cellDef.geometryType));
        }
        mesh.SetUVs(2, uvs3);

        float stageNormalIndex = repo.CalculateStageNormalIndex(cellPosition, chunk, direction);

        List<Vector4> uvs4 = new List<Vector4>();
        foreach (Vector2 uv in meshPrototype.uv)
        {
            uvs4.Add(new Vector4(stageNormalIndex, 0f, 0f, 0f));
        }
        mesh.SetUVs(3, uvs4);

        List<Vector3> normals = new List<Vector3>();
        foreach (Vector3 normal in meshPrototype.normals)
        {
            normals.Add(normal);
        }
        mesh.SetNormals(normals);

        List<Vector4> tangents = new List<Vector4>();
        foreach (Vector4 tangent in meshPrototype.tangents)
        {
            tangents.Add(tangent);
        }
        mesh.SetTangents(tangents);

        List<Color> colors = new List<Color>();
        foreach (Color color in meshPrototype.colors)
        {
            colors.Add(color);
        }
        mesh.SetColors(colors);

        return mesh;
    }
}

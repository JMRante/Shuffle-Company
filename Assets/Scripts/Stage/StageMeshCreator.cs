using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageMeshCreator
{
    private ChunkCell emptyCell = new ChunkCell(0);

    private Dictionary<int, Vector3Int[]> quadrantChecks;
    
    private StageGeometryRepo geometryRepo;

    public StageMeshCreator(StageGeometryRepo geometryRepo)
    {
        this.geometryRepo = geometryRepo;

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

        for (int i = 1; i <= 4; i++)
        {
            string quadrantType = "";

            Vector3Int[] quadrantCheck = quadrantChecks[i];

            bool quadrantZCheck = chunk.GetChunkCell(cell + quadrantCheck[0]).tilesetIndex != 0;
            bool quadrantXCheck = chunk.GetChunkCell(cell + quadrantCheck[1]).tilesetIndex != 0;

            if (quadrantZCheck && !quadrantXCheck)
            {
                quadrantType = "EdgeX";

                CombineInstance ci = new CombineInstance();
                ci.mesh = CreateMeshPart(geometryRepo.jaggedStageMeshDefinition.GetStageMeshPart(quadrantType + "_W" + i), 0, 0f);
                ci.transform = transformationMatrix;
                combineList.Add(ci);
            }
            else if (!quadrantZCheck && quadrantXCheck)
            {
                quadrantType = "EdgeZ";

                CombineInstance ci = new CombineInstance();
                ci.mesh = CreateMeshPart(geometryRepo.jaggedStageMeshDefinition.GetStageMeshPart(quadrantType + "_W" + i), 0, 0f);
                ci.transform = transformationMatrix;
                combineList.Add(ci);
            }
            else if (!quadrantZCheck && !quadrantXCheck)
            {
                quadrantType = "Corner";

                CombineInstance ciL = new CombineInstance();
                ciL.mesh = CreateMeshPart(geometryRepo.jaggedStageMeshDefinition.GetStageMeshPart(quadrantType + "_W" + i + "L"), 0, 0f);
                ciL.transform = transformationMatrix;
                combineList.Add(ciL);

                CombineInstance ciR = new CombineInstance();
                ciR.mesh = CreateMeshPart(geometryRepo.jaggedStageMeshDefinition.GetStageMeshPart(quadrantType + "_W" + i + "R"), 0, 0f);
                ciR.transform = transformationMatrix;
                combineList.Add(ciR);
            }
            else
            {
                quadrantType = "Center";
            }

            if (chunk.GetChunkCell(cell + Vector3Int.up).tilesetIndex == emptyCell.tilesetIndex ||
                chunk.GetChunkCell(cell + Vector3Int.up + quadrantCheck[0]).tilesetIndex == emptyCell.tilesetIndex ||
                chunk.GetChunkCell(cell + Vector3Int.up + quadrantCheck[1]).tilesetIndex == emptyCell.tilesetIndex ||
                chunk.GetChunkCell(cell + Vector3Int.up + quadrantCheck[0] + quadrantCheck[1]).tilesetIndex == emptyCell.tilesetIndex)
            {
                CombineInstance ci = new CombineInstance();
                ci.mesh = CreateMeshPart(geometryRepo.jaggedStageMeshDefinition.GetStageMeshPart(quadrantType + "_T" + i), 0, 1f);
                ci.transform = transformationMatrix;
                combineList.Add(ci);
            }

            if (chunk.GetChunkCell(cell + Vector3Int.down).tilesetIndex == emptyCell.tilesetIndex ||
                chunk.GetChunkCell(cell + Vector3Int.down + quadrantCheck[0]).tilesetIndex == emptyCell.tilesetIndex ||
                chunk.GetChunkCell(cell + Vector3Int.down + quadrantCheck[1]).tilesetIndex == emptyCell.tilesetIndex ||
                chunk.GetChunkCell(cell + Vector3Int.down + quadrantCheck[0] + quadrantCheck[1]).tilesetIndex == emptyCell.tilesetIndex)
            {
                CombineInstance ci = new CombineInstance();
                ci.mesh = CreateMeshPart(geometryRepo.jaggedStageMeshDefinition.GetStageMeshPart(quadrantType + "_B" + i), 0, 1f);
                ci.transform = transformationMatrix;
                combineList.Add(ci);
            }
        }

        return combineList;
    }

    private Mesh CreateMeshPart(Mesh meshPrototype, int layer, float textureIndex)
    {
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

        List<Vector2> uvs2 = new List<Vector2>();
        foreach (Vector2 uv in meshPrototype.uv)
        {
            uvs2.Add(new Vector2(textureIndex, 0f));
        }
        mesh.SetUVs(1, uvs2);

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

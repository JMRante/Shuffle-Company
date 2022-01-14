using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageMeshCreator
{
    private JaggedStageMeshDefinition jaggedStageMeshDefinition;

    private ChunkCell emptyCell = new ChunkCell(0);

    private Dictionary<int, Vector3Int[]> quadrantChecks;

    public StageMeshCreator()
    {
        jaggedStageMeshDefinition = new JaggedStageMeshDefinition();

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
                ci.mesh = jaggedStageMeshDefinition.GetStageMeshPart(quadrantType + "_W" + i);
                ci.transform = transformationMatrix;
                combineList.Add(ci);
            }
            else if (!quadrantZCheck && quadrantXCheck)
            {
                quadrantType = "EdgeZ";

                CombineInstance ci = new CombineInstance();
                ci.mesh = jaggedStageMeshDefinition.GetStageMeshPart(quadrantType + "_W" + i);
                ci.transform = transformationMatrix;
                combineList.Add(ci);
            }
            else if (!quadrantZCheck && !quadrantXCheck)
            {
                quadrantType = "Corner";

                CombineInstance ciL = new CombineInstance();
                ciL.mesh = jaggedStageMeshDefinition.GetStageMeshPart(quadrantType + "_W" + i + "L");
                ciL.transform = transformationMatrix;
                combineList.Add(ciL);

                CombineInstance ciR = new CombineInstance();
                ciR.mesh = jaggedStageMeshDefinition.GetStageMeshPart(quadrantType + "_W" + i + "R");
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
                ci.mesh = jaggedStageMeshDefinition.GetStageMeshPart(quadrantType + "_T" + i);
                ci.transform = transformationMatrix;
                combineList.Add(ci);
            }

            if (chunk.GetChunkCell(cell + Vector3Int.down).tilesetIndex == emptyCell.tilesetIndex ||
                chunk.GetChunkCell(cell + Vector3Int.down + quadrantCheck[0]).tilesetIndex == emptyCell.tilesetIndex ||
                chunk.GetChunkCell(cell + Vector3Int.down + quadrantCheck[1]).tilesetIndex == emptyCell.tilesetIndex ||
                chunk.GetChunkCell(cell + Vector3Int.down + quadrantCheck[0] + quadrantCheck[1]).tilesetIndex == emptyCell.tilesetIndex)
            {
                CombineInstance ci = new CombineInstance();
                ci.mesh = jaggedStageMeshDefinition.GetStageMeshPart(quadrantType + "_B" + i);
                ci.transform = transformationMatrix;
                combineList.Add(ci);
            }
        }


        // foreach (Vector3 octantPosition in octantPositions)
        // {
        //     float octantRotation = 0f;

        //     if (octantPosition.x == -1f && octantPosition.z == 1f)
        //     {
        //         octantRotation = 90f;
        //     }
        //     else if (octantPosition.x == -1f && octantPosition.z == -1f)
        //     {
        //         octantRotation = 180f;
        //     }
        //     else if (octantPosition.x == 1f && octantPosition.z == -1f)
        //     {
        //         octantRotation = 270f;
        //     }

        //     Vector3Int octantLeft = Vector3Int.RoundToInt(Quaternion.AngleAxis(octantRotation, Vector3.up) * Vector3.forward);
        //     Vector3Int octantRight = Vector3Int.RoundToInt(Quaternion.AngleAxis(octantRotation, Vector3.up) * Vector3.right);

        //     bool isLeftFilled = chunk.GetChunkCell(cell + octantLeft).tilesetIndex != 0;
        //     // bool isCenterFilled = chunk.GetChunkCell(cell + octantLeft + octantRight).tilesetIndex != 0;
        //     bool isRightFilled = chunk.GetChunkCell(cell + octantRight).tilesetIndex != 0;

        //     if (isLeftFilled && !isRightFilled)
        //     {
        //         CombineInstance ci = new CombineInstance();
        //         ci.mesh = octantPosition.y > 0 ? cubicStageMeshDefinition.edgeAboveLeft : cubicStageMeshDefinition.edgeBelowLeft;
        //         ci.transform = Matrix4x4.Translate(cell + (octantPosition * 0.25f)) * Matrix4x4.Rotate(Quaternion.AngleAxis(-180f - octantRotation, Vector3.up)) * Matrix4x4.Rotate(Quaternion.AngleAxis(90f, Vector3.right));
        //         combineList.Add(ci);
        //     } 
        //     else if (!isLeftFilled && isRightFilled)
        //     {
        //         CombineInstance ci = new CombineInstance();
        //         ci.mesh = octantPosition.y > 0 ? cubicStageMeshDefinition.edgeAboveRight : cubicStageMeshDefinition.edgeBelowRight;
        //         ci.transform = Matrix4x4.Translate(cell + (octantPosition * 0.25f)) * Matrix4x4.Rotate(Quaternion.AngleAxis(-90f - octantRotation, Vector3.up)) * Matrix4x4.Rotate(Quaternion.AngleAxis(90f, Vector3.right));
        //         combineList.Add(ci);
        //     }
        //     else if (!isLeftFilled && !isRightFilled)
        //     {
        //         CombineInstance ciL = new CombineInstance();
        //         ciL.mesh = octantPosition.y > 0 ? cubicStageMeshDefinition.outerCornerAboveLeft : cubicStageMeshDefinition.outerCornerBelowLeft;
        //         ciL.transform = Matrix4x4.Translate(cell + (octantPosition * 0.25f)) * Matrix4x4.Rotate(Quaternion.AngleAxis(-90f - octantRotation, Vector3.up)) * Matrix4x4.Rotate(Quaternion.AngleAxis(90f, Vector3.right));
        //         combineList.Add(ciL);

        //         CombineInstance ciR = new CombineInstance();
        //         ciR.mesh = octantPosition.y > 0 ? cubicStageMeshDefinition.outerCornerAboveRight : cubicStageMeshDefinition.outerCornerBelowRight;
        //         ciR.transform = Matrix4x4.Translate(cell + (octantPosition * 0.25f)) * Matrix4x4.Rotate(Quaternion.AngleAxis(-90f - octantRotation, Vector3.up)) * Matrix4x4.Rotate(Quaternion.AngleAxis(90f, Vector3.right));
        //         combineList.Add(ciR);
        //     }

        //     if (chunk.GetChunkCell(cell + Vector3Int.up).tilesetIndex == emptyCell.tilesetIndex)
        //     {
        //         CombineInstance ci = new CombineInstance();
        //         ci.mesh = cubicStageMeshDefinition.centerCap;
        //         ci.transform = Matrix4x4.Translate(cell + (octantPosition * 0.25f)) * Matrix4x4.Rotate(Quaternion.AngleAxis(90f * octantPosition.y, Vector3.right));
        //         combineList.Add(ci);
        //     }
        // }

        return combineList;
    }
}

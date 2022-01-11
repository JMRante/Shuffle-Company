using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageMeshCreator
{
    private CubicStageMeshDefinition cubicStageMeshDefinition;

    private ChunkCell emptyCell = new ChunkCell(0);

    private Vector3[] octantPositions = new Vector3[] 
    { 
        new Vector3(1f, 1f, 1f),
        new Vector3(-1f, 1f, 1f),
        new Vector3(1f, -1f, 1f),
        new Vector3(-1f, -1f, 1f),
        new Vector3(1f, 1f, -1f),
        new Vector3(-1f, 1f, -1f),
        new Vector3(1f, -1f, -1f),
        new Vector3(-1f, -1f, -1f),
    };

    public StageMeshCreator()
    {
        cubicStageMeshDefinition = new CubicStageMeshDefinition();
    }

    public List<CombineInstance> GetCellMesh(Chunk chunk, Vector3Int cell)
    {
        List<CombineInstance> combineList = new List<CombineInstance>();

        foreach (Vector3 octantPosition in octantPositions)
        {
            float octantRotation = 0f;

            if (octantPosition.x == -1f && octantPosition.z == 1f)
            {
                octantRotation = 90f;
            }
            else if (octantPosition.x == -1f && octantPosition.z == -1f)
            {
                octantRotation = 180f;
            }
            else if (octantPosition.x == 1f && octantPosition.z == -1f)
            {
                octantRotation = 270f;
            }

            Vector3Int octantLeft = Vector3Int.RoundToInt(Quaternion.AngleAxis(octantRotation, Vector3.up) * Vector3.forward);
            Vector3Int octantRight = Vector3Int.RoundToInt(Quaternion.AngleAxis(octantRotation, Vector3.up) * Vector3.right);

            bool isLeftFilled = chunk.GetChunkCell(cell + octantLeft).tilesetIndex != 0;
            // bool isCenterFilled = chunk.GetChunkCell(cell + octantLeft + octantRight).tilesetIndex != 0;
            bool isRightFilled = chunk.GetChunkCell(cell + octantRight).tilesetIndex != 0;

            if (isLeftFilled && !isRightFilled)
            {
                CombineInstance ci = new CombineInstance();
                ci.mesh = octantPosition.y > 0 ? cubicStageMeshDefinition.edgeAboveLeft : cubicStageMeshDefinition.edgeBelowLeft;
                ci.transform = Matrix4x4.Translate(cell + (octantPosition * 0.25f)) * Matrix4x4.Rotate(Quaternion.AngleAxis(-180f - octantRotation, Vector3.up)) * Matrix4x4.Rotate(Quaternion.AngleAxis(90f, Vector3.right));
                combineList.Add(ci);
            } 
            else if (!isLeftFilled && isRightFilled)
            {
                CombineInstance ci = new CombineInstance();
                ci.mesh = octantPosition.y > 0 ? cubicStageMeshDefinition.edgeAboveRight : cubicStageMeshDefinition.edgeBelowRight;
                ci.transform = Matrix4x4.Translate(cell + (octantPosition * 0.25f)) * Matrix4x4.Rotate(Quaternion.AngleAxis(-90f - octantRotation, Vector3.up)) * Matrix4x4.Rotate(Quaternion.AngleAxis(90f, Vector3.right));
                combineList.Add(ci);
            }
            else if (!isLeftFilled && !isRightFilled)
            {
                CombineInstance ciL = new CombineInstance();
                ciL.mesh = octantPosition.y > 0 ? cubicStageMeshDefinition.outerCornerAboveLeft : cubicStageMeshDefinition.outerCornerBelowLeft;
                ciL.transform = Matrix4x4.Translate(cell + (octantPosition * 0.25f)) * Matrix4x4.Rotate(Quaternion.AngleAxis(-90f - octantRotation, Vector3.up)) * Matrix4x4.Rotate(Quaternion.AngleAxis(90f, Vector3.right));
                combineList.Add(ciL);

                CombineInstance ciR = new CombineInstance();
                ciR.mesh = octantPosition.y > 0 ? cubicStageMeshDefinition.outerCornerAboveRight : cubicStageMeshDefinition.outerCornerBelowRight;
                ciR.transform = Matrix4x4.Translate(cell + (octantPosition * 0.25f)) * Matrix4x4.Rotate(Quaternion.AngleAxis(-90f - octantRotation, Vector3.up)) * Matrix4x4.Rotate(Quaternion.AngleAxis(90f, Vector3.right));
                combineList.Add(ciR);
            }

            if (chunk.GetChunkCell(cell + Vector3Int.up).tilesetIndex == emptyCell.tilesetIndex)
            {
                CombineInstance ci = new CombineInstance();
                ci.mesh = cubicStageMeshDefinition.centerCap;
                ci.transform = Matrix4x4.Translate(cell + (octantPosition * 0.25f)) * Matrix4x4.Rotate(Quaternion.AngleAxis(90f * octantPosition.y, Vector3.right));
                combineList.Add(ci);
            }
        }

        return combineList;
    }
}

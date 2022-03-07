using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawBox : CreatorOperation
{
    public CreatorOperationType type;
    public Vector3 positionStart;
    public Vector3 positionEnd;
    public ChunkCell brush;
    public ChunkCell[,,] previousBrushes;
    public StageChunks stageChunks;

    public DrawBox(Vector3 positionStart, Vector3 positionEnd, ChunkCell brush, StageChunks stageChunks)
    {
        this.type = CreatorOperationType.Point;
        this.positionStart = new Vector3(Mathf.Min(positionStart.x, positionEnd.x), Mathf.Min(positionStart.y, positionEnd.y), Mathf.Min(positionStart.z, positionEnd.z));
        this.positionEnd = new Vector3(Mathf.Max(positionStart.x, positionEnd.x), Mathf.Max(positionStart.y, positionEnd.y), Mathf.Max(positionStart.z, positionEnd.z)) + Vector3.one;
        this.brush = brush;
        this.stageChunks = stageChunks;
    }

    public override void operate()
    {
        Vector3Int extents = Vector3Int.RoundToInt(positionEnd - positionStart);
        Vector3Int positionStartInt = Vector3Int.RoundToInt(positionStart);
        Vector3Int positionEndInt = Vector3Int.RoundToInt(positionEnd);
        previousBrushes = new ChunkCell[extents.x, extents.y, extents.z];

        for (int z = positionStartInt.z; z < positionEndInt.z; z++)
        {
            for (int y = positionStartInt.y; y < positionEndInt.y; y++)
            {
                for (int x = positionStartInt.x; x < positionEndInt.x; x++)
                {
                    Vector3 drawPosition = new Vector3(x, y, z);
                    previousBrushes[x - positionStartInt.x, y - positionStartInt.y, z - positionStartInt.z] = stageChunks.GetChunkCell(drawPosition);
                    stageChunks.Draw(drawPosition, brush);
                }
            }
        }

        stageChunks.Refresh();
    }

    public override void reverse()
    {
        Vector3Int positionStartInt = Vector3Int.RoundToInt(positionStart);
        Vector3Int positionEndInt = Vector3Int.RoundToInt(positionEnd);

        for (int z = positionStartInt.z; z < positionEndInt.z; z++)
        {
            for (int y = positionStartInt.y; y < positionEndInt.y; y++)
            {
                for (int x = positionStartInt.x; x < positionEndInt.x; x++)
                {
                    Vector3 drawPosition = new Vector3(x, y, z);
                    stageChunks.Draw(drawPosition, previousBrushes[x - positionStartInt.x, y - positionStartInt.y, z - positionStartInt.z]);
                }
            }
        }

        stageChunks.Refresh();
    }
}

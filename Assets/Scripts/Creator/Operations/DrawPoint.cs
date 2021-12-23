using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawPoint : CreatorOperation
{
    public CreatorOperationType type;
    public Vector3 position;
    public ChunkCell brush;
    public ChunkCell previousBrush;
    public StageChunks stageChunks;

    public DrawPoint(Vector3 position, ChunkCell brush, StageChunks stageChunks, bool isErase)
    {
        this.type = isErase ? CreatorOperationType.Point : CreatorOperationType.ErasePoint;
        this.position = position;
        this.brush = brush;
        this.stageChunks = stageChunks;
    }

    public override void operate() 
    {
        previousBrush = stageChunks.GetChunkCell(position);
        stageChunks.Draw(position, brush);
    }

    public override void reverse() 
    {
        stageChunks.Draw(position, previousBrush);
    }
}

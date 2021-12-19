using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct ChunkCell
{
    public int tilesetIndex;

    public ChunkCell(int tilesetIndex)
    {
        this.tilesetIndex = tilesetIndex;
    }
}
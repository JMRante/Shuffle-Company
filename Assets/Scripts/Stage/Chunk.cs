using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chunk : MonoBehaviour
{
    public const int CHUNK_WIDTH = 5;
    public const int CHUNK_HEIGHT = 20;
    public const int CHUNK_DEPTH = 5;

    public ChunkCell[,,] chunkData;

    public Chunk()
    {
        chunkData = new ChunkCell[CHUNK_WIDTH, CHUNK_HEIGHT, CHUNK_DEPTH];
    }
}

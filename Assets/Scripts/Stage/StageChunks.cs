using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageChunks : MonoBehaviour
{
    public GameObject chunkPrefab;

    public const int TOTAL_CELL_WIDTH = 200;
    public const int TOTAL_CELL_HEIGHT = 100;
    public const int TOTAL_CELL_DEPTH = 200;

    public const int STAGE_WIDTH = TOTAL_CELL_WIDTH / Chunk.CHUNK_WIDTH;
    public const int STAGE_HEIGHT = TOTAL_CELL_HEIGHT / Chunk.CHUNK_HEIGHT;
    public const int STAGE_DEPTH = TOTAL_CELL_DEPTH / Chunk.CHUNK_DEPTH;

    void Start()
    {
        GenerateChunks();
    }

    void Update()
    {
        
    }

    public void GenerateChunks()
    {
        for (int z = 0; z < STAGE_DEPTH; z++)
        {
            for (int y = 0; y < STAGE_HEIGHT; y++)
            {
                for (int x = 0; x < STAGE_WIDTH; x++)
                {
                    Vector3 chunkPosition = new Vector3(
                        (x * Chunk.CHUNK_WIDTH) - (TOTAL_CELL_WIDTH / 2) + (Chunk.CHUNK_WIDTH / 2f), 
                        (y * Chunk.CHUNK_HEIGHT) - (TOTAL_CELL_HEIGHT / 2) + (Chunk.CHUNK_HEIGHT / 2f),
                        (z * Chunk.CHUNK_DEPTH) - (TOTAL_CELL_DEPTH / 2) + (Chunk.CHUNK_DEPTH / 2f));

                    GameObject chunk = GameObject.Instantiate(chunkPrefab, chunkPosition, Quaternion.identity, transform);
                    chunk.name = "Chunk_" + x + "_" + y + "_" + z;
                }
            }
        }
    }
}

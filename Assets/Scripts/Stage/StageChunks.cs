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

    public const int CHUNK_START_X = 0; 
    public const int CHUNK_START_Y = 0;
    public const int CHUNK_START_Z = 0;

    public const float CHUNK_START_OFFSET_X = -0.5f;
    public const float CHUNK_START_OFFSET_Y = -0.5f;
    public const float CHUNK_START_OFFSET_Z = -0.5f;

    public Chunk[,,] chunks;

    void Start()
    {
        GenerateChunks();
    }

    void Update()
    {
        
    }

    public void GenerateChunks()
    {
        chunks = new Chunk[STAGE_WIDTH, STAGE_HEIGHT, STAGE_DEPTH];

        for (int z = 0; z < STAGE_DEPTH; z++)
        {
            for (int y = 0; y < STAGE_HEIGHT; y++)
            {
                for (int x = 0; x < STAGE_WIDTH; x++)
                {
                    Vector3 chunkPosition = new Vector3(
                        (x * Chunk.CHUNK_WIDTH) + CHUNK_START_X + CHUNK_START_OFFSET_X + (Chunk.CHUNK_WIDTH / 2f), 
                        (y * Chunk.CHUNK_HEIGHT) + CHUNK_START_Y + CHUNK_START_OFFSET_Y + (Chunk.CHUNK_HEIGHT / 2f),
                        (z * Chunk.CHUNK_DEPTH) + CHUNK_START_Z + CHUNK_START_OFFSET_Z + (Chunk.CHUNK_DEPTH / 2f));

                    GameObject chunk = GameObject.Instantiate(chunkPrefab, chunkPosition, Quaternion.identity, transform);
                    chunk.name = "Chunk_" + x + "_" + y + "_" + z;
                    chunks[x, y, z] = chunk.GetComponent<Chunk>();
                }
            }
        }
    }

    public Vector3Int WorldPositionToChunkPosition(Vector3Int position)
    {
        return new Vector3Int(position.x % Chunk.CHUNK_WIDTH, position.y % Chunk.CHUNK_HEIGHT, position.z % Chunk.CHUNK_DEPTH);
    }

    public Chunk GetChunkAtPosition(Vector3 position)
    {
        if (position.x < CHUNK_START_X || position.x > CHUNK_START_X + TOTAL_CELL_WIDTH
            || position.y < CHUNK_START_Y || position.y > CHUNK_START_Y + TOTAL_CELL_HEIGHT
            || position.z < CHUNK_START_Z || position.z > CHUNK_START_Z + TOTAL_CELL_DEPTH)
        {
            return null;
        }
        else
        {
            return chunks[Mathf.FloorToInt(position.x / STAGE_WIDTH), Mathf.FloorToInt(position.y / STAGE_HEIGHT), Mathf.FloorToInt(position.z / STAGE_DEPTH)];
        }
    }

    public void Draw(Vector3Int position, ChunkCell brush)
    {

    }

    public void Erase(Vector3Int position)
    {

    }
}
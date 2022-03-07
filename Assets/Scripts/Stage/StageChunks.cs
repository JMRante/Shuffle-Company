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

    public string stageTheme = "TestWorld1";

    public StageRepository stageRepo;
    private StageMeshCreator stageMeshCreator;

    void Start()
    {
        stageRepo = GetComponent<StageRepository>();
        stageMeshCreator = new StageMeshCreator(stageRepo);

        stageRepo.LoadStageTheme(stageTheme);

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
                    chunks[x, y, z].stageMeshCreator = stageMeshCreator;
                    chunks[x, y, z].chunkManager = this;

                    Material chunkMaterial = chunks[x, y, z].GetComponentInChildren<MeshRenderer>().sharedMaterial;
                    stageRepo.LoadStageTexturesToMaterial(chunkMaterial);
                }
            }
        }
    }

    public static Vector3Int WorldPositionToChunkPosition(Vector3 position)
    {
        return new Vector3Int(
            Mathf.CeilToInt(position.x) % Chunk.CHUNK_WIDTH, 
            Mathf.CeilToInt(position.y) % Chunk.CHUNK_HEIGHT, 
            Mathf.CeilToInt(position.z) % Chunk.CHUNK_DEPTH);
    }

    public static Vector3 ChunkPositionToWorldPosition(Vector3Int position, Vector3 chunkPosition)
    {
        return new Vector3(
            (chunkPosition.x - Mathf.RoundToInt(Chunk.CHUNK_WIDTH / 2f)) + position.x, 
            (chunkPosition.y - Mathf.RoundToInt(Chunk.CHUNK_HEIGHT / 2f)) + position.y, 
            (chunkPosition.z - Mathf.RoundToInt(Chunk.CHUNK_DEPTH / 2f)) + position.z);
    }

    public Chunk GetChunkAtPosition(Vector3 position)
    {
        if (position.x < CHUNK_START_X || position.x >= CHUNK_START_X + TOTAL_CELL_WIDTH
            || position.y < CHUNK_START_Y || position.y >= CHUNK_START_Y + TOTAL_CELL_HEIGHT
            || position.z < CHUNK_START_Z || position.z >= CHUNK_START_Z + TOTAL_CELL_DEPTH)
        {
            return null;
        }
        else
        {
            return chunks[
                Mathf.FloorToInt(position.x / (float)Chunk.CHUNK_WIDTH), 
                Mathf.FloorToInt(position.y / (float)Chunk.CHUNK_HEIGHT), 
                Mathf.FloorToInt(position.z / (float)Chunk.CHUNK_DEPTH)];
        }
    }

    public ChunkCell GetChunkCell(Vector3 position)
    {
        Chunk chunk = GetChunkAtPosition(position);

        if (chunk != null)
        {
            Vector3Int chunkPosition = WorldPositionToChunkPosition(position);
            return chunk.GetChunkCell(chunkPosition);
        }
        else
        {
            return new ChunkCell(0, 0);
        }
    }

    public void Draw(Vector3 position, ChunkCell brush)
    {
        Chunk chunk = GetChunkAtPosition(position);

        if (chunk != null)
        {
            Vector3Int chunkPosition = WorldPositionToChunkPosition(position);
            chunk.chunkData[chunkPosition.x, chunkPosition.y, chunkPosition.z] = brush;
            chunk.isDirty = true;

            List<Chunk> surroundingChunks = GetChunksSurroundingPosition(position);
            foreach (Chunk surroundingChunk in surroundingChunks)
            {
                surroundingChunk.isDirty = true;
            }
        }
    }

    public void Erase(Vector3 position)
    {
        Draw(position, new ChunkCell(0, 0));
    }

    public void Refresh()
    {
        for (int z = 0; z < STAGE_DEPTH; z++)
        {
            for (int y = 0; y < STAGE_HEIGHT; y++)
            {
                for (int x = 0; x < STAGE_WIDTH; x++)
                {
                    Chunk chunk = chunks[x, y, z];

                    if (chunk.isDirty)
                    {
                        chunk.GenerateMesh();
                        chunk.GenerateColliders();

                        chunk.isDirty = false;
                    }
                }
            }
        }
    }

    public List<Chunk> GetChunksSurroundingPosition(Vector3 position)
    {
        List<Chunk> surroundingChunks = new List<Chunk>();
        Vector3Int chunkPosition = WorldPositionToChunkPosition(position);

        if (chunkPosition.x == 0)
        {
            Chunk surroundingChunk = GetChunkAtPosition(position + Vector3.left);
            
            if (surroundingChunk != null)
            {
                surroundingChunks.Add(surroundingChunk);
            }
        }
        
        if (chunkPosition.x == Chunk.CHUNK_WIDTH - 1)
        {
            Chunk surroundingChunk = GetChunkAtPosition(position + Vector3.right);

            if (surroundingChunk != null)
            {
                surroundingChunks.Add(surroundingChunk);
            }
        }

        if (chunkPosition.y == 0)
        {
            Chunk surroundingChunk = GetChunkAtPosition(position + Vector3.down);

            if (surroundingChunk != null)
            {
                surroundingChunks.Add(surroundingChunk);
            }
        }

        if (chunkPosition.y == Chunk.CHUNK_HEIGHT - 1)
        {
            Chunk surroundingChunk = GetChunkAtPosition(position + Vector3.up);

            if (surroundingChunk != null)
            {
                surroundingChunks.Add(surroundingChunk);
            }
        }

        if (chunkPosition.z == 0)
        {
            Chunk surroundingChunk = GetChunkAtPosition(position + Vector3.back);

            if (surroundingChunk != null)
            {
                surroundingChunks.Add(surroundingChunk);
            }
        }

        if (chunkPosition.z == Chunk.CHUNK_DEPTH - 1)
        {
            Chunk surroundingChunk = GetChunkAtPosition(position + Vector3.forward);

            if (surroundingChunk != null)
            {
                surroundingChunks.Add(surroundingChunk);
            }
        }

        return surroundingChunks;
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chunk : MonoBehaviour
{
    public const int CHUNK_WIDTH = 5;
    public const int CHUNK_HEIGHT = 25;
    public const int CHUNK_DEPTH = 5;

    public ChunkCell[,,] chunkData;

    public GameObject testMeshSource;
    public GameObject model;
    private MeshFilter meshFilter;

    public StageMeshCreator stageMeshCreator;

    public StageChunks chunkManager;

    void Start()
    {
        chunkData = new ChunkCell[CHUNK_WIDTH, CHUNK_HEIGHT, CHUNK_DEPTH];

        meshFilter = GetComponentInChildren<MeshFilter>();

        model.transform.position = model.transform.position - new Vector3(CHUNK_WIDTH / 2f, CHUNK_HEIGHT / 2f, CHUNK_DEPTH / 2f) + new Vector3(0.5f, 0.5f, 0.5f);
    }

    public void GenerateMesh()
    {
        List<CombineInstance> combineList = new List<CombineInstance>();

        for (int z = 0; z < CHUNK_DEPTH; z++)
        {
            for (int y = 0; y < CHUNK_HEIGHT; y++)
            {
                for (int x = 0; x < CHUNK_WIDTH; x++)
                {
                    if (chunkData[x, y, z].tilesetIndex != 0)
                    {
                        combineList.AddRange(stageMeshCreator.GetCellMesh(this, new Vector3Int(x, y, z)));
                    }
                }
            }
        }

        meshFilter.mesh.Clear();
        meshFilter.mesh.CombineMeshes(combineList.ToArray(), true);
    }

    public ChunkCell GetChunkCell(Vector3Int position)
    {
        if (position.x < 0 || position.x >= CHUNK_WIDTH || position.y < 0 || position.y >= CHUNK_HEIGHT || position.z < 0 || position.z >= CHUNK_DEPTH)
        {
            Vector3 worldPosition = chunkManager.ChunkPositionToWorldPosition(position, transform.position);
            return chunkManager.GetChunkCell(worldPosition);
        }
        else
        {
            return chunkData[position.x, position.y, position.z];
        }
    }

    public void GenerateColliders()
    {
        Collider[] existingChildColliders = GetComponentsInChildren<Collider>();
        
        for (int i = 0; i < existingChildColliders.Length; i++)
        {
            GameObject.Destroy(existingChildColliders[i].gameObject);
        }

        bool[,,] colliderMask = new bool[CHUNK_WIDTH, CHUNK_HEIGHT, CHUNK_DEPTH];
        int j = 0;

        for (int z = 0; z < CHUNK_DEPTH; z++)
        {
            for (int y = 0; y < CHUNK_HEIGHT; y++)
            {
                for (int x = 0; x < CHUNK_WIDTH; x++)
                {
                    if (chunkData[x, y, z].tilesetIndex != 0 && !colliderMask[x, y, z])
                    {
                        GameObject colliderObject = new GameObject("Collider_" + j);
                        colliderObject.layer = LayerMask.NameToLayer("Solid");
                        colliderObject.transform.parent = transform;
                        colliderObject.transform.localPosition = Vector3.zero;
                        BoxCollider boxCollider = colliderObject.AddComponent<BoxCollider>();

                        int gx = x + 1;
                        int gy = y + 1;
                        int gz = z + 1;

                        while (gx < CHUNK_WIDTH && chunkData[gx, y, z].tilesetIndex != 0 && !colliderMask[gx, y, z])
                        {
                            gx++;
                        }

                        while (gy < CHUNK_HEIGHT && chunkData[gx - 1, gy, z].tilesetIndex != 0 && !colliderMask[gx - 1, gy, z])
                        {
                            if (RangeContainsHoles(x, y, z, gx, gy + 1, gz))
                            {
                                break;
                            }

                            gy++;
                        }

                        while (gz < CHUNK_DEPTH && chunkData[gx - 1, gy - 1, gz].tilesetIndex != 0 && !colliderMask[gx - 1, gy - 1, gz])
                        {
                            if (RangeContainsHoles(x, y, z, gx, gy, gz + 1))
                            {
                                break;
                            }

                            gz++;
                        }

                        boxCollider.center = colliderObject.transform.localPosition 
                            - new Vector3(CHUNK_WIDTH / 2f, CHUNK_HEIGHT / 2f, CHUNK_DEPTH / 2f) 
                            + new Vector3((float)(gx - x) / 2f, (float)(gy - y) / 2f, (float)(gz - z) / 2f) 
                            + new Vector3(x, y, z);
                        boxCollider.size = new Vector3(gx - x, gy - y, gz - z);

                        j++;

                        for (int mz = z; mz < gz; mz++)
                        {
                            for (int my = y; my < gy; my++)
                            {
                                for (int mx = x; mx < gx; mx++)
                                {
                                    colliderMask[mx, my, mz] = true;
                                }
                            }
                        }
                    }
                    else
                    {
                        colliderMask[x, y, z] = true;
                    }
                }
            }
        }
    }

    private bool RangeContainsHoles(int x, int y, int z, int gx, int gy, int gz)
    {
        for (int k = z; k < gz; k++)
        {
            for (int j = y; j < gy; j++)
            {
                for (int i = x; i < gx; i++)
                {
                    if (chunkData[i, j, k].tilesetIndex == 0)
                    {
                        return true;
                    }
                }
            }
        }

        return false;
    }
}

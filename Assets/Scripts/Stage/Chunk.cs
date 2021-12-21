using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chunk : MonoBehaviour
{
    public const int CHUNK_WIDTH = 5;
    public const int CHUNK_HEIGHT = 20;
    public const int CHUNK_DEPTH = 5;

    public ChunkCell[,,] chunkData;

    public GameObject testMeshSource;
    public GameObject model;
    private Mesh testMesh;
    private MeshFilter meshFilter;

    void Start()
    {
        chunkData = new ChunkCell[CHUNK_WIDTH, CHUNK_HEIGHT, CHUNK_DEPTH];

        testMesh = testMeshSource.GetComponent<MeshFilter>().sharedMesh;
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
                        CombineInstance ci = new CombineInstance();
                        ci.mesh = testMesh;
                        ci.transform = Matrix4x4.Translate(new Vector3(x, y, z));

                        combineList.Add(ci);
                    }
                }
            }
        }

        meshFilter.mesh.Clear();
        meshFilter.mesh.CombineMeshes(combineList.ToArray(), true);
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
                            // bool colliderContainsHole = false;

                            // for (int gx2 = x; gx2 < gx; gx2++)
                            // {
                            //     for (int gy2 = y; gy2 < gy; gy2++)
                            //     {
                            //         Debug.Log(gx2 + "," + gy2);
                            //         if (chunkData[gx2, gy2, z].tilesetIndex == 0 && colliderMask[gx2, gy2, z])
                            //         {
                            //             colliderContainsHole = true;
                            //         }
                            //     }
                            // }

                            // if (colliderContainsHole)
                            // {
                            //     break;
                            // }

                            gy++;
                        }

                        while (gz < CHUNK_DEPTH && chunkData[gx - 1, gy - 1, gz].tilesetIndex != 0 && !colliderMask[gx - 1, gy - 1, gz])
                        {
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
}

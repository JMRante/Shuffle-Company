using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct LiquidStruct
{
    public LiquidStruct(byte type, float amount, byte fall)
    {
        Type = type;
        Amount = amount;
        Fall = fall;
    }

    public byte Type { get; set; }
    public float Amount { get; set; }
    public byte Fall { get; set; }
}

public class LiquidController : MonoBehaviour
{
    public const int WORLD_SIM_WIDTH = 200;
    public const int WORLD_SIM_HEIGHT = 100;
    public const int WORLD_SIM_DEPTH = 200;
    private LiquidStruct[,,] liquidSim = new LiquidStruct[WORLD_SIM_WIDTH, WORLD_SIM_HEIGHT, WORLD_SIM_DEPTH];

    private Sensor sensor;

    private Vector3 offsetVector = new Vector3(WORLD_SIM_DEPTH / 2, WORLD_SIM_HEIGHT / 2, WORLD_SIM_DEPTH / 2);

    public LiquidStruct[,,] LiquidSim
    {
        get => liquidSim;
    }

    private Vector3Int[] directions = new Vector3Int[] { Vector3Int.forward, Vector3Int.right, Vector3Int.back, Vector3Int.left, Vector3Int.up };

    void Start()
    {
        sensor = GetComponentInChildren<Sensor>();

        for (int x = 0; x < WORLD_SIM_WIDTH; x++)
        {
            for (int y = 0; y < WORLD_SIM_HEIGHT; y++)
            {
                for (int z = 0; z < WORLD_SIM_DEPTH; z++)
                {
                    if (sensor.IsCellPositionBlocked(TransformSimToWorldCoords(new Vector3(x, y, z)), Vector3.one * 0.49f, sensor.SolidLayerMask))
                    {
                        liquidSim[x, y, z].Type = 1;
                        liquidSim[x, y, z].Amount = 1f;
                    }
                    else
                    {
                        liquidSim[x, y, z].Type = 0;
                        liquidSim[x, y, z].Amount = 0f;
                    }
                }
            }
        }
    }

    void Update()
    {
        for (int x = 0; x < WORLD_SIM_WIDTH; x++)
        {
            for (int y = 0; y < WORLD_SIM_HEIGHT; y++)
            {
                for (int z = 0; z < WORLD_SIM_DEPTH; z++)
                {
                    LiquidStruct liquidCell = liquidSim[x, y, z];

                    
                }
            }
        }
    }

    public void AddLiquid(byte type, float amount, Vector3 position)
    {
        Vector3 simPosition = TransformWorldToSimCoords(position);
        liquidSim[(int)simPosition.x, (int)simPosition.y, (int)simPosition.z].Type = type;
        liquidSim[(int)simPosition.x, (int)simPosition.y, (int)simPosition.z].Amount = amount;
    }

    public Vector3 TransformWorldToSimCoords(Vector3 position)
    {
        return position + offsetVector;
    }

    public Vector3 TransformSimToWorldCoords(Vector3 position)
    {
        return position - offsetVector;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiquidRenderer : MonoBehaviour
{
    public GameObject liquidModel;
    public GameObject fallModel;

    private LiquidController liquidController;

    void Start()
    {
        liquidController = GetComponentInParent<LiquidController>();
    }

    void Update()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        for (int x = 0; x < LiquidController.WORLD_SIM_WIDTH; x++)
        {
            for (int y = 0; y < LiquidController.WORLD_SIM_HEIGHT; y++)
            {
                for (int z = 0; z < LiquidController.WORLD_SIM_DEPTH; z++)
                {
                    if (liquidController.LiquidSim[x, y, z].Type == 2 && liquidController.LiquidSim[x, y, z].Amount >= 0.2f)
                    {
                        Instantiate(liquidModel, liquidController.TransformSimToWorldCoords(new Vector3(x, y, z)), Quaternion.identity, transform);
                    }
                }
            }
        }
    }
}

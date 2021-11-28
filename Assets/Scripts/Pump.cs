using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pump : MonoBehaviour
{
    public GameObject liquidCell;
    public int pumpMaxHeight;

    private Sensor sensor;

    private int totalMask;

    void Start()
    {
        sensor = GetComponentInChildren<Sensor>();
        totalMask = LayerMask.GetMask("Solid", "Water");
    }

    void Update()
    {
        if (!sensor.IsCellBlocked(Vector3.up, Vector3.one * 0.49f, totalMask) && pumpMaxHeight > 0)
        {
            GameObject liquidSeedObject = Instantiate(liquidCell, transform.position + Vector3.up, Quaternion.identity, transform.parent);
            liquidSeedObject.name = "WC";
            Liquid liquidSeed = liquidSeedObject.GetComponent<Liquid>();
            liquidSeed.ParentPump = this;
        }
    }

    // public LiquidController liquidController;
    // public int pumpMaxHeight;

    // private Sensor sensor;

    // private int totalMask;

    // void Start()
    // {
    //     sensor = GetComponentInChildren<Sensor>();
    //     totalMask = LayerMask.GetMask("Solid", "Water");
    // }

    // void Update()
    // {
    //     if (!sensor.IsCellBlocked(Vector3.up, Vector3.one * 0.49f, totalMask))
    //     {
    //         liquidController.AddLiquid(2, 1f, transform.position + Vector3.up);
    //     }
    // }
}

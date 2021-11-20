using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WaterDestroys : MonoBehaviour
{
    private Destructable destructable;

    void Start()
    {
        destructable = GetComponent<Destructable>();
    }

    void Update()
    {
        Sensor[] sensors = Utility.GetComponentsInDirectChildren(gameObject, typeof(Sensor)).Cast<Sensor>().ToArray();

        foreach (Sensor sensor in sensors)
        {
            if (sensor.IsCellBlockedByLiquid(Vector3.up * 0.7f))
            {
                destructable.Destruct();
            }
        }
    }
}

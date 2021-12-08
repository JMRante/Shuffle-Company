using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collector : MonoBehaviour
{
    private Dictionary<string, int> itemCollection;
    private Sensor sensor;

    void Start()
    {
        itemCollection = new Dictionary<string, int>();
        sensor = GetComponentInChildren<Sensor>();
    }

    void Update()
    {
        Collectable collectable = (Collectable) sensor.GetComponentFromCell(Vector3.zero, Vector3.one * 0.3f, typeof(Collectable), sensor.SolidLayerMask, true);

        if (collectable != null)
        {
            if (!itemCollection.ContainsKey(collectable.collectableName))
            {
                itemCollection.Add(collectable.collectableName, collectable.collectableValue);
            }
            else
            {
                itemCollection[collectable.collectableName] += collectable.collectableValue;
            }

            collectable.CollectDestroy();
        }
    }

    public int GetCollectableCount(string collectableName)
    {
        if (!itemCollection.ContainsKey(collectableName))
        {
            return 0;
        }
        else
        {
            return itemCollection[collectableName];
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemGate : MonoBehaviour
{
    public string item;
    public string itemTag;

    private Collector playerCollector;
    private int itemTotal;

    void Start()
    {
        playerCollector = GameObject.Find("Player").GetComponent<Collector>();
        itemTotal = GameObject.FindGameObjectsWithTag(itemTag).Length;
    }

    void Update()
    {
        if (playerCollector.GetCollectableCount(item) >= itemTotal)
        {
            GameObject.Destroy(gameObject);
        }
    }
}

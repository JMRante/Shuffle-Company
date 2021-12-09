using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GoldCubeUI : MonoBehaviour
{
    public Text goldCubeCountText;
    
    private Collector playerCollector;
    private int goldCubeTotal;

    void Start()
    {
        goldCubeTotal = GameObject.FindGameObjectsWithTag("GoldCube").Length;
        playerCollector = GameObject.Find("Player").GetComponent<Collector>();

        UpdateGoldCubeCountText();
    }

    void Update()
    {
        UpdateGoldCubeCountText();
    }

    private void UpdateGoldCubeCountText()
    {
        goldCubeCountText.text = playerCollector.GetCollectableCount("GoldCube") + "/" + goldCubeTotal;
    }
}

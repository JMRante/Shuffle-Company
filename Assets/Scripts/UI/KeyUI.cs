using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeyUI : MonoBehaviour
{
    public Text keyCountText;
    public string keyType;

    private Collector playerCollector;

    void Start()
    {
        playerCollector = GameObject.Find("Player").GetComponent<Collector>();
        
        UpdateKeyCountText();
    }

    void Update()
    {
        UpdateKeyCountText();
    }

    private void UpdateKeyCountText()
    {
        keyCountText.text = playerCollector.GetCollectableCount(keyType).ToString();
    }
}

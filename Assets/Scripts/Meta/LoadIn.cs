using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadIn : MonoBehaviour
{
    public GameObject blackScreen;

    void Start()
    {
        blackScreen.SetActive(true);
    }

    void Update()
    {
        if (Time.timeSinceLevelLoad > 0.2)
        {
            blackScreen.SetActive(false);
        }
    }
}

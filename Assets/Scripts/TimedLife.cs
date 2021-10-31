using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimedLife : MonoBehaviour
{
    public float lifeTime = 5f;

    void Start()
    {
        GameObject.Destroy(gameObject, lifeTime);
    }
}

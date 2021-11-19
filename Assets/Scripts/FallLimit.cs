using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallLimit : MonoBehaviour
{
    public float limit = -100f;
    
    private Destructable destructable;

    void Start()
    {
        destructable = GetComponent<Destructable>();
    }

    void Update()
    {
        if (transform.position.y < limit)
        {
            destructable.Destruct();
        }
    }
}

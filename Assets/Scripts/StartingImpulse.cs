using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartingImpulse : MonoBehaviour
{
    void Start()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.AddForce(new Vector3(Random.Range(-1f, 1f), 0f, Random.Range(-1f, 1f)), ForceMode.Impulse);
    }
}

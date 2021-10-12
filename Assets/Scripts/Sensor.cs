using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sensor : MonoBehaviour
{
    private int solidLayerMask;

    void Start()
    {
        solidLayerMask = LayerMask.GetMask("Solid");
    }

    public bool IsBlocked(Vector3 direction)
    {
        Collider[] colliders = Physics.OverlapBox(transform.position + (direction * 0.51f), Vector3.one * 0.45f, Quaternion.identity, solidLayerMask);

        foreach (Collider collider in colliders)
        {
            if (collider.transform.parent.gameObject != transform.parent.gameObject)
                return true;
        }

        return false;
    }
}

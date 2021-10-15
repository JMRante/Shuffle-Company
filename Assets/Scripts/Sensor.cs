using System;
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

    public bool IsCellBlocked(Vector3 direction)
    {
        Collider[] colliders = Physics.OverlapBox(transform.position + direction, Vector3.one * 0.49f, Quaternion.identity, solidLayerMask);

        foreach (Collider collider in colliders)
        {
            if (collider.transform.parent.gameObject != transform.parent.gameObject)
                return true;
        }

        return false;
    }

    public bool IsRayBlocked(Vector3 direction)
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position, direction, out hit, 0.98f, solidLayerMask))
        {
            if (hit.collider.transform.parent.gameObject != transform.parent.gameObject)
                return true;
        }

        return false;
    }

    public Component GetComponentFromCell(Vector3 direction, Type type)
    {
        Collider[] colliders = Physics.OverlapBox(transform.position + direction, Vector3.one * 0.49f, Quaternion.identity, solidLayerMask);

        foreach (Collider collider in colliders)
        {
            return collider.GetComponentInParent(type);
        }

        return null;
    }

    public bool DoesCellContainElementProperty(Vector3 direction, ElementProperty elementProperty)
    {
        Collider[] colliders = Physics.OverlapBox(transform.position + direction, Vector3.one * 0.49f, Quaternion.identity, solidLayerMask);

        foreach (Collider collider in colliders)
        {
            Element element = collider.GetComponentInParent<Element>();

            if (element != null && element.HasProperty(elementProperty))
            {
                return true;
            }
        }

        return false;
    }
}

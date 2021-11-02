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

    public bool IsCellPositionBlocked(Vector3 position)
    {
        Collider[] colliders = Physics.OverlapBox(position, Vector3.one * 0.49f, Quaternion.identity, solidLayerMask);

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

    public Component GetComponentFromCellPosition(Vector3 position, Type type)
    {
        Collider[] colliders = Physics.OverlapBox(position, Vector3.one * 0.49f, Quaternion.identity, solidLayerMask);

        foreach (Collider collider in colliders)
        {
            return collider.GetComponentInParent(type);
        }

        return null;
    }

    public Component GetComponentFromRay(Vector3 direction, Type type)
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position, direction, out hit, 0.98f, solidLayerMask))
        {
            return hit.collider.GetComponentInParent(type);
        }

        return null;
    }

    public bool DoesCellContainElementProperty(Vector3 direction, ElementProperty elementProperty)
    {
        Collider[] colliders = Physics.OverlapBox(transform.position + direction, Vector3.one * 0.49f, Quaternion.identity, solidLayerMask);

        foreach (Collider collider in colliders)
        {
            if (collider.transform.parent.gameObject != transform.parent.gameObject)
            {
                Element element = collider.GetComponentInParent<Element>();

                if (element != null && element.HasProperty(elementProperty))
                {
                    return true;
                }
            }
        }

        return false;
    }

    public bool DoesCellPositionContainElementProperty(Vector3 position, ElementProperty elementProperty)
    {
        Collider[] colliders = Physics.OverlapBox(position, Vector3.one * 0.49f, Quaternion.identity, solidLayerMask);

        foreach (Collider collider in colliders)
        {
            if (collider.transform.parent.gameObject != transform.parent.gameObject)
            {
                Element element = collider.GetComponentInParent<Element>();

                if (element != null && element.HasProperty(elementProperty))
                {
                    return true;
                }
            }
        }

        return false;
    }


    public bool DoesRayContainElementProperty(Vector3 direction, ElementProperty elementProperty)
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position, direction, out hit, 0.98f, solidLayerMask))
        {
            if (hit.collider.transform.parent.gameObject != transform.parent.gameObject)
            {
                Element element = hit.collider.GetComponentInParent<Element>();

                if (element != null && element.HasProperty(elementProperty))
                {
                    return true;
                }
            }
        }

        return false;
    }
}

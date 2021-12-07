using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sensor : MonoBehaviour
{
    private int solidLayerMask;
    private int waterLayerMask;
    private Dictionary<ElementProperty, ElementProperty> collisionExceptionList;

    public int SolidLayerMask
    {
        get => solidLayerMask;
    }

    public int WaterLayerMask
    {
        get => waterLayerMask;
    }

    void Start()
    {
        solidLayerMask = LayerMask.GetMask("Solid");
        waterLayerMask = LayerMask.GetMask("Water");

        collisionExceptionList = new Dictionary<ElementProperty, ElementProperty>();
        collisionExceptionList.Add(ElementProperty.Liquid, ElementProperty.Fillable);
        collisionExceptionList.Add(ElementProperty.Collector, ElementProperty.Collectable);
    }

    public bool IsCellBlocked(Vector3 direction)
    {
        return IsCellBlocked(direction, Vector3.one * 0.49f, solidLayerMask);
    }

    public bool IsCellBlockedByLiquid(Vector3 direction)
    {
        return IsCellBlocked(direction, Vector3.one * 0.49f, waterLayerMask);
    }

    public bool IsCellBlocked(Vector3 direction, Vector3 halfExtents, int layerMask)
    {
        Collider[] colliders = Physics.OverlapBox(transform.position + direction, halfExtents, Quaternion.identity, layerMask);

        foreach (Collider collider in colliders)
        {
            if (collider.transform.parent.gameObject != transform.parent.gameObject)
            {
                if (HasElementCollisionException(collider))
                {
                    return false;
                }

                return true;
            }
        }

        return false;
    }

    public bool IsCellPositionBlocked(Vector3 position)
    {
        Collider[] colliders = Physics.OverlapBox(position, Vector3.one * 0.49f, Quaternion.identity, solidLayerMask);

        foreach (Collider collider in colliders)
        {
            if (collider.transform.parent.gameObject != transform.parent.gameObject)
            {
                if (HasElementCollisionException(collider))
                {
                    return false;
                }

                return true;
            }
        }

        return false;
    }

    public bool IsCellPositionBlocked(Vector3 position, Vector3 halfExtents, int layerMask)
    {
        Collider[] colliders = Physics.OverlapBox(position, halfExtents, Quaternion.identity, layerMask);

        foreach (Collider collider in colliders)
        {
            if (collider.transform.parent.gameObject != transform.parent.gameObject)
            {
                if (HasElementCollisionException(collider))
                {
                    return false;
                }

                return true;
            }
        }

        return false;
    }

    public bool IsRayBlocked(Vector3 direction)
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position, direction, out hit, 0.98f, solidLayerMask))
        {
            if (hit.collider.transform.parent.gameObject != transform.parent.gameObject)
            {
                if (HasElementCollisionException(hit.collider))
                {
                    return false;
                }

                return true;
            }
        }

        return false;
    }

    public Component GetComponentFromCell(Vector3 direction, Type type)
    {
        Collider[] colliders = Physics.OverlapBox(transform.position + direction, Vector3.one * 0.49f, Quaternion.identity, solidLayerMask);

        foreach (Collider collider in colliders)
        {
            if (collider.transform.parent.gameObject != transform.parent.gameObject)
            {
                if (HasElementCollisionException(collider))
                {
                    return null;
                }

                return collider.GetComponentInParent(type);
            }
        }

        return null;
    }

    public Component GetComponentFromCell(Vector3 direction, Type type, int layerMask)
    {
        Collider[] colliders = Physics.OverlapBox(transform.position + direction, Vector3.one * 0.49f, Quaternion.identity, layerMask);

        foreach (Collider collider in colliders)
        {
            if (collider.transform.parent.gameObject != transform.parent.gameObject)
            {
                if (HasElementCollisionException(collider))
                {
                    return null;
                }

                return collider.GetComponentInParent(type);
            }
        }

        return null;
    }

    public Component GetComponentFromCellPosition(Vector3 position, Type type)
    {
        Collider[] colliders = Physics.OverlapBox(position, Vector3.one * 0.49f, Quaternion.identity, solidLayerMask);

        foreach (Collider collider in colliders)
        {
            if (collider.transform.parent.gameObject != transform.parent.gameObject)
            {
                if (HasElementCollisionException(collider))
                {
                    return null;
                }

                return collider.GetComponentInParent(type);
            }
        }

        return null;
    }

    public Component GetComponentFromRay(Vector3 direction, Type type)
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position, direction, out hit, 0.98f, solidLayerMask))
        {
            if (hit.collider.transform.parent.gameObject != transform.parent.gameObject)
            {
                if (HasElementCollisionException(hit.collider))
                {
                    return null;
                }

                return hit.collider.GetComponentInParent(type);
            }
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
                if (HasElementCollisionException(collider))
                {
                    return false;
                }

                Element element = collider.GetComponentInParent<Element>();

                if (element != null && element.HasProperty(elementProperty))
                {
                    return true;
                }
            }
        }

        return false;
    }

    public bool DoesCellContainElementProperty(Vector3 direction, ElementProperty elementProperty, Vector3 halfExtents, int layerMask)
    {
        Collider[] colliders = Physics.OverlapBox(transform.position + direction, halfExtents, Quaternion.identity, layerMask);

        foreach (Collider collider in colliders)
        {
            if (collider.transform.parent.gameObject != transform.parent.gameObject)
            {
                if (HasElementCollisionException(collider))
                {
                    return false;
                }

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
                if (HasElementCollisionException(collider))
                {
                    return false;
                }

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
                if (HasElementCollisionException(hit.collider))
                {
                    return false;
                }

                Element element = hit.collider.GetComponentInParent<Element>();

                if (element != null && element.HasProperty(elementProperty))
                {
                    return true;
                }
            }
        }

        return false;
    }

    public bool HasElementCollisionException(Collider otherCollider)
    {
        Element thisElement = transform.gameObject.GetComponentInParent<Element>();
        Element otherElement = otherCollider.transform.gameObject.GetComponentInParent<Element>();

        if (thisElement != null && otherElement != null)
        {
            foreach (ElementProperty property in thisElement.elementProperties)
            {
                if (collisionExceptionList.ContainsKey(property) && otherElement.HasProperty(collisionExceptionList[property]))
                {
                    return true;
                }
            }
        }

        return false;
    }
}

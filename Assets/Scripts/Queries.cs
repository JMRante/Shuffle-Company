using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Queries
{
    public static GameObject FirstElementAtIndexWithProperty(Vector3 index, ElementProperty elementProperty)
    {
        Collider[] hitColliders = Physics.OverlapSphere(index, 0.25f);

        foreach (Collider hitCollider in hitColliders)
        {
            Element element = hitCollider.GetComponentInParent<Element>();

            if (element != null && element.HasProperty(elementProperty))
            {
                return element.gameObject;
            }
        }

        return null;
    }

    public static List<GameObject> AllElementsAtIndexWithProperty(Vector3 index, ElementProperty elementProperty)
    {
        List<GameObject> elementList = new List<GameObject>();
        Collider[] hitColliders = Physics.OverlapSphere(index, 0.25f);

        foreach (Collider hitCollider in hitColliders)
        {
            Element element = hitCollider.GetComponentInParent<Element>();

            if (element != null && element.HasProperty(elementProperty))
            {
                elementList.Add(element.gameObject);
            }
        }

        return elementList;
    }

    public static bool ElementExistsAtIndexWithProperty(Vector3 index, ElementProperty elementProperty)
    {
        Collider[] hitColliders = Physics.OverlapSphere(index, 0.25f);

        foreach (Collider hitCollider in hitColliders)
        {
            Element element = hitCollider.GetComponentInParent<Element>();

            if (element != null && element.HasProperty(elementProperty))
            {
                return true;
            }
        }

        return false;
    }

    public static bool ElementHasProperty(GameObject gameObject, ElementProperty elementProperty)
    {
        Element element = gameObject.GetComponentInParent<Element>();

        if (element != null && element.HasProperty(elementProperty))
        {
            return true;
        }

        return false;
    }

    // public static int GetVariableAtIndex(Vector3 index, string key)
    // {

    // }

    // public static int GetVariableAtIndexWithProperty(Vector3 index, string key, ElementProperty elementProperty)
    // {

    // }

    // public static void SetVariableAtIndex(Vector3 index, string key, int value)
    // {

    // }

    // public static void SetVariableAtIndexWithProperty(Vector3 index, string key, int value, ElementProperty elementProperty)
    // {

    // }
}

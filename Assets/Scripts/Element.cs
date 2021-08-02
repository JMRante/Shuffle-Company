using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ElementProperty
{
    Solid,
    Pushable,
    Pusher,
    Slot,
    Loose
}

public class Element : MonoBehaviour
{
    public string elementName;
    public int sortOrder;
    public ElementProperty[] elementProperties;

    public bool HasProperty(ElementProperty elementProperty)
    {
        foreach (ElementProperty ep in elementProperties)
        {
            if (ep == elementProperty)
            {
                return true;
            }
        }

        return false;
    }
}

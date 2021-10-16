using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ElementProperty
{
    Solid,
    Pushable,
    Pusher,
    Slotable,
    Loose,
    Key,
    KeyBlock,
    All,
    Blocker,
    Movable,
    Climbable
}

public class Element : MonoBehaviour
{
    public string elementId;
    public string elementName;
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

    public bool HasAnyProperty(ElementProperty[] anyElementProperties)
    {
        foreach (ElementProperty ep1 in elementProperties)
        {
            foreach (ElementProperty ep2 in anyElementProperties)
            {
                if (ep1 == ep2)
                {
                    return true;
                }
            }
        }

        return false;
    }
}

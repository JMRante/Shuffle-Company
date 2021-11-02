using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utility
{
    public static bool IsSnapped(Vector3 vec)
    {
        return Mathf.Approximately(vec.x % 1f, 0f) && Mathf.Approximately(vec.y % 1f, 0f) && Mathf.Approximately(vec.z % 1f, 0f);
    }

    public static Vector3 Round(Vector3 vec)
    {
        return new Vector3(Mathf.Round(vec.x), Mathf.Round(vec.y), Mathf.Round(vec.z));
    }

    public static Component[] GetComponentsInDirectChildren(GameObject gameObject, Type type)
    {
        Component[] components = gameObject.GetComponentsInChildren(type);
        List<Component> directComponents = new List<Component>();

        foreach (Component comp in components)
        {
            if (comp.gameObject.transform.parent == gameObject.transform)
            {
                directComponents.Add(comp);
            }
        }

        return directComponents.ToArray();
    }
}

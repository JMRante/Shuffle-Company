using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageMeshDefinition
{
    public Dictionary<string, Mesh> stageMeshes;
    public string name;

    public StageMeshDefinition()
    {
        stageMeshes = new Dictionary<string, Mesh>();
        name = "";
    }

    public Mesh GetStageMeshPart(string partName)
    {
        string key = name + partName;

        if (stageMeshes.ContainsKey(key))
        {
            return stageMeshes[key];
        }
        else
        {
            return null;
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StageThemeDefinition
{
    public string name;
    public StageCellDefinition[] stageCellDefinitions;

    public static StageThemeDefinition createFromJSON(string jsonString)
    {
        return JsonUtility.FromJson<StageThemeDefinition>(jsonString);
    }
}

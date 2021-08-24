using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateVariables : MonoBehaviour
{
    private Dictionary<string, int> variables;

    void Awake()
    {
        variables = new Dictionary<string, int>();
    }

    public void SetInt(string key, int value)
    {
        variables[key] = value;
    }

    public int GetInt(string key)
    {
        return variables[key];
    }

    public void SetBoolean(string key, bool value)
    {
        variables[key] = value ? 1 : 0;
    }

    public bool GetBoolean(string key)
    {
        return variables[key] != 0 ? true : false;
    }
}

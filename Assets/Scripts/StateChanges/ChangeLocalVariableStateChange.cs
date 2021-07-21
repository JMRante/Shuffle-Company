using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeLocalVariableStateChange : StateChange
{
    private GameObject gameObject;
    private StateVariables stateVariables;
    private string key;
    private int oldValue;
    private int newValue;
    private float changeTime;

    public ChangeLocalVariableStateChange(GameObject gameObject, string key, int newValue, float changeTime)
    {
        this.gameObject = gameObject;
        this.stateVariables = gameObject.GetComponent<StateVariables>();
        this.key = key;
        this.oldValue = stateVariables.variables[key];
        this.newValue = newValue;
        this.changeTime = changeTime;
    }

    public override StateChangeType GetStateChangeCode()
    {
        return StateChangeType.ChangeLocalVariable;
    }

    public override void Do()
    {
        stateVariables.variables[key] = newValue;
    }

    public override void Undo()
    {
        stateVariables.variables[key] = oldValue;
    }

    public override void Render(float timer)
    {
        if (timer >= changeTime)
        {
            stateVariables.variables[key] = newValue;
        }
        else
        {
            stateVariables.variables[key] = oldValue;
        }
    }
}

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

    private StateChangeRecord stateChangeRecord;

    public ChangeLocalVariableStateChange(GameObject gameObject, string key, int newValue, float changeTime)
    {
        this.gameObject = gameObject;
        this.stateVariables = gameObject.GetComponent<StateVariables>();
        this.key = key;
        this.oldValue = stateVariables.GetInt(key);
        this.newValue = newValue;
        this.changeTime = changeTime;

        this.stateChangeRecord = new StateChangeRecord(GetStateChangeCode(), this.gameObject);
    }

    public override StateChangeType GetStateChangeCode()
    {
        return StateChangeType.ChangeLocalVariable;
    }

    public override StateChangeRecord GetStateChangeRecord()
    {
        return stateChangeRecord;
    }

    public override void Do()
    {
        stateVariables.SetInt(key, newValue);
    }

    public override void Undo()
    {
        stateVariables.SetInt(key, oldValue);
    }

    public override void Render(float timer)
    {
        if (timer >= changeTime)
        {
            stateVariables.SetInt(key, newValue);
        }
        else
        {
            stateVariables.SetInt(key, oldValue);
        }
    }
}

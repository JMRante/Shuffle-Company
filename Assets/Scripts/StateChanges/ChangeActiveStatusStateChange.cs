using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeActiveStatusStateChange : StateChange
{
    private GameObject gameObject;
    private bool newStatus;
    private float changeTime;

    private StateChangeRecord stateChangeRecord;

    public ChangeActiveStatusStateChange(GameObject gameObject, bool newStatus, float changeTime)
    {
        this.gameObject = gameObject;
        this.newStatus = newStatus;
        this.changeTime = changeTime;

        this.stateChangeRecord = new StateChangeRecord(GetStateChangeCode(), this.gameObject);
    }

    public override StateChangeType GetStateChangeCode()
    {
        return StateChangeType.ChangeActiveStatus;
    }

    public override StateChangeRecord GetStateChangeRecord()
    {
        return stateChangeRecord;
    }

    public override void Do()
    {
        gameObject.SetActive(newStatus);
    }

    public override void Undo()
    {
        gameObject.SetActive(!newStatus);
    }

    public override void Render(float timer)
    {
        if (timer >= changeTime)
        {
            gameObject.SetActive(newStatus);
        }
        else
        {
            gameObject.SetActive(!newStatus);
        }
    }
}

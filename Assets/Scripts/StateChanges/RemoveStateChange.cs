using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoveStateChange : StateChange
{
    private GameObject removedGameObject;
    private Vector3 removedGameObjectPosition;
    private Quaternion removedGameObjectRotation;
    private float changeTime;

    private StateChangeRecord stateChangeRecord;

    public RemoveStateChange(GameObject removedGameObject, float changeTime)
    {
        this.removedGameObject = removedGameObject;
        this.removedGameObjectPosition = removedGameObject.transform.position;
        this.removedGameObjectRotation = removedGameObject.transform.rotation;

        this.stateChangeRecord = new StateChangeRecord(GetStateChangeCode(), this.removedGameObject);
    }

    public override StateChangeType GetStateChangeCode()
    {
        return StateChangeType.Remove;
    }

    public override StateChangeRecord GetStateChangeRecord()
    {
        return stateChangeRecord;
    }

    public override void Do()
    {
        GameObject.Destroy(removedGameObject);
    }

    public override void Undo()
    {
        GameObject.Instantiate(removedGameObject, removedGameObjectPosition, removedGameObjectRotation);
    }

    public override void Render(float timer)
    {
        if (timer >= changeTime)
        {
            removedGameObject.SetActive(false);
        }
        else
        {
            removedGameObject.SetActive(true);
        }
    }
}

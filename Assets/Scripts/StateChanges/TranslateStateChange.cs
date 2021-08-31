using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TranslateStateChange : StateChange
{
    private GameObject gameObject;
    private Vector3 translation;
    private Vector3 startPosition;

    private GridCollisionSystem gcs;
    private StateChangeRecord stateChangeRecord;

    public TranslateStateChange(GameObject gameObject, Vector3 translation, GridCollisionSystem gcs) 
    {
        this.gameObject = gameObject;
        this.translation = translation;
        this.startPosition = this.gameObject.transform.position;
        this.gcs = gcs;

        this.stateChangeRecord = new StateChangeRecord(GetStateChangeCode(), this.gameObject);
    }

    public override StateChangeType GetStateChangeCode() 
    {
        return StateChangeType.Translate;
    }

    public override StateChangeRecord GetStateChangeRecord()
    {
        return stateChangeRecord;
    }

    public override void Do() 
    {
        // Debug.Log("Do " + gameObject + " from " + startPosition + " to " + (startPosition + translation));
        gameObject.transform.position = startPosition + translation;
        gcs.Hash(gameObject, startPosition, startPosition + translation);
    }

    public override void Undo() 
    {
        gameObject.transform.position = startPosition;
        gcs.Hash(gameObject, startPosition + translation, startPosition);
    }

    public override void Render(float timer)
    {
        gameObject.transform.position = Vector3.Lerp(startPosition, startPosition + translation, timer);
    }
}

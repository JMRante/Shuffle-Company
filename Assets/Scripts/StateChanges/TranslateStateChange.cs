using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TranslateStateChange : StateChange
{
    private GameObject gameObject;
    private Vector3 translation;
    private Vector3 startPosition;

    public TranslateStateChange(GameObject gameObject, Vector3 translation) 
    {
        this.gameObject = gameObject;
        this.translation = translation;
        this.startPosition = this.gameObject.transform.position;
    }

    public override StateChangeType GetStateChangeCode() 
    {
        return StateChangeType.Translate;
    }

    public override void Do() 
    {
        gameObject.transform.position = startPosition + translation;
    }

    public override void Undo() 
    {
        gameObject.transform.position = startPosition;
    }

    public override void Render(float timer)
    {
        gameObject.transform.position = Vector3.Lerp(startPosition, startPosition + translation, timer);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TranslateAction : IAction
{
    private GameObject gameObject;
    private Vector3 translation;
    private Vector3 startPosition;

    public TranslateAction(GameObject gameObject, Vector3 translation) 
    {
        this.gameObject = gameObject;
        this.translation = translation;
        this.startPosition = this.gameObject.transform.position;
    }

    public int GetActionCode() 
    {
        return 1;
    }

    public void Do() 
    {
        gameObject.transform.position = startPosition + translation;
    }

    public void Undo() 
    {
        gameObject.transform.position = startPosition;
    }

    public void Render(float timer)
    {
        gameObject.transform.position = Vector3.Lerp(startPosition, startPosition + translation, timer);
    }
}

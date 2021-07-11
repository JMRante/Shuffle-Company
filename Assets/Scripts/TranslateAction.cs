using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TranslateAction : IAction
{
    private GameObject gameObject;
    private Vector3 translation;

    public TranslateAction(GameObject gameObject, Vector3 translation) 
    {
        this.gameObject = gameObject;
        this.translation = translation;
    }

    public int GetActionCode() 
    {
        return 1;
    }

    public void Do() 
    {
        gameObject.transform.position += translation;
    }

    public void Undo() 
    {
        gameObject.transform.position -= translation;
    }
}

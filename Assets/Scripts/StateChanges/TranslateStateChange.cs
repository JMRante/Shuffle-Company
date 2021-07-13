using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TranslateStateChange : IStateChange
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

    public StateChange GetStateChangeCode() 
    {
        return StateChange.Translate;
    }

    public bool IsPossible()
    {
        Collider[] hitColliders = Physics.OverlapSphere(startPosition + translation, 0.5f);

        foreach (Collider hitCollider in hitColliders)
        {
            ElementProperties elementProperties = hitCollider.GetComponent<ElementProperties>();
            
            if (elementProperties != null && elementProperties.HasProperty(ElementProperty.Solid))
            {
                return false;
            }
        }

        return true;
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

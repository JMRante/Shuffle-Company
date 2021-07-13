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

    public bool IsPossible(List<IStateChange> resultingStateChanges)
    {
        Collider[] hitColliders = Physics.OverlapSphere(startPosition + translation, 0.5f);

        foreach (Collider hitCollider in hitColliders)
        {
            ElementProperties otherElementProperties = hitCollider.GetComponent<ElementProperties>();
            
            if (otherElementProperties != null && otherElementProperties.HasProperty(ElementProperty.Solid))
            {
                ElementProperties myElementProperties = gameObject.GetComponent<ElementProperties>();

                if (myElementProperties != null 
                    && otherElementProperties.HasProperty(ElementProperty.Pushable)
                    && myElementProperties.HasProperty(ElementProperty.Pusher))
                {
                    TranslateStateChange pushStateChange = new TranslateStateChange(hitCollider.gameObject, translation);
                    resultingStateChanges.Add(pushStateChange);
                }
                else
                {
                    return false;
                }
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

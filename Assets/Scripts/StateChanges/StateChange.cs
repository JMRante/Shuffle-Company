using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StateChangeType
{
    Translate,
    ChangeLocalVariable,
    ChangeGlobalVariable,
    Create,
    Remove,
    IncrementItemInInventory,
    DecrementItemInInventory,
    Teleport,
    ChangeLiquidHeight,
    BounceLaser
}

public abstract class StateChange
{
    public abstract StateChangeType GetStateChangeCode();
    public abstract bool IsPossible(List<StateChange> resultingStateChanges);
    public abstract void Do();
    public abstract void Undo();
    public abstract void Render(float timer);

    public GameObject FirstElementAtIndexWithProperty(Vector3 index, ElementProperty elementProperty)
    {
        Collider[] hitColliders = Physics.OverlapSphere(index, 0.5f);

        foreach (Collider hitCollider in hitColliders)
        {
            ElementProperties elementProperties = hitCollider.GetComponent<ElementProperties>();

            if (elementProperties != null && elementProperties.HasProperty(elementProperty))
            {
                return hitCollider.gameObject;
            }
        }

        return null;
    }

    public List<GameObject> AllElementsAtIndexWithProperty(Vector3 index, ElementProperty elementProperty)
    {
        List<GameObject> elementList = new List<GameObject>();
        Collider[] hitColliders = Physics.OverlapSphere(index, 0.5f);

        foreach (Collider hitCollider in hitColliders)
        {
            ElementProperties elementProperties = hitCollider.GetComponent<ElementProperties>();

            if (elementProperties != null && elementProperties.HasProperty(elementProperty))
            {
                elementList.Add(hitCollider.gameObject);
            }
        }

        return elementList;
    }

    public bool ElementExistsAtIndexWithProperty(Vector3 index, ElementProperty elementProperty)
    {
        Collider[] hitColliders = Physics.OverlapSphere(index, 0.5f);

        foreach (Collider hitCollider in hitColliders)
        {
            ElementProperties elementProperties = hitCollider.GetComponent<ElementProperties>();

            if (elementProperties != null && elementProperties.HasProperty(elementProperty))
            {
                return true;
            }
        }

        return false;
    }

    public bool ElementHasProperty(GameObject gameObject, ElementProperty elementProperty)
    {
        ElementProperties elementProperties = gameObject.GetComponent<ElementProperties>();

        if (elementProperties != null && elementProperties.HasProperty(elementProperty))
        {
            return true;
        }

        return false;
    }

    // public int GetVariableAtIndex(Vector3 index, string key)
    // {

    // }

    // public int GetVariableAtIndexWithProperty(Vector3 index, string key, ElementProperty elementProperty)
    // {

    // }

    // public void SetVariableAtIndex(Vector3 index, string key, int value)
    // {

    // }

    // public void SetVariableAtIndexWithProperty(Vector3 index, string key, int value, ElementProperty elementProperty)
    // {

    // }
}

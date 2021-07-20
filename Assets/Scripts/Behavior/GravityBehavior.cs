using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityBehavior : MonoBehaviour, IBehavior
{
    public int GetPriority()
    {
        return 0;
    }

    public bool IsTriggered()
    {
        bool isSolidBelow = Queries.ElementExistsAtIndexWithProperty(gameObject.transform.position + Vector3.down, ElementProperty.Solid);
        return !isSolidBelow;
    }

    public bool IsPossible()
    {
        return true;
    }

    public List<StateChange> GetStateChanges()
    {
        List<StateChange> stateChanges = new List<StateChange>();
        stateChanges.Add(new TranslateStateChange(gameObject, Vector3.down));

        Vector3 positionAbove = gameObject.transform.position + Vector3.up;
        GameObject looseObjectAbove = Queries.FirstElementAtIndexWithProperty(positionAbove, ElementProperty.Loose);

        if (looseObjectAbove != null && looseObjectAbove != gameObject)
        {
            GravityBehavior looseObjectAboveGravityBehavior = looseObjectAbove.GetComponent<GravityBehavior>();

            if (looseObjectAboveGravityBehavior != null)
            {
                stateChanges.AddRange(looseObjectAboveGravityBehavior.GetStateChanges());
            }
        }

        return stateChanges;
    }
}

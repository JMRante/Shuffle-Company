using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityBehavior : MonoBehaviour, IBehavior
{
    public List<StateChange> CheckForStateChanges()
    {
        bool isSolidBelow = Queries.ElementExistsAtIndexWithProperty(gameObject.transform.position + Vector3.down, ElementProperty.Solid);

        if (!isSolidBelow)
        {
            List<StateChange> stateChanges = new List<StateChange>();
            stateChanges.Add(new TranslateStateChange(gameObject, Vector3.down));
            return stateChanges;
        }

        return null;
    }
}

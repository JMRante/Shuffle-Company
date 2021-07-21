using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityBehavior : MonoBehaviour, IBehavior
{
    public int GetPriority()
    {
        return 0;
    }

    public List<StateChange> GetStateChanges()
    {
        bool isSolidBelow = Queries.ElementExistsAtIndexWithProperty(gameObject.transform.position + Vector3.down, ElementProperty.Solid);
        StateVariables stateVariables = gameObject.GetComponent<StateVariables>();

        if (!isSolidBelow)
        {
            List<StateChange> stateChanges = new List<StateChange>();
            stateChanges.Add(new TranslateStateChange(gameObject, Vector3.down));

            Vector3 positionAbove = gameObject.transform.position + Vector3.up;
            GameObject looseObjectAbove = Queries.FirstElementAtIndexWithProperty(positionAbove, ElementProperty.Loose);

            // Vector3 positionBelow2 = gameObject.transform.position + (Vector3.down * 2);
            // GameObject solidObjectBelow2 = Queries.FirstElementAtIndexWithProperty(positionBelow2, ElementProperty.Solid);
            
            // if (solidObjectBelow2.GetComponent<StateVariables>().variables["isFalling"] == 0)
            // stateChanges.Add(new ChangeLocalVariableStateChange(gameObject, "isFalling", 1, 1f));

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

        return null;
    }
}

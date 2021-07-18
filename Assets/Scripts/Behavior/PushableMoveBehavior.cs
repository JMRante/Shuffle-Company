using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushableMoveBehavior : IBehavior
{
    private GameObject pushable;
    private Vector3 direction;

    public PushableMoveBehavior(GameObject pushable, Vector3 direction)
    {
        this.pushable = pushable;
        this.direction = direction;
    }

    public int GetPriority()
    {
        return 1;
    }

    public bool IsTriggered()
    {
        return false;
    }

    public bool IsPossible()
    {
        Vector3 positionAhead = pushable.transform.position + direction;

        GameObject solidAhead = Queries.FirstElementAtIndexWithProperty(positionAhead, ElementProperty.Solid);

        if (solidAhead)
        {
            return false;
        }

        return true;
    }

    public List<StateChange> GetStateChanges()
    {
        List<StateChange> stateChanges = new List<StateChange>();
        stateChanges.Add(new TranslateStateChange(pushable, direction));

        Vector3 positionAhead = pushable.transform.position + direction;
        SlotBehavior slotBehavior = new SlotBehavior(pushable.transform.position, positionAhead);

        if (slotBehavior.IsTriggered())
        {
            stateChanges.AddRange(slotBehavior.GetStateChanges());
        }

        return stateChanges;
    }
}

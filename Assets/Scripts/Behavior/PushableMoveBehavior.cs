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

    public List<StateChange> GetStateChanges()
    {
        List<StateChange> stateChanges = new List<StateChange>();

        Vector3 positionAhead = pushable.transform.position + direction;
        GameObject solidAhead = Queries.FirstElementAtIndexWithProperty(positionAhead, ElementProperty.Solid);
        
        if (solidAhead)
        {
            return null;
        }

        SlotBehavior slotBehavior = new SlotBehavior(pushable.transform.position, positionAhead);
        List<StateChange> slotBehaviorStateChanges = slotBehavior.GetStateChanges();

        if (slotBehaviorStateChanges != null)
        {
            stateChanges.AddRange(slotBehaviorStateChanges);
        }

        Vector3 positionAbove = pushable.transform.position + Vector3.up;
        GameObject looseObjectAbove = Queries.FirstElementAtIndexWithProperty(positionAbove, ElementProperty.Loose);

        if (looseObjectAbove != null)
        {
            PushableMoveBehavior pushAboveBehavior = new PushableMoveBehavior(looseObjectAbove, direction);
            List<StateChange> pushAboveBehaviorStateChanges = pushAboveBehavior.GetStateChanges();

            if (pushAboveBehaviorStateChanges != null)
            {
                stateChanges.AddRange(pushAboveBehaviorStateChanges);
            }
        }

        stateChanges.Add(new TranslateStateChange(pushable, direction));

        return stateChanges;
    }
}

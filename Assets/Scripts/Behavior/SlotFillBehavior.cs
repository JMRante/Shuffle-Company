using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotFillBehavior : IBehavior
{
    private Vector3 position;

    public SlotFillBehavior(Vector3 position)
    {
        this.position = position;
    }

    public int GetPriority()
    {
        return 1;
    }

    public bool IsTriggered()
    {
        bool isInSlot = Queries.ElementExistsAtIndexWithProperty(position, ElementProperty.Slot);

        if (isInSlot)
        {
            return true;
        }

        return false;
    }

    public bool IsPossible()
    {
        return true;
    }

    public List<StateChange> GetStateChanges()
    {
        GameObject slot = Queries.FirstElementAtIndexWithProperty(position, ElementProperty.Slot);

        List<StateChange> stateChanges = new List<StateChange>();
        stateChanges.Add(new ChangeMaterialStateChange(slot, Resources.Load<Material>("Materials/SlotTestFull"), 0.8f));
        return stateChanges;
    }
}

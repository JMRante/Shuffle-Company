using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotBehavior : IBehavior
{
    private Vector3 slotablePosition;
    private Vector3 slotablePositionAhead;
    private bool isFill;

    public SlotBehavior(Vector3 slotablePosition, Vector3 slotablePositionAhead)
    {
        this.slotablePosition = slotablePosition;
        this.slotablePositionAhead = slotablePositionAhead;
    }

    public int GetPriority()
    {
        return 1;
    }

    public bool IsTriggered()
    {
        return true;
    }

    public bool IsPossible()
    {
        return true;
    }

    public List<StateChange> GetStateChanges()
    {
        GameObject enteringSlot = Queries.FirstElementAtIndexWithProperty(slotablePositionAhead, ElementProperty.Slot);
        GameObject exitingSlot = Queries.FirstElementAtIndexWithProperty(slotablePosition, ElementProperty.Slot);

        List<StateChange> stateChanges = new List<StateChange>();

        if (enteringSlot != null)
        {
            stateChanges.Add(new ChangeMaterialStateChange(enteringSlot, Resources.Load<Material>("Materials/SlotTestFull"), 0.8f));
        }

        if (exitingSlot != null)
        {
            stateChanges.Add(new ChangeMaterialStateChange(exitingSlot, Resources.Load<Material>("Materials/SlotTestEmpty"), 0.2f));
        }

        return stateChanges;
    }
}

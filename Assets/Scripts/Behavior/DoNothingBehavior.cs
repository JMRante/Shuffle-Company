using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoNothingBehavior : IBehavior
{
    public int GetPriority()
    {
        return 100;
    }

    public bool IsTriggered()
    {
        return false;
    }

    public bool IsPossible()
    {
        return true;
    }

    public List<StateChange> GetStateChanges()
    {
        return null;
    }
}

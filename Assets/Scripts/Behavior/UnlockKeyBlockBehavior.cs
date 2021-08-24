using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnlockKeyBlockBehavior : MonoBehaviour, IBehavior
{
    public bool IsPassive()
    {
        return true;
    }

    public int GetPriority()
    {
        return 0;
    }

    public List<StateChange> GetStateChanges()
    {
        List<StateChange> stateChanges = new List<StateChange>();
        stateChanges.Add(new ChangeActiveStatusStateChange(gameObject, false, 0.1f));
        return stateChanges;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move
{
    private List<StateChange> stateChanges;
    private HashSet<StateChangeRecord> stateChangeRecords;

    public Move() 
    {
        stateChanges = new List<StateChange>();
        stateChangeRecords = new HashSet<StateChangeRecord>();
    }

    public void AddStateChange(StateChange stateChangeToAdd)
    {
        stateChanges.Add(stateChangeToAdd);
        stateChangeRecords.Add(stateChangeToAdd.GetStateChangeRecord());
    }

    public void AddStateChanges(List<StateChange> stateChangesToAdd)
    {
        foreach (StateChange stateChange in stateChangesToAdd)
        {
            AddStateChange(stateChange);
        }
    }
    
    public void Do() 
    {
        foreach (StateChange stateChange in stateChanges) 
        {
            stateChange.Do();
        }
    }

    public void Undo() 
    {
        foreach (StateChange stateChange in stateChanges)
        {
            stateChange.Undo();
        }
    }

    public void Render(float timer)
    {
        foreach (StateChange stateChange in stateChanges)
        {
            stateChange.Render(timer);
        }
    }

    public bool IsInStateChangeRecords(StateChange stateChange)
    {
        return stateChangeRecords.Contains(stateChange.GetStateChangeRecord());
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move
{
    private List<StateChange> stateChanges;

    public Move() 
    {
        stateChanges = new List<StateChange>();
    }

    public void AddStateChange(StateChange stateChangeToAdd)
    {
        stateChanges.Add(stateChangeToAdd);
    }

    public void AddStateChanges(List<StateChange> stateChangesToAdd)
    {
        this.stateChanges.AddRange(stateChangesToAdd);
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
}

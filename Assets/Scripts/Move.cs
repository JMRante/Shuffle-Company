using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move
{
    List<IStateChange> stageChanges;

    public Move() 
    {
        stageChanges = new List<IStateChange>();
    }

    private List<IStateChange> GetActions() 
    {
        return stageChanges;
    }

    public bool RequestStateChange(IStateChange stateChange)
    {
        if (stateChange.IsPossible())
        {
            stageChanges.Add(stateChange);
            return true;
        }
        else
        {
            return false;
        }

        // Calculate resulting state changes
    }

    public void Do() 
    {
        foreach (IStateChange stateChange in stageChanges) 
        {
            stateChange.Do();
        }
    }

    public void Undo() 
    {
        foreach (IStateChange stateChange in stageChanges)
        {
            stateChange.Undo();
        }
    }

    public void Render(float timer)
    {
        foreach (IStateChange stateChange in stageChanges)
        {
            stateChange.Render(timer);
        }
    }
}

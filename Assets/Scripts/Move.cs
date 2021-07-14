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

    private List<StateChange> GetActions() 
    {
        return stateChanges;
    }

    public bool RequestStateChange(StateChange stateChange)
    {
        Queue<StateChange> resultantStateChanges = new Queue<StateChange>();
        resultantStateChanges.Enqueue(stateChange);
        
        while (resultantStateChanges.Count > 0)
        {
            StateChange stateChangeToCheck = resultantStateChanges.Dequeue();
            List<StateChange> nextStateChanges = new List<StateChange>();

            if (stateChangeToCheck.IsPossible(nextStateChanges))
            {
                stateChanges.Add(stateChangeToCheck);
                
                foreach (StateChange sc in nextStateChanges)
                {
                    resultantStateChanges.Enqueue(sc);
                }
            }
            else
            {
                return false;                
            }
        }

        return true;
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

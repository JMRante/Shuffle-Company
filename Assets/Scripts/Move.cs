using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move
{
    private List<IStateChange> stateChanges;

    public Move() 
    {
        stateChanges = new List<IStateChange>();
    }

    private List<IStateChange> GetActions() 
    {
        return stateChanges;
    }

    public bool RequestStateChange(IStateChange stateChange)
    {
        Queue<IStateChange> resultantStateChanges = new Queue<IStateChange>();
        resultantStateChanges.Enqueue(stateChange);
        
        while (resultantStateChanges.Count > 0)
        {
            IStateChange stateChangeToCheck = resultantStateChanges.Dequeue();
            List<IStateChange> nextStateChanges = new List<IStateChange>();

            if (stateChangeToCheck.IsPossible(nextStateChanges))
            {
                stateChanges.Add(stateChangeToCheck);
                
                foreach (IStateChange sc in nextStateChanges)
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
        foreach (IStateChange stateChange in stateChanges) 
        {
            stateChange.Do();
        }
    }

    public void Undo() 
    {
        foreach (IStateChange stateChange in stateChanges)
        {
            stateChange.Undo();
        }
    }

    public void Render(float timer)
    {
        foreach (IStateChange stateChange in stateChanges)
        {
            stateChange.Render(timer);
        }
    }
}

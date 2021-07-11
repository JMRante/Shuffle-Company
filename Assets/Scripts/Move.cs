using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move
{
    List<IAction> actions;

    public Move() 
    {
        actions = new List<IAction>();
    }

    public List<IAction> GetActions() 
    {
        return actions;
    }

    public void Do() 
    {
        foreach (IAction action in actions) 
        {
            action.Do();
        }
    }

    public void Undo() 
    {
        foreach (IAction action in actions)
        {
            action.Undo();
        }
    }
}

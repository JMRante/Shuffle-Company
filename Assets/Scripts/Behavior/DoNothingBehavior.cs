using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoNothingBehavior : MonoBehaviour, IBehavior
{
    public bool IsPassive()
    {
        return true;
    }

    public int GetPriority()
    {
        return 100;
    }

    public List<StateChange> GetStateChanges()
    {
        return null;
    }
}

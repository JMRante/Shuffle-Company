using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBehavior
{
    public int GetPriority();
    public List<StateChange> GetStateChanges();
}

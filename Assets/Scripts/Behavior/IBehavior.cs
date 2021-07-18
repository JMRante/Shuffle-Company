using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBehavior
{
    public int GetPriority();
    public bool IsTriggered();
    public bool IsPossible();
    public List<StateChange> GetStateChanges();
}

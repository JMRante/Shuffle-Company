using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBehavior
{
    public List<StateChange> CheckForStateChanges();
}

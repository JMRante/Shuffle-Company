using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoidStateChange : StateChange
{
    public override StateChangeType GetStateChangeCode()
    {
        return StateChangeType.Translate;
    }

    public override bool IsPossible(List<StateChange> resultingStateChanges)
    {
        return true;
    }

    public override void Do() {}

    public override void Undo() {}

    public override void Render(float timer) {}
}

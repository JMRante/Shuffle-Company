using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StateChangeType
{
    Translate,
    ChangeLocalVariable,
    ChangeGlobalVariable,
    Create,
    Remove,
    ChangeMaterial,
    IncrementItemInInventory,
    DecrementItemInInventory,
    Teleport,
    ChangeLiquidHeight,
    BounceLaser
}

public abstract class StateChange
{
    public abstract StateChangeType GetStateChangeCode();
    public abstract void Do();
    public abstract void Undo();
    public abstract void Render(float timer);
}

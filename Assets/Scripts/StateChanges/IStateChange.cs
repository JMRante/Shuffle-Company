using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StateChange 
{
    Translate,
    ChangeLocalValue,
    ChangeGlobalValue,
    Create,
    Remove,
    IncrementItemInInventory,
    DecrementItemInInventory,
    Teleport,
    ChangeLiquidHeight,
    BounceLaser
}

public interface IStateChange
{
    public StateChange GetStateChangeCode();
    public bool IsPossible();
    public void Do();
    public void Undo();
    public void Render(float timer);
}

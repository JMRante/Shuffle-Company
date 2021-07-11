using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAction
{
    public int GetActionCode();
    public void Do();
    public void Undo();
}

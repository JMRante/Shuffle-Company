using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FillAllSlotsGoal : Goal
{
    private Slot[] slots;

    void Start()
    {
        slots = GameObject.Find("Stage").GetComponentsInChildren<Slot>();
    }

    public override bool IsGoalMet()
    {
        foreach (Slot s in slots)
        {
            if (!s.IsFilled)
            {
                return false;
            }
        }

        return true;
    }
}

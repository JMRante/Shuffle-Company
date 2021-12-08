using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReachGoalBlockGoal : Goal
{
    private GoalBlock[] goalBlocks;

    void Start()
    {
        goalBlocks = GameObject.Find("Stage").GetComponentsInChildren<GoalBlock>();
    }

    public override bool IsGoalMet()
    {
        foreach (GoalBlock gb in goalBlocks)
        {
            if (gb.IsPlayerOnGoal)
            {
                return true;
            }
        }

        return false;
    }
}

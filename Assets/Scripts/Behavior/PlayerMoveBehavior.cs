using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveBehavior : IBehavior
{
    private GameObject player;
    private Vector3 direction;

    public PlayerMoveBehavior(GameObject player, Vector3 direction)
    {
        this.player = player;
        this.direction = direction;
    }

    public int GetPriority()
    {
        return 0;
    }

    public bool IsTriggered()
    {
        return false;
    }

    public bool IsPossible()
    {
        Vector3 positionAhead = player.transform.position + direction;

        GameObject pushableAhead = Queries.FirstElementAtIndexWithProperty(positionAhead, ElementProperty.Pushable);
        GameObject solidAhead = Queries.FirstElementAtIndexWithProperty(positionAhead, ElementProperty.Solid);
        bool canIPush = Queries.ElementHasProperty(player, ElementProperty.Pusher);

        if (canIPush && pushableAhead != null)
        {
            PushableMoveBehavior pushableMove = new PushableMoveBehavior(pushableAhead.gameObject, direction);
            return pushableMove.IsPossible();
        }
        else if (solidAhead)
        {
            return false;
        }

        return true;
    }

    public List<StateChange> GetStateChanges()
    {
        List<StateChange> stateChanges = new List<StateChange>();
        stateChanges.Add(new TranslateStateChange(player, direction));

        Vector3 positionAhead = player.transform.position + direction;
        GameObject pushableAhead = Queries.FirstElementAtIndexWithProperty(positionAhead, ElementProperty.Pushable);
        bool canIPush = Queries.ElementHasProperty(player, ElementProperty.Pusher);

        if (canIPush && pushableAhead != null)
        {
            PushableMoveBehavior pushableMove = new PushableMoveBehavior(pushableAhead.gameObject, direction);
            stateChanges.AddRange(pushableMove.GetStateChanges());
        }
        
        return stateChanges;
    }
}

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

    public List<StateChange> GetStateChanges()
    {
        List<StateChange> stateChanges = new List<StateChange>();

        Vector3 positionAhead = player.transform.position + direction;
        GameObject pushableAhead = Queries.FirstElementAtIndexWithProperty(positionAhead, ElementProperty.Pushable);
        GameObject solidAhead = Queries.FirstElementAtIndexWithProperty(positionAhead, ElementProperty.Solid);
        bool canIPush = Queries.ElementHasProperty(player, ElementProperty.Pusher);

        if (canIPush && pushableAhead != null)
        {
            PushableMoveBehavior pushableMove = new PushableMoveBehavior(pushableAhead.gameObject, direction);
            List<StateChange> pushableStateChanges = pushableMove.GetStateChanges();

            if (pushableStateChanges != null)
            {
                stateChanges.Add(new TranslateStateChange(player, direction));
                stateChanges.AddRange(pushableStateChanges);
                return stateChanges;
            }
            else
            {
                return null;
            }
        }
        else if (solidAhead)
        {
            return null;
        }

        stateChanges.Add(new TranslateStateChange(player, direction));

        return stateChanges;
    }
}

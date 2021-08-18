using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveBehavior : MonoBehaviour, IBehavior
{
    private GridCollisionSystem gcs;
    private Vector3 direction;

    void Start()
    {
        GameObject gameController = GameObject.Find("GameController");

        if (gameController != null)
        {
            gcs = gameController.GetComponent<GridCollisionSystem>();
        }
    }

    public bool IsPassive()
    {
        return true;
    }

    public void SetDirection(Vector3 direction)
    {
        this.direction = direction;
    }

    public int GetPriority()
    {
        return 0;
    }

    public List<StateChange> GetStateChanges()
    {
        List<StateChange> stateChanges = new List<StateChange>();

        Vector3 positionAhead = gameObject.transform.position + direction;
        GameObject pushableAhead = gcs.FirstElementAtIndexWithProperty(positionAhead, ElementProperty.Pushable);
        GameObject solidAhead = gcs.FirstElementAtIndexWithProperty(positionAhead, ElementProperty.Solid);
        bool canIPush = gcs.ElementHasProperty(gameObject, ElementProperty.Pusher);

        if (canIPush && pushableAhead != null)
        {
            PushableMoveBehavior pushableMove = pushableAhead.GetComponent<PushableMoveBehavior>();
            pushableMove.SetDirection(direction);
            List<StateChange> pushableStateChanges = pushableMove.GetStateChanges();

            if (pushableStateChanges != null)
            {
                stateChanges.Add(new TranslateStateChange(gameObject, direction, gcs));
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

        stateChanges.Add(new TranslateStateChange(gameObject, direction, gcs));

        return stateChanges;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushableMoveBehavior : MonoBehaviour, IBehavior
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
        return 1;
    }

    public List<StateChange> GetStateChanges()
    {
        List<StateChange> stateChanges = new List<StateChange>();

        Vector3 positionAhead = gameObject.transform.position + direction;
        GameObject solidAhead = gcs.FirstElementAtIndex(positionAhead, ElementProperty.Solid);
        
        if (solidAhead != null)
        {
            return null;
        }

        Vector3 positionAbove = gameObject.transform.position + Vector3.up;
        GameObject looseObjectAbove = gcs.FirstElementAtIndex(positionAbove, ElementProperty.Loose);

        if (looseObjectAbove != null)
        {
            PushableMoveBehavior pushAboveBehavior = looseObjectAbove.GetComponent<PushableMoveBehavior>();
            pushAboveBehavior.SetDirection(direction);
            List<StateChange> pushAboveBehaviorStateChanges = pushAboveBehavior.GetStateChanges();

            if (pushAboveBehaviorStateChanges != null)
            {
                stateChanges.AddRange(pushAboveBehaviorStateChanges);
            }
            else
            {
                return null;
            }
        }

        stateChanges.Add(new TranslateStateChange(gameObject, direction, gcs));

        return stateChanges;
    }
}

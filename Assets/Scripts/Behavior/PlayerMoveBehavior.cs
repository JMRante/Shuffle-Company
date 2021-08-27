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
        GameObject pushableAhead = gcs.FirstElementAtIndex(positionAhead, ElementProperty.Pushable);
        GameObject solidAhead = gcs.FirstElementAtIndex(positionAhead, ElementProperty.Solid);
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
            GameObject keyBlockAhead = gcs.FirstElementAtIndex(positionAhead, ElementProperty.KeyBlock);
            
            if (keyBlockAhead != null)
            {
                Key keyBlockKeyProperties = keyBlockAhead.GetComponent<Key>();

                StateVariables stateVariables = gameObject.GetComponent<StateVariables>();
                int keyCount = stateVariables.GetInt(keyBlockKeyProperties.keyStateVariableName);

                if (keyCount > 0)
                {
                    UnlockKeyBlockBehavior unlockKeyBlock = keyBlockAhead.GetComponent<UnlockKeyBlockBehavior>();

                    stateChanges.Add(new TranslateStateChange(gameObject, direction, gcs));
                    stateChanges.Add(new ChangeLocalVariableStateChange(gameObject, keyBlockKeyProperties.keyStateVariableName, keyCount - 1, 0.1f));
                    stateChanges.AddRange(unlockKeyBlock.GetStateChanges());

                    return stateChanges;
                }
            }

            return null;
        }

        stateChanges.Add(new TranslateStateChange(gameObject, direction, gcs));

        return stateChanges;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotBehavior : MonoBehaviour, IBehavior
{
    private GridCollisionSystem gcs;
    private StateVariables stateVariables;

    void Start()
    {
        GameObject gameController = GameObject.Find("GameController");

        if (gameController != null)
        {
            gcs = gameController.GetComponent<GridCollisionSystem>();
        }

        stateVariables = gameObject.GetComponent<StateVariables>();
        stateVariables.SetBoolean("filled", gcs.ElementExistsAtIndexWithProperty(gameObject.transform.position, ElementProperty.Slotable));
    }

    public bool IsPassive()
    {
        return false;
    }

    public int GetPriority()
    {
        return 1;
    }

    public List<StateChange> GetStateChanges()
    {
        List<StateChange> stateChanges = new List<StateChange>();

        bool isFilled = gcs.ElementExistsAtIndexWithProperty(gameObject.transform.position, ElementProperty.Slotable);
        bool wasFilled = stateVariables.GetBoolean("filled");

        if (isFilled && !wasFilled)
        {
            stateChanges.Add(new ChangeMaterialStateChange(gameObject, Resources.Load<Material>("Materials/SlotTestFull"), 0.8f));
            stateChanges.Add(new ChangeLocalVariableStateChange(gameObject, "filled", 1, 0));
        }
        else if (!isFilled && wasFilled)
        {
            stateChanges.Add(new ChangeMaterialStateChange(gameObject, Resources.Load<Material>("Materials/SlotTestEmpty"), 0.2f));
            stateChanges.Add(new ChangeLocalVariableStateChange(gameObject, "filled", 0, 0));
        }

        return stateChanges;
    }
}

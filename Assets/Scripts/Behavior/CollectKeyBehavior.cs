using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectKeyBehavior : MonoBehaviour, IBehavior
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
        stateVariables.SetInt("redKeys", 0);
        stateVariables.SetInt("yellowKeys", 0);
        stateVariables.SetInt("greenKeys", 0);
        stateVariables.SetInt("blueKeys", 0);
    }

    public bool IsPassive()
    {
        return false;
    }

    public int GetPriority()
    {
        return 2;
    }

    public List<StateChange> GetStateChanges()
    {
        List<StateChange> stateChanges = new List<StateChange>();

        GameObject key = gcs.FirstElementAtIndex(gameObject.transform.position, ElementProperty.Key);

        if (key != null)
        {
            Key keyProperties = key.GetComponent<Key>();

            int keyCount = stateVariables.GetInt(keyProperties.keyStateVariableName);
            stateChanges.Add(new ChangeLocalVariableStateChange(gameObject, keyProperties.keyStateVariableName, keyCount + 1, 0.5f));
            stateChanges.Add(new ChangeActiveStatusStateChange(key, false, 0.5f));

            return stateChanges;
        }

        return null;
    }
}

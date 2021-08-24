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
        stateVariables.SetInt("yellowKeys", 0);
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

        GameObject key = gcs.FirstElementAtIndexWithProperty(gameObject.transform.position, ElementProperty.Key);

        if (key != null)
        {
            if (key.name.Contains("YellowKey"))
            {
                int yellowKeyCount = stateVariables.GetInt("yellowKeys");
                stateChanges.Add(new ChangeLocalVariableStateChange(gameObject, "yellowKeys", yellowKeyCount + 1, 0.5f));
                stateChanges.Add(new ChangeActiveStatusStateChange(key, false, 0.5f));

                return stateChanges;
            }
        }

        return null;
    }
}

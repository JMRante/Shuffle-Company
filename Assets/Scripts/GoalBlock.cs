using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalBlock : MonoBehaviour
{
    private bool isPlayerOnGoal;
    private Sensor sensor;

    public bool IsPlayerOnGoal
    {
        get => isPlayerOnGoal;
    }

    void Start()
    {
        isPlayerOnGoal = false;
        sensor = GetComponentInChildren<Sensor>();
    }

    void Update()
    {
        bool isPlayerAboveGoal = sensor.DoesCellContainElementProperty(Vector3.up, ElementProperty.Player, Vector3.one * 0.1f, sensor.solidLayerMask);

        if (isPlayerAboveGoal)
        {
            KinematicMover playerMover = (KinematicMover) sensor.GetComponentFromCell(Vector3.up, typeof(KinematicMover));
            
            if (playerMover.Mode == KinematicMoverMode.moving)
            {
                playerMover.Mode = KinematicMoverMode.snapping;
            }
            else if (playerMover.Mode == KinematicMoverMode.snapped)
            {
                isPlayerOnGoal = true;
            }
        }
    }
}

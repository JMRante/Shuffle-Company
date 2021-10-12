using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Conveyable : MonoBehaviour
{
    // public float conveyorSpeed = 5.3f;
    // public Vector3 conveyorDirection = Vector3.zero;

    // private KinematicMover mover;

    // void Start()
    // {
    //     mover = GetComponent<KinematicMover>();
    // }

    // void Update()
    // {
    //     if (IsConveyorBelow() && mover.Mode == KinematicMoverMode.snapped)
    //     {
    //         mover.Velocity = latestInputDirection * walkSpeed;
    //         mover.Mode = KinematicMoverMode.moving;
    //     }
    // }

    // private bool IsConveyorBelow()
    // {
    //     Sensor[] sensors = GetComponentsInChildren<Sensor>();

    //     foreach (Sensor sensor in sensors)
    //     {
    //         if (sensor.IsBlocked(Vector3.down))
    //         {
    //             return true;
    //         }
    //     }

    //     return false;
    // }
}

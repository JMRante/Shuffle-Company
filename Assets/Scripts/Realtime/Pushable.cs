using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pushable : MonoBehaviour
{
    private KinematicMover mover;
    private KinematicMover pusher;
    private SnappingGravity gravityComp;

    void Start()
    {
        mover = GetComponent<KinematicMover>();
        pusher = null;
        gravityComp = GetComponent<SnappingGravity>();
    }

    void Update()
    {
        if (pusher != null)
        {
            mover.Velocity = pusher.Velocity;
            mover.Mode = pusher.Mode;

            if (pusher.Mode == KinematicMoverMode.snapped)
            {
                pusher = null;
            }
        }
    }

    public void Push(KinematicMover pusher)
    {
        this.pusher = pusher;
        mover.Velocity = pusher.Velocity;
        mover.Mode = pusher.Mode;
    }

    public bool CanBePushed(Vector3 direction)
    {
        if (!gravityComp.IsFalling && mover.Mode == KinematicMoverMode.snapped)
        {
            Sensor[] sensors = GetComponentsInChildren<Sensor>();

            foreach (Sensor sensor in sensors)
            {
                if (sensor.IsBlocked(direction))
                {
                    return false;
                }
            }

            return true;
        }
        else
        {
            return false;
        }
    }
}

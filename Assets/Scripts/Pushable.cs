using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
            if (gravityComp.IsFalling)
            {
                pusher = null;
            }
            else
            {
                mover.Velocity = pusher.Velocity;
                mover.Mode = pusher.Mode;

                if (pusher.Mode == KinematicMoverMode.snapped)
                {
                    pusher = null;
                }
            }
        }
    }

    public void Push(KinematicMover pusher)
    {
        this.pusher = pusher;
        mover.Velocity = pusher.Velocity;
        mover.Mode = pusher.Mode;
    }

    public void StopPushing()
    {
        if (pusher != null)
        {
            mover.Mode = KinematicMoverMode.snapping;
            pusher = null;
        }
    }

    public KinematicMoverMode GetMode()
    {
        return mover.Mode;
    }

    public bool CanBePushed(Vector3 direction)
    {
        if (mover.NetVelocity != Vector3.zero)
        {
            return false;
        }

        if (!gravityComp.IsSolidBelow)
        {
            return false;
        }

        if (!gravityComp.IsFalling)
        {
            Sensor[] sensors = Utility.GetComponentsInDirectChildren(gameObject, typeof(Sensor)).Cast<Sensor>().ToArray();

            foreach (Sensor sensor in sensors)
            {
                if (sensor.IsCellBlocked(direction))
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

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Pushable : MonoBehaviour
{
    private KinematicMover mover;
    private KinematicMover pusher;
    private SnappingGravity gravityComp;
    private Conveyable conveyable;

    private Vector3 pusherStartVelocity;

    void Start()
    {
        mover = GetComponent<KinematicMover>();
        pusher = null;
        gravityComp = GetComponent<SnappingGravity>();
        conveyable = GetComponent<Conveyable>();
    }

    void Update()
    {
        if (pusher != null)
        {
            if (gravityComp.IsFalling)
            {
                DisconnectFromPusher();
            }
            else if (pusher.Velocity != pusherStartVelocity)
            {
                StopPushing();
            }
            else
            {
                mover.Velocity = pusher.Velocity;
                mover.Mode = pusher.Mode;

                if (pusher.Mode == KinematicMoverMode.snapped)
                {
                    DisconnectFromPusher();
                }
            }
        }
    }

    public void Push(KinematicMover pusher)
    {
        this.pusher = pusher;
        mover.Velocity = pusher.Velocity;
        pusherStartVelocity = pusher.Velocity;
        mover.Mode = pusher.Mode;
    }

    public void StopPushing()
    {
        if (pusher != null)
        {
            mover.Mode = KinematicMoverMode.snapping;
            DisconnectFromPusher();
        }
    }

    private void DisconnectFromPusher()
    {
        pusher = null;
    }

    public KinematicMoverMode GetMode()
    {
        return mover.Mode;
    }

    public bool CanBePushed(Vector3 direction, GameObject pusher)
    {
        if (mover.NetVelocity != Vector3.zero && pusher.transform.parent != gameObject.transform.parent)
        {
            return false;
        }

        if (!gravityComp.IsSolidBelow)
        {
            return false;
        }

        if (!gravityComp.IsSolidBelowRay)
        {
            return false;
        }

        if (pusher.transform.IsChildOf(gameObject.transform))
        {
            return false;
        }

        if (conveyable != null && conveyable.IsOnConveyor())
        {
            return false;
        }

        if (!gravityComp.IsFalling)
        {
            Sensor[] sensors = Utility.GetComponentsInDirectChildren(gameObject, typeof(Sensor)).Cast<Sensor>().ToArray();

            foreach (Sensor sensor in sensors)
            {
                if (sensor.transform.position - direction == pusher.transform.position)
                {
                    if (!sensor.IsRayBlocked(Vector3.down))
                    {
                        return false;
                    }
                }

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

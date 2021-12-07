using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Conveyable : MonoBehaviour
{
    public bool isTurnableOnConveyor = false;

    private float conveyorSpeed;
    private float conveyorTurnSpeed;
    private Vector3 previousConveyorDirection;
    private Vector3 conveyorDirection;
    private KinematicMover mover;
    private KinematicRotater rotater;
    private SnappingGravity gravity;

    void Start()
    {
        conveyorSpeed = 3.5f;
        conveyorTurnSpeed = 900;
        previousConveyorDirection = Vector3.zero;
        conveyorDirection = Vector3.zero;
        mover = GetComponent<KinematicMover>();
        rotater = GetComponent<KinematicRotater>();
        gravity = GetComponent<SnappingGravity>();
    }

    void Update()
    {
        if (Time.fixedTime > 0.5f && !gravity.IsFalling)
        {
            previousConveyorDirection = conveyorDirection;
            conveyorDirection = GetConveyorDirection();

            Sensor[] sensors = Utility.GetComponentsInDirectChildren(gameObject, typeof(Sensor)).Cast<Sensor>().ToArray();

            foreach (Sensor sensor in sensors)
            {
                Pushable pushableAhead = (Pushable)sensor.GetComponentFromCell(conveyorDirection, typeof(Pushable));
                bool canPushSolidAhead = pushableAhead != null ? pushableAhead.CanBePushed(conveyorDirection, gameObject) : false;

                if (pushableAhead != null)
                {
                    if (canPushSolidAhead && mover.Mode == KinematicMoverMode.snapped && pushableAhead.GetMode() == KinematicMoverMode.snapped)
                    {
                        mover.Velocity = conveyorDirection * conveyorSpeed;
                        mover.Mode = KinematicMoverMode.moving;
                        pushableAhead.Push(mover);
                    }
                    else if (canPushSolidAhead && pushableAhead.GetMode() == KinematicMoverMode.snapped)
                    {
                        if (mover.Mode == KinematicMoverMode.moving)
                        {
                            mover.Mode = KinematicMoverMode.snapping;
                        }
                    }
                }
            }

            if (previousConveyorDirection != conveyorDirection && mover.Mode != KinematicMoverMode.snapped)
            {
                mover.Mode = KinematicMoverMode.snapping;
            }
            else if (conveyorDirection != Vector3.zero && mover.Mode == KinematicMoverMode.snapped)
            {
                mover.Velocity = conveyorDirection * conveyorSpeed;
                mover.Mode = KinematicMoverMode.moving;

                if (isTurnableOnConveyor)
                {
                    rotater.TargetForwardDirection = conveyorDirection;
                    rotater.RotationSpeed = conveyorTurnSpeed;
                }

            }
        }
    }

    public bool IsOnConveyor()
    {
        if (conveyorDirection != Vector3.zero)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private Vector3 GetConveyorDirection()
    {
        Sensor[] sensors = Utility.GetComponentsInDirectChildren(gameObject, typeof(Sensor)).Cast<Sensor>().ToArray();

        Vector3 direction = Vector3.zero;

        foreach (Sensor sensor in sensors)
        {
            if (sensor.DoesRayContainElementProperty(Vector3.down, ElementProperty.Conveyor))
            {
                Transform conveyorTransform = (Transform) sensor.GetComponentFromRay(Vector3.down, typeof(Transform));
                direction += conveyorTransform.forward;
            }
        }

        float directionXAbs = Mathf.Abs(direction.x);
        float directionZAbs = Mathf.Abs(direction.z);

        if (directionXAbs > directionZAbs)
        {
            direction.Set(direction.x, 0f, 0f);
        }
        else if (directionXAbs <= directionZAbs)
        {
            direction.Set(0f, 0f, direction.z);
        }

        return direction.normalized;
    }
}

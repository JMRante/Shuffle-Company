using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Conveyable : MonoBehaviour
{
    private float conveyorSpeed;
    private Vector3 previousConveyorDirection;
    private Vector3 conveyorDirection;
    private KinematicMover mover;

    void Start()
    {
        conveyorSpeed = 8.5f;
        previousConveyorDirection = Vector3.zero;
        conveyorDirection = Vector3.zero;
        mover = GetComponent<KinematicMover>();
    }

    void Update()
    {
        previousConveyorDirection = conveyorDirection;
        conveyorDirection = GetConveyorDirection();

        if (previousConveyorDirection != conveyorDirection)
        {
            mover.Mode = KinematicMoverMode.snapping;
        }
        else if (conveyorDirection != Vector3.zero && mover.Mode == KinematicMoverMode.snapped)
        {
            mover.Velocity = conveyorDirection * conveyorSpeed;
            mover.Mode = KinematicMoverMode.moving;
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

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SnappingGravity : MonoBehaviour
{
    public bool isFallingEnabled = true;
    public bool isBuoyant = false;

    private bool isFalling;
    private bool isSolidBelow;
    private bool isSolidBelowRay;

    private bool isInWater;
    private bool isWaterAbove;
    private bool isBuoying;

    private KinematicMover mover;
    private Friction friction;

    private int solidLayerMask;

    public bool IsFalling
    {
        get { return isFallingEnabled ? isFalling : false; }
    }

    public bool IsSolidBelow
    {
        get => isSolidBelow;
    }

    public bool IsSolidBelowRay
    {
        get => isSolidBelowRay;
    }

    void Start()
    {
        isFalling = false;
        isSolidBelow = true;
        isSolidBelowRay = true;

        isInWater = false;
        isWaterAbove = false;
        isBuoying = false;

        mover = GetComponent<KinematicMover>();
        friction = GetComponent<Friction>();
        
        solidLayerMask = LayerMask.GetMask("Solid");
    }

    void Update()
    {
        CalculateCollisions();

        if (isFallingEnabled)
        {
            if (!isSolidBelow && (!isInWater || !isBuoyant))
            {
                if (mover.Mode == KinematicMoverMode.snapped)
                {
                    mover.Mode = KinematicMoverMode.moving;
                    isFalling = true;
                    SetChildrenFallingState(isFalling);
                }
            }
            else if (!isSolidBelowRay && (!isInWater || !isBuoyant))
            {
                if (mover.Mode == KinematicMoverMode.moving)
                {
                    mover.Mode = KinematicMoverMode.snapping;
                }
            }
            else
            {
                if (isFalling)
                {
                    if (mover.Mode == KinematicMoverMode.moving)
                    {
                        mover.Mode = KinematicMoverMode.snapping;
                    }
                    else if (mover.Mode == KinematicMoverMode.snapped)
                    {
                        isFalling = false;
                        SetChildrenFallingState(isFalling);
                    }
                }
            }

            if (isFalling)
            {
                mover.VelocityY += -9.8f * Time.deltaTime;
            }
        }

        if (isBuoyant)
        {
            if (isWaterAbove)// && isInWater)
            {
                if (mover.Mode == KinematicMoverMode.snapped)
                {
                    mover.Mode = KinematicMoverMode.moving;
                    isBuoying = true;
                    SetChildrenBuoyingState(isBuoying);
                }
            }
            else
            {
                if (isBuoying)
                {
                    if (mover.Mode == KinematicMoverMode.moving)
                    {
                        mover.Mode = KinematicMoverMode.snapping;
                    }
                    else if (mover.Mode == KinematicMoverMode.snapped)
                    {
                        isBuoying = false;
                        SetChildrenBuoyingState(isBuoying);
                    }
                }
            }

            if (isBuoying)
            {
                mover.VelocityY += 6f * Time.deltaTime;
            }
        }
    }

    private void SetChildrenFallingState(bool isFalling)
    {
        SnappingGravity[] childrenGravity = GetComponentsInChildren<SnappingGravity>();
        
        foreach (SnappingGravity grav in childrenGravity)
        {
            grav.isFalling = isFalling;
        }
    }

    private void SetChildrenBuoyingState(bool isBuoying)
    {
        SnappingGravity[] childrenGravity = GetComponentsInChildren<SnappingGravity>();

        foreach (SnappingGravity grav in childrenGravity)
        {
            grav.isBuoying = isBuoying;
        }
    }

    private void CalculateCollisions()
    {
        Sensor[] sensors = Utility.GetComponentsInDirectChildren(gameObject, typeof(Sensor)).Cast<Sensor>().ToArray();

        isSolidBelow = false;
        isSolidBelowRay = false;
        isInWater = false;
        isWaterAbove = false;

        foreach (Sensor sensor in sensors)
        {
            if (sensor.IsRayBlocked(Vector3.down * 0.49f))
            {
                isSolidBelowRay = true;
            }

            if (sensor.IsCellBlocked(Vector3.down, new Vector3(0.47f, 0.49f, 0.45f), sensor.solidLayerMask))
            {
                isSolidBelow = true;
            }

            if (sensor.IsCellBlocked(Vector3.zero, new Vector3(0.49f, 0.49f, 0.49f), sensor.waterLayerMask))
            {
                isInWater = true;
            }

            if (sensor.IsCellBlocked(Vector3.up * 1.2f, new Vector3(0.49f, 0.49f, 0.49f), sensor.waterLayerMask))
            {
                isWaterAbove = true;
            }
        }
    }
}

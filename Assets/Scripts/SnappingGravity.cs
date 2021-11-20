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
    private bool isInWater;
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

    void Start()
    {
        isFalling = false;
        isSolidBelow = true;
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
    }

    private void SetChildrenFallingState(bool isFalling)
    {
        SnappingGravity[] childrenGravity = GetComponentsInChildren<SnappingGravity>();
        
        foreach (SnappingGravity grav in childrenGravity)
        {
            grav.isFalling = isFalling;
        }
    }

    private void CalculateCollisions()
    {
        Sensor[] sensors = Utility.GetComponentsInDirectChildren(gameObject, typeof(Sensor)).Cast<Sensor>().ToArray();

        isSolidBelow = false;
        isInWater = false;

        foreach (Sensor sensor in sensors)
        {
            if (sensor.IsCellBlocked(Vector3.down, new Vector3(0.47f, 0.49f, 0.45f), sensor.SolidLayerMask))
            {
                isSolidBelow = true;
            }

            if (sensor.IsCellBlocked(Vector3.zero, new Vector3(0.47f, 0.49f, 0.45f), sensor.WaterLayerMask))
            {
                isInWater = true;
            }
        }
    }
}

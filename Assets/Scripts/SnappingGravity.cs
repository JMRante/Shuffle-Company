using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SnappingGravity : MonoBehaviour
{
    public bool isFallingEnabled = true;

    private bool isFalling;
    private bool isSolidBelow;
    private KinematicMover mover;

    private int solidLayerMask;

    public bool IsFalling
    {
        get { return isFallingEnabled ? isFalling : false; }
    }

    public bool IsSolidBelow
    {
        get => isSolidBelow;
    }

    // Start is called before the first frame update
    void Start()
    {
        isFalling = false;
        isSolidBelow = true;
        mover = GetComponent<KinematicMover>();
        
        solidLayerMask = LayerMask.GetMask("Solid");
    }

    void Update()
    {
        isSolidBelow = CalculateIsSolidBelow();

        if (isFallingEnabled)
        {
            if (!isSolidBelow)
            {
                if (mover.Mode == KinematicMoverMode.snapped)
                {
                    mover.Mode = KinematicMoverMode.moving;
                    isFalling = true;
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
                    }
                }
            }

            if (isFalling)
            {
                mover.VelocityY += -9.8f * Time.deltaTime;
            }
        }
    }

    private bool CalculateIsSolidBelow()
    {
        Sensor[] sensors = Utility.GetComponentsInDirectChildren(gameObject, typeof(Sensor)).Cast<Sensor>().ToArray();

        foreach (Sensor sensor in sensors)
        {
            if (sensor.IsRayBlocked(Vector3.down))
            {
                return true;
            }
        }

        return false;
    }
}

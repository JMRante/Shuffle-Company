using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnappingGravity : MonoBehaviour
{
    private bool isFalling;
    private bool isSolidBelow;
    private KinematicMover mover;

    private int solidLayerMask;

    public bool IsFalling
    {
        get => isFalling;
    }

    public bool IsSolidBelow
    {
        get => isSolidBelow;
    }

    // Start is called before the first frame update
    void Start()
    {
        isFalling = false;
        isSolidBelow = Physics.Raycast(transform.position + (0.49f * Vector3.down), Vector3.down, 0.5f, solidLayerMask);
        mover = GetComponent<KinematicMover>();
        
        solidLayerMask = LayerMask.GetMask("Solid");
    }

    // Update is called once per frame
    void Update()
    {
        isSolidBelow = Physics.Raycast(transform.position + (0.49f * Vector3.down), Vector3.down, 0.5f, solidLayerMask);

        Debug.Log(mover.Velocity + ", " + mover.Mode + ", " + isFalling + ", " + isSolidBelow);

        if (!isSolidBelow)
        {
            if (mover.Mode == KinematicMoverMode.Snapped)
            {
                mover.Mode = KinematicMoverMode.Moving;
                Debug.Log("Velocity is " + mover.Velocity + ", " + (mover.Velocity.y + (-9.8f * Time.deltaTime)));
                isFalling = true;
            }
        }
        else
        {
            if (isFalling)
            {
                if (mover.Mode == KinematicMoverMode.Moving)
                {
                    mover.Mode = KinematicMoverMode.Snapping;
                }
                else if (mover.Mode == KinematicMoverMode.Snapped)
                {
                    isFalling =false;
                }
            }
        }

        if (isFalling)
        {
            mover.VelocityY += -9.8f * Time.deltaTime;
        }
    }
}

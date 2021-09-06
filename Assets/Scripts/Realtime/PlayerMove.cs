using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float walkSpeed = 3.7f;
    
    private List<Vector3> inputDirections;
    private KinematicMover mover;
    private SnappingGravity gravityComp;
    
    private int solidLayerMask;

    void Start()
    {
        inputDirections = new List<Vector3>();
        mover = GetComponent<KinematicMover>();
        gravityComp = GetComponent<SnappingGravity>();

        solidLayerMask = LayerMask.GetMask("Solid");
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
            inputDirections.Add(Vector3.forward);
        else if (Input.GetKeyDown(KeyCode.RightArrow))
            inputDirections.Add(Vector3.right);
        else if (Input.GetKeyDown(KeyCode.DownArrow))
            inputDirections.Add(Vector3.back);
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
            inputDirections.Add(Vector3.left);

        if (Input.GetKeyUp(KeyCode.UpArrow))
            inputDirections.Remove(Vector3.forward);
        else if (Input.GetKeyUp(KeyCode.RightArrow))
            inputDirections.Remove(Vector3.right);
        else if (Input.GetKeyUp(KeyCode.DownArrow))
            inputDirections.Remove(Vector3.back);
        else if (Input.GetKeyUp(KeyCode.LeftArrow))
            inputDirections.Remove(Vector3.left);

        if (!Input.GetKey(KeyCode.UpArrow) && !Input.GetKey(KeyCode.RightArrow) && !Input.GetKey(KeyCode.DownArrow) && !Input.GetKey(KeyCode.LeftArrow))
            inputDirections.Clear();

        if (!gravityComp.IsFalling)
        {
            if (inputDirections.Count > 0)
            {
                Vector3 latestInputDirection = inputDirections[inputDirections.Count - 1];

                if (Physics.CheckSphere(transform.position + latestInputDirection, 0.49f, solidLayerMask) || !gravityComp.IsSolidBelow)
                {
                    if (mover.Mode == KinematicMoverMode.Moving)
                    {
                        mover.Mode = KinematicMoverMode.Snapping;
                    }
                }
                else if (mover.Velocity.normalized != latestInputDirection && mover.Velocity != Vector3.zero)
                {
                    mover.Mode = KinematicMoverMode.Snapping;
                }
                else if (mover.Mode == KinematicMoverMode.Snapped)
                {
                    mover.Velocity = latestInputDirection * walkSpeed;
                    mover.Mode = KinematicMoverMode.Moving;
                }
            }
            else if (mover.Mode != KinematicMoverMode.Snapped)
            {
                mover.Mode = KinematicMoverMode.Snapping;
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float walkSpeed = 3.7f;
    public float pushSpeed = 3.7f;
    
    private List<Vector3> inputDirections;
    private KinematicMover mover;
    private SnappingGravity gravityComp;
    private Pushable lastPushable;
    
    private int solidLayerMask;

    void Start()
    {
        inputDirections = new List<Vector3>();
        mover = GetComponent<KinematicMover>();
        gravityComp = GetComponent<SnappingGravity>();
        lastPushable = null;

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

                Collider[] collidersAhead = Physics.OverlapBox(transform.position + (latestInputDirection * 0.51f), Vector3.one * 0.45f, Quaternion.identity, solidLayerMask);
                bool isSolidAhead = false;
                bool canPushSolidAhead = false;
                Pushable pushableAhead = null;

                foreach (Collider colliderAhead in collidersAhead)
                {
                    if (colliderAhead.transform.parent.gameObject != transform.gameObject)
                    {
                        isSolidAhead = true;
                    }

                    Pushable tempPushableAhead = colliderAhead.GetComponentInParent<Pushable>();
                    if (tempPushableAhead != null)
                    {
                        canPushSolidAhead = tempPushableAhead.CanBePushed(latestInputDirection);
                        pushableAhead = tempPushableAhead;
                    }
                }

                if (canPushSolidAhead && mover.Mode == KinematicMoverMode.snapped)
                {
                    mover.Velocity = latestInputDirection * pushSpeed;
                    mover.Mode = KinematicMoverMode.moving;
                    pushableAhead.Push(mover);
                }
                else if (((!canPushSolidAhead || (canPushSolidAhead && pushableAhead.GetMode() == KinematicMoverMode.snapped)) && isSolidAhead) || !gravityComp.IsSolidBelow)
                {
                    if (mover.Mode == KinematicMoverMode.moving)
                    {
                        mover.Mode = KinematicMoverMode.snapping;
                    }
                }
                else if (mover.Velocity.normalized != latestInputDirection && mover.Velocity != Vector3.zero)
                {
                    mover.Mode = KinematicMoverMode.snapping;
                }
                else if (mover.Mode == KinematicMoverMode.snapped)
                {
                    mover.Velocity = latestInputDirection * walkSpeed;
                    mover.Mode = KinematicMoverMode.moving;
                }

                if (lastPushable != pushableAhead)
                {
                    if (lastPushable != null)
                    {
                        lastPushable.StopPushing();
                    }

                    lastPushable = pushableAhead;
                }
            }
            else if (mover.Mode != KinematicMoverMode.snapped)
            {
                mover.Mode = KinematicMoverMode.snapping;
            }
        }
    }
}

// if (pushableAhead != null && pushableAhead.IsMoving())
// {
//     pushableAhead.StopPushing();
// }

// if (pushableAhead != null && pushableAhead.CanBePushed(latestInputDirection))
// {
//     pushableAhead.Push(latestInputDirection * pushSpeed);
//     mover.Velocity = latestInputDirection * pushSpeed;
// }
// else
// {
//     mover.Velocity = latestInputDirection * walkSpeed;
// }

// void OnDrawGizmos()
// {
//     if (inputDirections.Count > 0)
//     {
//         Vector3 latestInputDirection = inputDirections[inputDirections.Count - 1];
//         Gizmos.color = Color.green;
//         Gizmos.DrawWireCube(transform.position + latestInputDirection, new Vector3(1, 1, 1));
//     }
// }
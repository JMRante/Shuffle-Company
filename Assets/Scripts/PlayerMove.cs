using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float walkSpeed = 4.8f;
    public float pushSpeed = 4.8f;
    public float walkRotationSpeed = 500f;
    
    private List<Vector3> inputDirections;
    private Vector3 latestInputDirection;

    private KinematicMover mover;
    private KinematicRotater rotator;
    private SnappingGravity gravityComp;
    private Sensor sensor;
    private Pushable lastPushable;

    private bool isClimbing;
    private Vector3 climbDirection;

    private int solidLayerMask;

    void Start()
    {
        inputDirections = new List<Vector3>();
        latestInputDirection = Vector3.zero;

        mover = GetComponent<KinematicMover>();
        rotator = GetComponent<KinematicRotater>();
        gravityComp = GetComponent<SnappingGravity>();
        sensor = GetComponentInChildren<Sensor>();
        lastPushable = null;

        isClimbing = false;
        climbDirection = Vector3.zero;

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

        if (inputDirections.Count > 0)
        {
            latestInputDirection = inputDirections[inputDirections.Count - 1];
        }
        else
        {
            latestInputDirection = Vector3.zero;
        }
    }

    void FixedUpdate()
    {
        // Check surroundings
        bool isSolidAhead = sensor.IsCellBlocked(latestInputDirection);
        Pushable pushableAhead = (Pushable) sensor.GetComponentFromCell(latestInputDirection, typeof(Pushable));
        bool canPushSolidAhead = pushableAhead != null ? pushableAhead.CanBePushed(latestInputDirection) : false;

        // Set Motion
        if (!gravityComp.IsFalling)
        {
            if (latestInputDirection != Vector3.zero)
            {
                // Turn
                if (mover.Mode == KinematicMoverMode.snapped)
                {
                    rotator.TargetForwardDirection = latestInputDirection;
                    rotator.RotationSpeed = walkRotationSpeed;
                }

                // Push
                if (canPushSolidAhead && mover.Mode == KinematicMoverMode.snapped)
                {
                    mover.Velocity = latestInputDirection * pushSpeed;
                    mover.Mode = KinematicMoverMode.moving;
                    pushableAhead.Push(mover);
                }
                // Stop
                else if (((!canPushSolidAhead || (canPushSolidAhead && pushableAhead.GetMode() == KinematicMoverMode.snapped)) && isSolidAhead) || !gravityComp.IsSolidBelow)
                {
                    if (mover.Mode == KinematicMoverMode.moving)
                    {
                        mover.Mode = KinematicMoverMode.snapping;
                    }
                }
                // Change Walk Direction
                else if (mover.Velocity.normalized != latestInputDirection && mover.Velocity != Vector3.zero)
                {
                    mover.Mode = KinematicMoverMode.snapping;
                }
                // Walk
                else if (mover.Mode == KinematicMoverMode.snapped)
                {
                    mover.Velocity = latestInputDirection * walkSpeed;
                    mover.Mode = KinematicMoverMode.moving;
                }

                // Stop Push
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

// Element elementAhead = colliderAhead.GetComponentInParent<Element>();
// if (elementAhead != null && elementAhead.HasProperty(ElementProperty.Ladder))
// {
//     Transform ladderTransform = colliderAhead.transform.parent;

//     if (ladderTransform.forward + latestInputDirection == Vector3.zero)
//     {
//         climbDirection = latestInputDirection;
//         isLadderAhead = true;
//     }
// }

// Element elementAhead = colliderAhead.GetComponentInParent<Element>();
// if (elementAhead != null && elementAhead.elementId == "Ladder")
// {
//     Transform ladderTransform = colliderAhead.transform.parent;

//     if (ladderTransform.forward + latestInputDirection == Vector3.zero)
//     {
//         climbDirection = latestInputDirection;
//         isLadderAhead = true;
//     }
// }

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




// Collider[] collidersBelow = Physics.OverlapBox(transform.position + Vector3.down, Vector3.one * 0.45f, Quaternion.identity, solidLayerMask);
// bool isLadderBelow = false;

// foreach (Collider colliderBelow in collidersBelow)
// {
//     Element elementBelow = colliderBelow.GetComponentInParent<Element>();
//     if (elementBelow != null && elementBelow.elementId == "Ladder")
//     {
//         Transform ladderTransform = colliderBelow.transform.parent;

//         if (ladderTransform.forward - latestInputDirection == Vector3.zero)
//         {
//             climbDirection = -latestInputDirection;
//             isLadderBelow = true;
//         }
//     }
// }

// Collider[] collidersAbove = Physics.OverlapBox(transform.position + Vector3.up, Vector3.one * 0.45f, Quaternion.identity, solidLayerMask);
// bool isSolidAbove = false;

// foreach (Collider colliderAbove in collidersAbove)
// {
//     if (colliderAbove.transform.parent.gameObject != transform.gameObject)
//     {
//         isSolidAbove = true;
//     }
// }

// Collider[] collidersAboveAhead = Physics.OverlapBox(transform.position + latestInputDirection + Vector3.up, Vector3.one * 0.45f, Quaternion.identity, solidLayerMask);
// bool isSolidAboveAhead = false;

// foreach (Collider colliderAboveAhead in collidersAboveAhead)
// {
//     if (colliderAboveAhead.transform.parent.gameObject != transform.gameObject)
//     {
//         isSolidAboveAhead = true;
//     }
// }

// Collider[] collidersBelowAhead = Physics.OverlapBox(transform.position + latestInputDirection + Vector3.down, Vector3.one * 0.45f, Quaternion.identity, solidLayerMask);
// bool isSolidBelowAhead = false;

// foreach (Collider colliderBelowAhead in collidersBelowAhead)
// {
//     if (colliderBelowAhead.transform.parent.gameObject != transform.gameObject)
//     {
//         isSolidBelowAhead = true;
//     }
// }

// Act on Surroundings
// if (isLadderAhead || isLadderBelow || isClimbing)
// {
//     Vector3 climbInputDirection = Quaternion.AngleAxis(90f, Vector3.Cross(climbDirection, Vector3.up)) * latestInputDirection;

//     if (mover.Mode == KinematicMoverMode.snapped)
//     {
//         isClimbing = true;
//         gravityComp.enabled = false;
//     }
//     else
//     {

//     }
// }
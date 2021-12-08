using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float walkSpeed = 4.8f;
    public float pushSpeed = 4.8f;
    public float walkRotationSpeed = 500f;

    public KeyCode upInput;
    public KeyCode rightInput;
    public KeyCode downInput;
    public KeyCode leftInput;

    private List<Vector3> inputDirections;
    private Vector3 latestInputDirection;
    private Vector3 latestInputDirectionRaw;

    private KinematicMover mover;
    private KinematicRotater rotator;
    private SnappingGravity gravityComp;
    private Sensor sensor;
    private Pushable lastPushable;
    private Conveyable conveyable;
    private Destructable destructable;
    private Collector collector;

    private bool isClimbing;
    private Vector3 climbDirection;

    private Vector3[] climbUpPath;
    private Vector3[] climbDownPath;

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
        conveyable = GetComponent<Conveyable>();
        destructable = GetComponent<Destructable>();
        collector = GetComponent<Collector>();

        isClimbing = false;
        climbDirection = Vector3.zero;

        climbUpPath = new Vector3[] 
        { 
            new Vector3(0f, 0.309f, 0.048f),
            new Vector3(0f, 0.588f, 0.191f),
            new Vector3(0f, 0.809f, 0.412f),
            new Vector3(0f, 0.951f, 0.691f),
            new Vector3(0f, 1f, 1f)
        };
        climbDownPath = new Vector3[]
        {
            new Vector3(0f, -0.048f, 0.309f),
            new Vector3(0f, -0.191f, 0.588f),
            new Vector3(0f, -0.412f, 0.809f),
            new Vector3(0f, -0.691f, 0.951f),
            new Vector3(0f, -1f, 1f)
        };

        solidLayerMask = LayerMask.GetMask("Solid");
    }

    void Update()
    {
        if (Input.GetKeyDown(upInput))
            inputDirections.Add(Vector3.forward);
        else if (Input.GetKeyDown(rightInput))
            inputDirections.Add(Vector3.right);
        else if (Input.GetKeyDown(downInput))
            inputDirections.Add(Vector3.back);
        else if (Input.GetKeyDown(leftInput))
            inputDirections.Add(Vector3.left);

        if (Input.GetKeyUp(upInput))
            inputDirections.Remove(Vector3.forward);
        else if (Input.GetKeyUp(rightInput))
            inputDirections.Remove(Vector3.right);
        else if (Input.GetKeyUp(downInput))
            inputDirections.Remove(Vector3.back);
        else if (Input.GetKeyUp(leftInput))
            inputDirections.Remove(Vector3.left);

        if (!Input.GetKey(upInput) && !Input.GetKey(rightInput) && !Input.GetKey(downInput) && !Input.GetKey(leftInput))
            inputDirections.Clear();

        if (inputDirections.Count > 0)
        {
            // Use climbing axis if climbing
            if (isClimbing)
            {
                latestInputDirection = Utility.Round(Quaternion.AngleAxis(90f, Vector3.Cross(climbDirection, Vector3.up)) * inputDirections[inputDirections.Count - 1]);
                latestInputDirectionRaw = inputDirections[inputDirections.Count - 1];
            }
            else
            {
                latestInputDirection = inputDirections[inputDirections.Count - 1];
                latestInputDirectionRaw = latestInputDirection;
            }
        }
        else
        {
            latestInputDirection = Vector3.zero;
        }

        // Check surroundings
        bool isSolidAhead = sensor.IsCellBlocked(latestInputDirection);

        Pushable pushableAhead = (Pushable) sensor.GetComponentFromCell(latestInputDirection, typeof(Pushable));
        bool canPushSolidAhead = pushableAhead != null ? pushableAhead.CanBePushed(latestInputDirection, gameObject) : false;

        bool isClimbableAhead = sensor.DoesRayContainElementProperty(latestInputDirectionRaw, ElementProperty.Climbable);
        bool isClimbableFacingAhead = sensor.DoesRayContainElementProperty(latestInputDirectionRaw, ElementProperty.Climbable)
            && ((Transform)sensor.GetComponentFromRay(latestInputDirectionRaw, typeof(Transform))).forward + transform.forward == Vector3.zero;
        bool isOnClimbable = sensor.DoesRayContainElementProperty(transform.forward, ElementProperty.Climbable) 
            && ((Transform)sensor.GetComponentFromRay(transform.forward, typeof(Transform))).forward + transform.forward == Vector3.zero;
        bool isClimbableBelowInDirection = sensor.DoesRayContainElementProperty(Vector3.down, ElementProperty.Climbable)
            && ((Transform)sensor.GetComponentFromRay(Vector3.down, typeof(Transform))).forward - latestInputDirectionRaw == Vector3.zero;

        bool isSolidAboveAheadForClimbable = sensor.IsCellPositionBlocked(transform.position + Vector3.up + latestInputDirectionRaw);
        bool isSolidBelowAheadForClimbable = sensor.IsCellPositionBlocked(transform.position + Vector3.down + latestInputDirectionRaw);
        bool isSolidAbove = sensor.IsCellPositionBlocked(transform.position + Vector3.up);

        // Can walk into key gates when you have the key
        KeyGate keyGateAhead = (KeyGate) sensor.GetComponentFromRay(latestInputDirection, typeof(KeyGate));
        bool openableKeyGateAhead = keyGateAhead != null && collector.GetCollectableCount(keyGateAhead.keyMatch) > 0 ? true : false;
        
        // Set Motion
        if (!gravityComp.IsFalling && mover.Mode != KinematicMoverMode.pathing && !conveyable.IsOnConveyor())
        {
            // Start Falling After Climbing
            if (isClimbing && !isOnClimbable && mover.Mode != KinematicMoverMode.pathing)
            {
                StopClimbing();
            }

            if (latestInputDirection != Vector3.zero)
            {
                // Start Climbing
                if (!isClimbing && isClimbableAhead && mover.Mode == KinematicMoverMode.snapped)
                {
                    Transform climbableTransform = (Transform)sensor.GetComponentFromCell(latestInputDirectionRaw, typeof(Transform));

                    if (climbableTransform.forward + transform.forward == Vector3.zero)
                    {
                        climbDirection = latestInputDirection;
                        isClimbing = true;
                        gravityComp.isFallingEnabled = false;
                    }
                }

                // Turn
                if (mover.Mode == KinematicMoverMode.snapped && !isClimbing && gravityComp.IsSolidBelow)
                {
                    rotator.TargetForwardDirection = latestInputDirection;
                    rotator.RotationSpeed = walkRotationSpeed;
                }

                // Unlock key gate
                if (mover.Mode == KinematicMoverMode.snapped && openableKeyGateAhead && !keyGateAhead.IsUnlocked)
                {
                    keyGateAhead.Unlock();
                    collector.ReduceCollectableCount(keyGateAhead.keyMatch, 1);
                }

                // Climb Onto Climbable From Above
                if (!isClimbing && !isSolidAhead && !isSolidBelowAheadForClimbable && isClimbableBelowInDirection)
                {
                    Transform climbableTransform = (Transform)sensor.GetComponentFromRay(Vector3.down, typeof(Transform));

                    if (mover.Mode == KinematicMoverMode.snapped || Vector3.Angle(climbableTransform.forward, transform.forward) < 80f)
                    {
                        climbDirection = -latestInputDirectionRaw;
                        isClimbing = true;
                        gravityComp.isFallingEnabled = false;

                        rotator.TargetForwardDirection = climbDirection;
                        rotator.RotationSpeed = walkRotationSpeed * 2f;

                        mover.Mode = KinematicMoverMode.pathing;
                        mover.Path = climbDownPath;
                        mover.PathingSpeed = walkSpeed;
                        mover.PathRotation = Quaternion.LookRotation(latestInputDirectionRaw, Vector3.up);
                    }
                }
                // Climb Atop Climbable
                else if (isClimbing && isClimbableAhead && isClimbableFacingAhead && !isSolidAboveAheadForClimbable && !isSolidAbove)
                {
                    mover.Mode = KinematicMoverMode.pathing;
                    mover.Path = climbUpPath;
                    mover.PathingSpeed = walkSpeed;
                    mover.PathRotation = Quaternion.LookRotation(latestInputDirectionRaw, Vector3.up);
                }
                // Dismount Climbing On Solid
                else if (isClimbing && gravityComp.IsSolidBelow && climbDirection != latestInputDirectionRaw && mover.Mode == KinematicMoverMode.snapped)
                {
                    StopClimbing();
                }
                // Push
                else if (canPushSolidAhead && mover.Mode == KinematicMoverMode.snapped)
                {
                    mover.Velocity = latestInputDirection * pushSpeed;
                    mover.Mode = KinematicMoverMode.moving;
                    pushableAhead.Push(mover);
                }
                // Stop
                else if (((!canPushSolidAhead || (canPushSolidAhead && pushableAhead.GetMode() == KinematicMoverMode.snapped)) && isSolidAhead && !openableKeyGateAhead) || (!gravityComp.IsSolidBelow && gravityComp.isFallingEnabled))
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
                // Stop When No Input
                mover.Mode = KinematicMoverMode.snapping;
            }
        }

        if (mover.Mode == KinematicMoverMode.pathing)
        {
            destructable.IsDestructionDetectionEnabled = false;
        }
        else
        {
            destructable.IsDestructionDetectionEnabled = true;
        }
    }

    private void StopClimbing()
    {
        climbDirection = Vector3.zero;
        isClimbing = false;
        gravityComp.isFallingEnabled = true;
        mover.Mode = KinematicMoverMode.snapping;
    }
}
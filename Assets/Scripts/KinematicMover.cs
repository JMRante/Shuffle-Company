using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum KinematicMoverMode
{
    snapped,
    snapping,
    moving,
    pathing
}

public class KinematicMover : MonoBehaviour
{
    private Rigidbody rb;
    private KinematicMoverMode mode;
    private Vector3 velocity;
    private Vector3 moveToSnapPointVelocity;
    private float snapSpeed;
    // private Transform parentTransform;

    private Vector3[] path;
    private int pathIndex;
    private float pathingSpeed;
    private Vector3 pathStart;
    private Quaternion pathRotation;

    public KinematicMoverMode Mode
    {
        get => mode;
        set => mode = value;
    }

    public Vector3 Velocity
    {
        get => velocity;
        set => velocity = value;
    }

    public float VelocityX
    {
        get => velocity.x;
        set => velocity.x = value;
    }

    public float VelocityY
    {
        get => velocity.y;
        set => velocity.y = value;
    }

    public float VelocityZ
    {
        get => velocity.z;
        set => velocity.z = value;
    }

    // public Transform ParentTransform
    // {
    //     get => parentTransform;
    //     set => parentTransform = value;
    // }

    public Vector3[] Path
    {
        set 
        { 
            path = value;
            pathIndex = 0;
            pathStart = transform.position;
        }
    }

    public float PathingSpeed
    {
        get => pathingSpeed;
        set => pathingSpeed = value;
    }

    public Quaternion PathRotation
    {
        get => pathRotation;
        set => pathRotation = value;
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        mode = KinematicMoverMode.snapped;
        velocity = Vector3.zero;
        snapSpeed = 0f;
        // parentTransform = null;

        path = null;
        pathIndex = 0;
        pathingSpeed = 0f;
        pathStart = Vector3.zero;
        pathRotation = Quaternion.identity;
    }

    void FixedUpdate()
    {
        switch (mode)
        {
            case KinematicMoverMode.snapped: 
                break;
            case KinematicMoverMode.snapping:
            {
                if (velocity == Vector3.zero)
                {
                    Snap(Utility.Round(transform.localPosition));
                    break;
                }

                snapSpeed = velocity.magnitude;

                Vector3 closestSnapPoint = Utility.Round(transform.localPosition);
                moveToSnapPointVelocity = Vector3.Normalize(closestSnapPoint - transform.localPosition) * snapSpeed;

                if (moveToSnapPointVelocity.normalized == velocity.normalized)
                {
                    velocity = moveToSnapPointVelocity;

                    Vector3 currentNorm = Vector3.Normalize(closestSnapPoint - transform.localPosition);
                    Vector3 overshotNorm = Vector3.Normalize(closestSnapPoint - (transform.localPosition + (velocity * Time.deltaTime)));

                    if (currentNorm != overshotNorm)
                    {
                        Snap(closestSnapPoint);
                    }
                    else
                    {
                        rb.MovePosition(transform.position + (velocity * Time.deltaTime));
                    }
                }
                else
                {
                    rb.MovePosition(transform.position + (velocity * Time.deltaTime));
                    // Debug.Log("v: " + velocity + "sp: " + closestSnapPoint + ", spv: " + moveToSnapPointVelocity);
                }

                break;
            }
            case KinematicMoverMode.moving:
            {
                rb.MovePosition(transform.position + (velocity * Time.deltaTime));
                break;
            }
            case KinematicMoverMode.pathing:
            {
                if (path != null && path.Length > 0)
                {
                    if (pathIndex < path.Length)
                    {
                        Vector3 destinationPoint = (pathRotation * path[pathIndex]) + pathStart;
                        velocity = Vector3.Normalize(destinationPoint - transform.position) * pathingSpeed;

                        Vector3 currentNorm = Vector3.Normalize(destinationPoint - transform.position);
                        Vector3 overshotNorm = Vector3.Normalize(destinationPoint - (transform.position + (velocity * Time.deltaTime)));

                        if (currentNorm != overshotNorm || velocity == Vector3.zero)
                        {
                            pathIndex++;
                        }

                        rb.MovePosition(transform.position + (velocity * Time.deltaTime));
                    }
                    
                    if (pathIndex >= path.Length)
                    {
                        Snap(Utility.Round(transform.position));
                    }
                }

                break;
            }
        }
    }

    public void Snap(Vector3 snapPoint)
    {
        rb.MovePosition(snapPoint);
        velocity = Vector3.zero;
        mode = KinematicMoverMode.snapped;
        snapSpeed = 0f;

        path = null;
        pathingSpeed = 0f;
        pathIndex = 0;
        pathStart = Vector3.zero;
    }
}
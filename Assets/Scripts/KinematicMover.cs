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
    private float snapSpeed;
    private Vector3 netVelocity;

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

    public Vector3 NetVelocity
    {
        get => netVelocity;
        set => netVelocity = value;
    }

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

        path = null;
        pathIndex = 0;
        pathingSpeed = 0f;
        pathStart = Vector3.zero;
        pathRotation = Quaternion.identity;
    }

    void Update()
    {
        Vector3 localVelocity = Quaternion.Inverse(transform.parent.rotation) * velocity;

        switch (mode)
        {
            case KinematicMoverMode.snapped: 
                break;
            case KinematicMoverMode.snapping:
            {
                if (localVelocity == Vector3.zero)
                {
                    Snap(Utility.Round(transform.localPosition));
                    break;
                }

                snapSpeed = localVelocity.magnitude;

                Vector3 closestSnapPoint = Utility.Round(transform.localPosition);
                Vector3 moveToSnapPointVelocity = Vector3.Normalize(closestSnapPoint - transform.localPosition) * snapSpeed;

                if (Vector3.Angle(moveToSnapPointVelocity, localVelocity) < 90f)
                {
                    localVelocity = moveToSnapPointVelocity;
                    velocity = transform.parent.rotation * localVelocity;

                    Vector3 currentNorm = Vector3.Normalize(closestSnapPoint - transform.localPosition);
                    Vector3 overshotNorm = Vector3.Normalize(closestSnapPoint - (transform.localPosition + (localVelocity * Time.deltaTime)));

                    if (currentNorm != overshotNorm || currentNorm == Vector3.zero)
                    {
                        Snap(closestSnapPoint);
                    }
                    else
                    {
                        transform.localPosition = transform.localPosition + (localVelocity * Time.deltaTime);
                    }
                }
                else
                {
                    transform.localPosition = transform.localPosition + (localVelocity * Time.deltaTime);
                }

                break;
            }
            case KinematicMoverMode.moving:
            {
                transform.localPosition = transform.localPosition + (localVelocity * Time.deltaTime);
                break;
            }
            case KinematicMoverMode.pathing:
            {
                if (path != null && path.Length > 0)
                {
                    if (pathIndex < path.Length)
                    {
                        Vector3 destinationPoint = (pathRotation * path[pathIndex]) + pathStart;
                        localVelocity = Vector3.Normalize(destinationPoint - transform.position) * pathingSpeed;
                        velocity = transform.parent.rotation * localVelocity;

                        Vector3 currentNorm = Vector3.Normalize(destinationPoint - transform.position);
                        Vector3 overshotNorm = Vector3.Normalize(destinationPoint - (transform.position + (localVelocity * Time.deltaTime)));

                        if (currentNorm != overshotNorm || localVelocity == Vector3.zero)
                        {
                            pathIndex++;
                        }

                        transform.localPosition = transform.localPosition + (localVelocity * Time.deltaTime);
                    }
                    
                    if (pathIndex >= path.Length)
                    {
                        Snap(Utility.Round(transform.localPosition));
                    }
                }

                break;
            }
        }

        CalculateNetVelocity();
    }

    private void CalculateNetVelocity()
    {
        KinematicMover[] parentMovers = GetComponentsInParent<KinematicMover>();

        netVelocity = Vector3.zero;

        foreach (KinematicMover parentMover in parentMovers)
        {
            netVelocity += parentMover.Velocity;
        }
    }

    public void Snap(Vector3 snapPoint)
    {
        transform.localPosition = snapPoint;
        velocity = Vector3.zero;
        mode = KinematicMoverMode.snapped;
        snapSpeed = 0f;

        path = null;
        pathingSpeed = 0f;
        pathIndex = 0;
        pathStart = Vector3.zero;
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum KinematicMoverMode
{
    snapped,
    snapping,
    moving
}

public class KinematicMover : MonoBehaviour
{
    private Rigidbody rb;
    private KinematicMoverMode mode;
    private Vector3 velocity;
    private float snapSpeed;

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

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        mode = KinematicMoverMode.snapped;
        velocity = Vector3.zero;
        snapSpeed = 0f;
    }

    void FixedUpdate()
    {
        switch (mode)
        {
            case KinematicMoverMode.snapped: break;
            case KinematicMoverMode.snapping:
            {
                if (velocity == Vector3.zero && transform.position == Utility.Round(transform.position))
                {
                    Snap(transform.position);
                }

                snapSpeed = velocity.magnitude;

                Vector3 closestSnapPoint = Utility.Round(transform.position);
                Vector3 moveToSnapPointVelocity = Vector3.Normalize(closestSnapPoint - transform.position) * snapSpeed;

                if (moveToSnapPointVelocity.normalized == velocity.normalized)
                {
                    velocity = moveToSnapPointVelocity;

                    Vector3 currentNorm = Vector3.Normalize(closestSnapPoint - transform.position);
                    Vector3 overshotNorm = Vector3.Normalize(closestSnapPoint - (transform.position + (velocity * Time.deltaTime)));

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
                }

                break;
            }
            case KinematicMoverMode.moving:
            {
                rb.MovePosition(transform.position + (velocity * Time.deltaTime));
                break;
            }
        }
    }

    private void Snap(Vector3 snapPoint)
    {
        rb.MovePosition(snapPoint);
        velocity = Vector3.zero;
        mode = KinematicMoverMode.snapped;
        snapSpeed = 0f;
    }
}
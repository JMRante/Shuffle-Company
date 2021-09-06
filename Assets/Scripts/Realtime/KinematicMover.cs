using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum KinematicMoverMode
{
    Snapped,
    Snapping,
    Moving
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
        mode = KinematicMoverMode.Snapped;
        velocity = Vector3.zero;
        snapSpeed = 0f;
    }

    void FixedUpdate()
    {
        switch (mode)
        {
            case KinematicMoverMode.Snapped: break;
            case KinematicMoverMode.Snapping:
            {
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
                        rb.MovePosition(closestSnapPoint);
                        velocity = Vector3.zero;
                        mode = KinematicMoverMode.Snapped;
                        snapSpeed = 0f;
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
            case KinematicMoverMode.Moving:
            {
                rb.MovePosition(transform.position + (velocity * Time.deltaTime));
                break;
            }
        }
    }
}
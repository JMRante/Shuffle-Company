using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KinematicRotater : MonoBehaviour
{
    private Rigidbody rb;
    private Vector3 targetForwardDirection;
    private float rotationSpeed = 0f;

    public Vector3 TargetForwardDirection
    {
        get => targetForwardDirection;
        set => targetForwardDirection = value;
    }

    public float RotationSpeed
    {
        get => rotationSpeed;
        set => rotationSpeed = value;
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        targetForwardDirection = transform.forward;

        rotationSpeed = 0f;
    }

    void FixedUpdate()
    {
        if (targetForwardDirection != transform.forward)
        {
            rb.MoveRotation(Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(targetForwardDirection, transform.up), rotationSpeed * Time.deltaTime));
        }
    }
}

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

    void Update()
    {
        if (targetForwardDirection != transform.forward)
        {
            transform.localRotation = Quaternion.RotateTowards(transform.localRotation, Quaternion.LookRotation(Quaternion.Inverse(transform.parent.rotation) * targetForwardDirection, transform.up), rotationSpeed * Time.deltaTime);
        }
    }
}

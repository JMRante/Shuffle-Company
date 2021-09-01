using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float walkSpeed = 3f;
    
    private Vector3 velocity;
    private Vector3 closestSnapPoint;
    private bool hasInput;

    void Start()
    {
        velocity = Vector3.zero;
        closestSnapPoint = transform.position;
        hasInput = false;
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.UpArrow))
        {
            if (Utility.IsSnapped(transform.position))
            {
                hasInput = true;
                velocity = Vector3.forward * walkSpeed;
            }
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            if (Utility.IsSnapped(transform.position))
            {
                hasInput = true;
                velocity = Vector3.right * walkSpeed;
            }
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            if (Utility.IsSnapped(transform.position))
            {
                hasInput = true;
                velocity = Vector3.back * walkSpeed;
            }
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            if (Utility.IsSnapped(transform.position))
            {
                hasInput = true;
                velocity = Vector3.left * walkSpeed;
            }
        }
        else
        {
            if (hasInput)
            {
                closestSnapPoint = Utility.Round(transform.position);
                hasInput = false;
                velocity = Vector3.Normalize(closestSnapPoint - transform.position) * walkSpeed;
            }
        }
    }

    void FixedUpdate()
    {
        Rigidbody rb = GetComponent<Rigidbody>();

        if (hasInput)
        {
            rb.MovePosition(transform.position + velocity * Time.deltaTime);
        }
        else if (velocity != Vector3.zero)
        {
            Vector3 currentNorm = Vector3.Normalize(closestSnapPoint - transform.position);
            Vector3 overshotNorm = Vector3.Normalize(closestSnapPoint - (transform.position + (velocity * Time.deltaTime)));

            if (!Vector3.Equals(currentNorm, overshotNorm))
            {
                rb.MovePosition(closestSnapPoint);
                velocity = Vector3.zero;
            }
            else
            {
                rb.MovePosition(transform.position + velocity * Time.deltaTime);
            }
        }
    }
}

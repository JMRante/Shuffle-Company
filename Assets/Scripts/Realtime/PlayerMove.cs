using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float walkSpeed = 3f;
    
    private Vector3 velocity;
    private Vector3 direction;
    private Vector3 closestSnapPoint;

    void Start()
    {
        velocity = Vector3.zero;
        direction = Vector3.zero;
        closestSnapPoint = transform.position;
    }

    void Update()
    {
        bool inputUp = Input.GetKey(KeyCode.UpArrow);
        bool inputRight = Input.GetKey(KeyCode.RightArrow);
        bool inputDown = Input.GetKey(KeyCode.DownArrow);
        bool inputLeft = Input.GetKey(KeyCode.LeftArrow);

        if (inputUp)
        {
            direction = Vector3.forward;

            if (Utility.IsSnapped(transform.position))
            {
                velocity = Vector3.forward * walkSpeed;
            }
        }
        else if (inputRight)
        {
            direction = Vector3.right;

            if (Utility.IsSnapped(transform.position))
            {
                velocity = Vector3.right * walkSpeed;
            }
        }
        else if (inputDown)
        {
            direction = Vector3.back;

            if (Utility.IsSnapped(transform.position))
            {
                velocity = Vector3.back * walkSpeed;
            }
        }
        else if (inputLeft)
        {
            direction = Vector3.left;

            if (Utility.IsSnapped(transform.position))
            {
                velocity = Vector3.left * walkSpeed;
            }
        }

        if (!inputUp || !inputRight || !inputDown || !inputLeft)
        {
            if (direction != Vector3.zero)
            {
                Vector3 tempClosestSnapPoint = Utility.Round(transform.position);
                Vector3 moveToSnapPointVelocity = Vector3.Normalize(tempClosestSnapPoint - transform.position) * walkSpeed;

                if (moveToSnapPointVelocity.normalized == velocity.normalized)
                {
                    direction = Vector3.zero;
                    velocity = moveToSnapPointVelocity;
                    closestSnapPoint = tempClosestSnapPoint;
                }
            }
        }
    }

    void FixedUpdate()
    {
        Rigidbody rb = GetComponent<Rigidbody>();

        if (direction != Vector3.zero)
        {
            rb.MovePosition(transform.position + velocity * Time.deltaTime);
        }
        else if (velocity != Vector3.zero)
        {
            Vector3 currentNorm = Vector3.Normalize(closestSnapPoint - transform.position);
            Vector3 overshotNorm = Vector3.Normalize(closestSnapPoint - (transform.position + (velocity * Time.deltaTime)));

            if (currentNorm != overshotNorm)
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

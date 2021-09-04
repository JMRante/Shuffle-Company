using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float walkSpeed = 3.7f;
    
    private Vector3 velocity;
    private List<Vector3> inputDirections;
    private Rigidbody rb;

    void Start()
    {
        velocity = Vector3.zero;
        inputDirections = new List<Vector3>();
        rb = GetComponent<Rigidbody>();
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
            if (Utility.IsSnapped(transform.position))
            {
                if (inputDirections.Count > 0)
                {
                    Vector3 latestInputDirection = inputDirections[inputDirections.Count - 1];

                    if (!Physics.CheckSphere(transform.position + latestInputDirection, 0.49f))
                    {
                        velocity = latestInputDirection * walkSpeed;
                    }
                }
                else
                    velocity = Vector3.zero;
            }
        }
    }

    void FixedUpdate()
    {
        if (inputDirections.Count > 0)
        {
            Vector3 latestInputDirection = inputDirections[inputDirections.Count - 1];

            if (Physics.CheckSphere(transform.position + latestInputDirection, 0.49f) || velocity.normalized != inputDirections[inputDirections.Count - 1])
            {
                MoveRigidbodyTowardsSnapPointAhead();
            }
            else
            {
                rb.MovePosition(transform.position + velocity * Time.deltaTime);
            }
        }
        else
        {
            MoveRigidbodyTowardsSnapPointAhead();
        }
    }

    private void MoveRigidbodyTowardsSnapPointAhead()
    {
        Vector3 closestSnapPoint = Utility.Round(transform.position);
        Vector3 moveToSnapPointVelocity = Vector3.Normalize(closestSnapPoint - transform.position) * walkSpeed;

        if (moveToSnapPointVelocity.normalized == velocity.normalized)
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
                rb.MovePosition(transform.position + moveToSnapPointVelocity * Time.deltaTime);
            }
        }
        else
        {
            rb.MovePosition(transform.position + velocity * Time.deltaTime);
        }
    }
}

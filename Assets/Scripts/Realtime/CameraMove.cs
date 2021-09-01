using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    public Vector3 offsetFromPlayer = new Vector3(0f, 9f, -5f);

    private Rigidbody rb;
    private Rigidbody playerRigidbody = null;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        if (playerRigidbody != null)
        {
            rb.position = playerRigidbody.position + offsetFromPlayer;
        }
        else
        {
            playerRigidbody = GameObject.Find("Player").GetComponent<Rigidbody>();

            if (playerRigidbody != null)
            {
                rb.position = playerRigidbody.position + offsetFromPlayer;
                this.transform.LookAt(playerRigidbody.position, this.transform.up);
            }
        }
    }
}

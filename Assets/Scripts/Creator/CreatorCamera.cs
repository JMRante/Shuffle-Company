using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatorCamera : MonoBehaviour
{
    public float cameraSpeed = 0.03f;
    public float cameraSpeedFast = 0.12f;

    public Vector3 cameraStartPosition = Vector3.zero;
    public Vector3 cameraOffset = new Vector3(0f, 11f, -7f);

    void Start()
    {
        transform.position = cameraStartPosition + cameraOffset;
    }

    void Update()
    {
        float finalCameraSpeed = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift) ? cameraSpeedFast : cameraSpeed;
        Vector3 translationVector = Vector3.zero;

        if (Input.GetKey(KeyCode.W))
        {
            translationVector += Vector3.forward;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            translationVector += Vector3.back;
        }
        
        if (Input.GetKey(KeyCode.D))
        {
            translationVector += Vector3.right;
        }
        else if (Input.GetKey(KeyCode.A))
        {
            translationVector += Vector3.left;
        }

        if (Input.GetKey(KeyCode.Q))
        {
            translationVector += transform.forward;
        }
        else if (Input.GetKey(KeyCode.E))
        {
            translationVector -= transform.forward;
        }

        transform.position += translationVector.normalized * finalCameraSpeed;
    }
}

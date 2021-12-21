using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatorCursor : MonoBehaviour
{
    public Collider creatorGridCollider;
    private MeshRenderer creatorCursorRenderer;

    void Start()
    {
        creatorCursorRenderer = GetComponent<MeshRenderer>();
        creatorCursorRenderer.enabled = false;
    }

    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (creatorGridCollider.Raycast(ray, out hit, 500.0f))
        {
            creatorCursorRenderer.enabled = true;
            transform.position = Utility.Round(hit.point + (Vector3.up * 0.5f));
        }
        else
        {
            creatorCursorRenderer.enabled = false;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatorCursor : MonoBehaviour
{
    public GameObject creatorGrid;
    public GameObject creatorCamera;

    private Collider creatorGridCollider;
    private MeshRenderer creatorCursorRenderer;

    void Start()
    {
        creatorGridCollider = creatorGrid.GetComponent<Collider>();

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

        if (Input.mouseScrollDelta.y > 0 && creatorGrid.transform.position.y < StageChunks.TOTAL_CELL_HEIGHT - 2)
        {
            creatorGrid.transform.position += Vector3.up;
            creatorCamera.transform.position += Vector3.up;
        }
        else if (Input.mouseScrollDelta.y < 0 && creatorGrid.transform.position.y > 0f)
        {
            creatorGrid.transform.position += Vector3.down;
            creatorCamera.transform.position += Vector3.down;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatorCursor : MonoBehaviour
{
    public float lastModifiedCellCooldown = 2f;

    public GameObject creatorGrid;
    public GameObject creatorCamera;

    public StageChunks stageChunks;

    private Collider creatorGridCollider;
    private MeshRenderer creatorCursorRenderer;

    private float lastModifiedCellCooldownTimer = 0f;
    private Vector3 lastModifiedCellPosition;
    private int lastOperation = 0;

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

        if (transform.position != lastModifiedCellPosition || lastModifiedCellCooldownTimer == 0f || lastOperation != 1)
        {
            if (Input.GetMouseButton(0))
            {
                stageChunks.Draw(transform.position, new ChunkCell(1));
                lastModifiedCellCooldownTimer = lastModifiedCellCooldown;
                lastModifiedCellPosition = transform.position;
                lastOperation = 1;
            }
        }

        if (transform.position != lastModifiedCellPosition || lastModifiedCellCooldownTimer == 0f || lastOperation != 2)
        {
            if (Input.GetMouseButton(1))
            {
                stageChunks.Erase(transform.position);
                lastModifiedCellCooldownTimer = lastModifiedCellCooldown;
                lastModifiedCellPosition = transform.position;
                lastOperation = 2;
            }
        }

        lastModifiedCellCooldownTimer = Mathf.Clamp(lastModifiedCellCooldownTimer - Time.deltaTime, 0, lastModifiedCellCooldown);
    }
}

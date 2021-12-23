using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatorCursor : MonoBehaviour
{
    public float lastModifiedCellCooldown = 2f;

    public CreatorManager creatorManager;

    public GameObject creatorGrid;
    public GameObject creatorCamera;

    public StageChunks stageChunks;

    private Collider creatorGridCollider;
    private MeshRenderer creatorCursorRenderer;

    private float lastModifiedCellCooldownTimer = 0f;
    private Vector3 lastModifiedCellPosition;
    private CreatorOperationType lastOperation = CreatorOperationType.None;

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

        if (transform.position != lastModifiedCellPosition || lastModifiedCellCooldownTimer == 0f || lastOperation != CreatorOperationType.Point)
        {
            if (Input.GetMouseButton(0))
            {
                creatorManager.DoOperation(new DrawPoint(transform.position, new ChunkCell(1), stageChunks, false));

                lastModifiedCellCooldownTimer = lastModifiedCellCooldown;
                lastModifiedCellPosition = transform.position;
                lastOperation = CreatorOperationType.Point;
            }
        }

        if (transform.position != lastModifiedCellPosition || lastModifiedCellCooldownTimer == 0f || lastOperation != CreatorOperationType.ErasePoint)
        {
            if (Input.GetMouseButton(1))
            {
                creatorManager.DoOperation(new DrawPoint(transform.position, new ChunkCell(0), stageChunks, true));

                lastModifiedCellCooldownTimer = lastModifiedCellCooldown;
                lastModifiedCellPosition = transform.position;
                lastOperation = CreatorOperationType.ErasePoint;
            }
        }

        lastModifiedCellCooldownTimer = Mathf.Clamp(lastModifiedCellCooldownTimer - Time.deltaTime, 0, lastModifiedCellCooldown);
    }
}

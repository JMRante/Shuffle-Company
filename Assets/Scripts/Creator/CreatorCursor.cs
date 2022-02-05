using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CreatorOperationType
{
    None,
    Point,
    BoxPivotPlace,
    Box,
    SelectPivotPlace,
    Select,
    Paste
}

public class CreatorCursor : MonoBehaviour
{
    public float lastModifiedCellCooldown = 2f;

    public CreatorManager creatorManager;

    public GameObject creatorGrid;
    public GameObject creatorCamera;
    public GameObject creatorCursorPivot;

    public StageChunks stageChunks;

    public ChunkCell brush;
    public ChunkCell eraseBrush;

    private Collider creatorGridCollider;
    private MeshRenderer creatorCursorRenderer;

    private float lastModifiedCellCooldownTimer;
    private Vector3 lastModifiedCellPosition;
    private bool wasLastOperationErase;
    private CreatorOperationType operationMode;

    void Start()
    {
        brush = new ChunkCell(1, 0);
        eraseBrush = new ChunkCell(0, 0);

        creatorGridCollider = creatorGrid.GetComponent<Collider>();

        creatorCursorRenderer = GetComponent<MeshRenderer>();
        creatorCursorRenderer.enabled = false;

        lastModifiedCellCooldownTimer = 0f;
        wasLastOperationErase = false;
        operationMode = CreatorOperationType.Point;
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

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            operationMode = CreatorOperationType.Point;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            operationMode = CreatorOperationType.BoxPivotPlace;
        }

        if (Input.GetKeyDown(KeyCode.I)) 
        {
            if (!brush.IsInsetBack())
            {
                brush.SetInsetBack(true);
            }
            else
            {
                brush.SetInsetBack(false);
            }
        }

        if (transform.position != lastModifiedCellPosition || lastModifiedCellCooldownTimer == 0f || wasLastOperationErase)
        {
            if (Input.GetMouseButton(0))
            {
                switch(operationMode)
                {
                    case CreatorOperationType.Point:
                    {
                        creatorManager.DoOperation(new DrawPoint(transform.position, brush, stageChunks));

                        lastModifiedCellCooldownTimer = lastModifiedCellCooldown;
                        lastModifiedCellPosition = transform.position;
                        wasLastOperationErase = false;

                        break;
                    }
                    case CreatorOperationType.BoxPivotPlace:
                    {
                        creatorCursorPivot.SetActive(true);
                        creatorCursorPivot.transform.position = transform.position;
                        operationMode = CreatorOperationType.Box;

                        lastModifiedCellCooldownTimer = lastModifiedCellCooldown;
                        lastModifiedCellPosition = transform.position;
                        wasLastOperationErase = false;

                        break;
                    }
                    case CreatorOperationType.Box:
                    {
                        creatorManager.DoOperation(new DrawBox(creatorCursorPivot.transform.position, transform.position, brush, stageChunks));

                        creatorCursorPivot.SetActive(false);
                        operationMode = CreatorOperationType.BoxPivotPlace;

                        lastModifiedCellCooldownTimer = lastModifiedCellCooldown;
                        lastModifiedCellPosition = transform.position;
                        wasLastOperationErase = false;

                        break;
                    }
                    default: break;
                }
            }
        }

        if (transform.position != lastModifiedCellPosition || lastModifiedCellCooldownTimer == 0f || !wasLastOperationErase)
        {
            if (Input.GetMouseButton(1))
            {
                switch (operationMode)
                {
                    case CreatorOperationType.Point:
                        {
                            creatorManager.DoOperation(new DrawPoint(transform.position, eraseBrush, stageChunks));

                            lastModifiedCellCooldownTimer = lastModifiedCellCooldown;
                            lastModifiedCellPosition = transform.position;
                            wasLastOperationErase = true;

                            break;
                        }
                    case CreatorOperationType.BoxPivotPlace:
                        {
                            creatorCursorPivot.SetActive(true);
                            creatorCursorPivot.transform.position = transform.position;
                            operationMode = CreatorOperationType.Box;

                            lastModifiedCellCooldownTimer = lastModifiedCellCooldown;
                            lastModifiedCellPosition = transform.position;
                            wasLastOperationErase = true;

                            break;
                        }
                    case CreatorOperationType.Box:
                        {
                            creatorManager.DoOperation(new DrawBox(creatorCursorPivot.transform.position, transform.position, eraseBrush, stageChunks));

                            creatorCursorPivot.SetActive(false);
                            operationMode = CreatorOperationType.BoxPivotPlace;

                            lastModifiedCellCooldownTimer = lastModifiedCellCooldown;
                            lastModifiedCellPosition = transform.position;
                            wasLastOperationErase = true;

                            break;
                        }
                    default: break;
                }
            }
        }

        lastModifiedCellCooldownTimer = Mathf.Clamp(lastModifiedCellCooldownTimer - Time.deltaTime, 0, lastModifiedCellCooldown);
    }
}

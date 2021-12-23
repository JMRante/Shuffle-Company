using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatorManager : MonoBehaviour
{
    public float undoCooldown = 0.1f;
    public float redoCooldown = 0.1f;

    private float undoCooldownTimer = 0f;
    private float redoCooldownTimer = 0f;

    public int operationMemorySize = 512;

    private LinkedList<CreatorOperation> operationMemory;
    private LinkedList<CreatorOperation> operationRedoMemory;

    public void DoOperation(CreatorOperation op)
    {
        op.operate();

        operationMemory.AddLast(new LinkedListNode<CreatorOperation>(op));

        if (operationMemory.Count > operationMemorySize)
        {
            operationMemory.RemoveFirst();
        }

        operationRedoMemory.Clear();
    }

    public void UndoOperation()
    {
        if (operationMemory.Count > 0)
        {
            CreatorOperation lastOperation = operationMemory.Last.Value;
            operationMemory.RemoveLast();
            lastOperation.reverse();
            operationRedoMemory.AddLast(new LinkedListNode<CreatorOperation>(lastOperation));
        }
    }

    public void RedoOperation()
    {
        if (operationRedoMemory.Count > 0)
        {
            CreatorOperation nextRedoOperation = operationRedoMemory.Last.Value;
            operationRedoMemory.RemoveLast();
            nextRedoOperation.operate();
            operationMemory.AddLast(new LinkedListNode<CreatorOperation>(nextRedoOperation));
        }
    }

    public void Clear()
    {
        operationMemory.Clear();
        operationRedoMemory.Clear();
    }

    void Start()
    {
        operationMemory = new LinkedList<CreatorOperation>();
        operationRedoMemory = new LinkedList<CreatorOperation>();
    }

    void Update()
    {
        // Undo Command
        if (Input.GetKey(KeyCode.Z) && undoCooldownTimer == 0f)
        {
            UndoOperation();
            undoCooldownTimer = undoCooldown;
        }

        // Redo Command
        if (Input.GetKey(KeyCode.Y) && redoCooldownTimer == 0f)
        {
            RedoOperation();
            redoCooldownTimer = redoCooldown;
        }

        undoCooldownTimer = Mathf.Clamp(undoCooldownTimer - Time.deltaTime, 0, undoCooldown);
        redoCooldownTimer = Mathf.Clamp(redoCooldownTimer - Time.deltaTime, 0, redoCooldown);
    }
}

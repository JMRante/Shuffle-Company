using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    private Stack<Move> moves;
    private List<GameObject> dynamicElements;
    private GameObject player;

    private float moveExecutionTime = 0.2f;
    private float moveExecutionTimer = 0f;
    private bool isUndo = false;

    void Start() 
    {
        moves = new Stack<Move>();
        dynamicElements = new List<GameObject>();

        GetElementsToTrack();
    }

    void Update() 
    {
        if (player != null)
        {
            if (moveExecutionTimer == 0f)
            {
                // Check Player Input for behavior requests
                IBehavior playerRequestedBehavior = null;
                bool canPlayerMove = CanPlayerMove();

                if (canPlayerMove && Input.GetKey(KeyCode.UpArrow))
                {
                    playerRequestedBehavior = new PlayerMoveBehavior(player, Vector3.forward);
                }
                else if (canPlayerMove && Input.GetKey(KeyCode.RightArrow))
                {
                    playerRequestedBehavior = new PlayerMoveBehavior(player, Vector3.right);
                }
                else if (canPlayerMove && Input.GetKey(KeyCode.DownArrow))
                {
                    playerRequestedBehavior = new PlayerMoveBehavior(player, Vector3.back);
                }
                else if (canPlayerMove && Input.GetKey(KeyCode.LeftArrow))
                {
                    playerRequestedBehavior = new PlayerMoveBehavior(player, Vector3.left);
                }
                else if (canPlayerMove && Input.GetKey(KeyCode.X))
                {
                    playerRequestedBehavior = new DoNothingBehavior();
                }
                else if (Input.GetKey(KeyCode.Z))
                {
                    if (moves.Count > 0) 
                    {
                        moveExecutionTimer = moveExecutionTime;
                        isUndo = true;
                    }
                }
                else if (!canPlayerMove)
                {
                    playerRequestedBehavior = new DoNothingBehavior();
                }

                // If player behavior queued, attempt it to go to next move
                if (playerRequestedBehavior != null)
                {
                    // Check dynamic objects for state changes
                    List<IBehavior> requestedBehaviors = new List<IBehavior>();
                    requestedBehaviors.Add(playerRequestedBehavior);

                    foreach (GameObject dynamicElement in dynamicElements)
                    {
                        IBehavior[] behaviors = dynamicElement.GetComponents<IBehavior>();
                        SortBehaviors(behaviors);

                        foreach (IBehavior behavior in behaviors)
                        {
                            requestedBehaviors.Add(behavior);
                        }
                    }

                    Move nextMove = CalculateMove(requestedBehaviors);
                    moves.Push(nextMove);

                    moveExecutionTimer = moveExecutionTime;
                    isUndo = false;
                }
            }
            else
            {
                moveExecutionTimer -= Time.deltaTime;
                float scaledMoveExecutionTimer = (1f / moveExecutionTime) * moveExecutionTimer;

                if (isUndo) 
                {
                    moves.Peek().Render(scaledMoveExecutionTimer);

                    if (moveExecutionTimer <= 0) 
                    {
                        moves.Pop().Undo();
                        SortDynamicElements();
                        moveExecutionTimer = 0f;
                    }
                }
                else
                {
                    moves.Peek().Render(1 - scaledMoveExecutionTimer);

                    if (moveExecutionTimer <= 0)
                    {
                        moves.Peek().Do();
                        SortDynamicElements();
                        moveExecutionTimer = 0f;
                    }
                }
            }
        }
    }

    public bool CanPlayerMove()
    {
        IBehavior[] behaviors = player.GetComponents<IBehavior>();

        foreach (IBehavior behavior in behaviors)
        {
            if (behavior.GetStateChanges() != null)
            {
                return false;
            }
        }

        Vector3[] directions = new Vector3[] { Vector3.forward, Vector3.right, Vector3.back, Vector3.left };

        foreach (Vector3 direction in directions)
        {
            PlayerMoveBehavior potentialMove = new PlayerMoveBehavior(player, direction);

            if (potentialMove.GetStateChanges() != null)
            {
                return true;
            }            
        }

        return false;
    }

    public void SortDynamicElements()
    {
        dynamicElements.Sort(delegate (GameObject obj1, GameObject obj2) 
        {
            Element obj1Element = obj1.GetComponent<Element>();
            Element obj2Element = obj2.GetComponent<Element>();

            int elementComparison = obj1Element.sortOrder.CompareTo(obj2Element.sortOrder);

            if (elementComparison != 0) {
                return elementComparison;
            }

            Vector3 obj1Position = obj1.transform.position;
            Vector3 obj2Position = obj2.transform.position;

            if (obj1Position.y.CompareTo(obj2Position.y) != 0)
            {
                return obj1Position.y.CompareTo(obj2Position.y);
            }
            else if (obj1Position.z.CompareTo(obj2Position.z) != 0)
            {
                return obj1Position.z.CompareTo(obj2Position.z);
            }
            else
            {
                return obj1Position.x.CompareTo(obj2Position.x);
            }
        });
    }

    public void SortBehaviors(IBehavior[] behaviors)
    {
        Array.Sort(behaviors, delegate (IBehavior b1, IBehavior b2) 
            {
                int priorityCompare = b1.GetPriority().CompareTo(b2.GetPriority());

                if (priorityCompare != 0)
                {
                    return priorityCompare;
                }

                return b1.GetType().ToString().CompareTo(b2.GetType().ToString());
            }
        );
    }

    public void GetElementsToTrack()
    {
        player = GameObject.Find("Player");
        GameObject[] dynamicElementArray = GameObject.FindGameObjectsWithTag("DynamicElement");

        foreach (GameObject dynamicElement in dynamicElementArray) 
        {
            dynamicElements.Add(dynamicElement);
        }

        SortDynamicElements();
    }

    public Move CalculateMove(List<IBehavior> behaviors)
    {
        Move move = new Move();

        foreach (IBehavior behavior in behaviors)
        {
            List<StateChange> behaviorStateChanges = behavior.GetStateChanges();
            
            if (behaviorStateChanges != null)
            {
                move.AddStateChanges(behaviorStateChanges);
            }
        }

        return move;
    }
}

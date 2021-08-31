using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    private Stack<Move> moves;
    private List<GameObject> dynamicElements;
    private GameObject player;
    private GridCollisionSystem gcs;

    private float moveExecutionTime = 0.2f;
    private float moveExecutionTimer = 0f;
    private bool isUndo = false;

    void Start() 
    {
        moves = new Stack<Move>();
        dynamicElements = new List<GameObject>();
        gcs = GetComponent<GridCollisionSystem>();

        HashAllElements();
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
                    PlayerMoveBehavior tempPlayerRequestedBehavior = player.GetComponent<PlayerMoveBehavior>();
                    tempPlayerRequestedBehavior.SetDirection(Vector3.forward);
                    playerRequestedBehavior = tempPlayerRequestedBehavior;
                }
                else if (canPlayerMove && Input.GetKey(KeyCode.RightArrow))
                {
                    PlayerMoveBehavior tempPlayerRequestedBehavior = player.GetComponent<PlayerMoveBehavior>();
                    tempPlayerRequestedBehavior.SetDirection(Vector3.right);
                    playerRequestedBehavior = tempPlayerRequestedBehavior;
                }
                else if (canPlayerMove && Input.GetKey(KeyCode.DownArrow))
                {
                    PlayerMoveBehavior tempPlayerRequestedBehavior = player.GetComponent<PlayerMoveBehavior>();
                    tempPlayerRequestedBehavior.SetDirection(Vector3.back);
                    playerRequestedBehavior = tempPlayerRequestedBehavior;
                }
                else if (canPlayerMove && Input.GetKey(KeyCode.LeftArrow))
                {
                    PlayerMoveBehavior tempPlayerRequestedBehavior = player.GetComponent<PlayerMoveBehavior>();
                    tempPlayerRequestedBehavior.SetDirection(Vector3.left);
                    playerRequestedBehavior = tempPlayerRequestedBehavior;
                }
                else if (canPlayerMove && Input.GetKey(KeyCode.X))
                {
                    playerRequestedBehavior = player.GetComponent<DoNothingBehavior>();
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
                    playerRequestedBehavior = player.GetComponent<DoNothingBehavior>();
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
                            if (!behavior.IsPassive())
                            {
                                requestedBehaviors.Add(behavior);
                            }
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
            if (!behavior.IsPassive() && behavior.GetStateChanges() != null)
            {
                return false;
            }
        }

        Vector3[] directions = new Vector3[] { Vector3.forward, Vector3.right, Vector3.back, Vector3.left };

        foreach (Vector3 direction in directions)
        {
            PlayerMoveBehavior potentialMove = player.GetComponent<PlayerMoveBehavior>();
            potentialMove.SetDirection(direction);

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

    public void HashAllElements()
    {
        GameObject[] staticElementArray = GameObject.FindGameObjectsWithTag("StaticElement");

        foreach (GameObject staticElement in staticElementArray)
        {
            gcs.Hash(staticElement, Vector3.negativeInfinity, staticElement.transform.position);
        }

        GameObject[] dynamicElementArray = GameObject.FindGameObjectsWithTag("DynamicElement");

        foreach (GameObject dynamicElement in dynamicElementArray)
        {
            gcs.Hash(dynamicElement, Vector3.negativeInfinity, dynamicElement.transform.position);
        }
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
        // Debug.Log("Calculate Move");

        foreach (IBehavior behavior in behaviors)
        {
            // Debug.Log("Behavior: " + behavior.ToString());
            List<StateChange> behaviorStateChanges = behavior.GetStateChanges();
            
            if (behaviorStateChanges != null)
            {
                foreach (StateChange behaviorStateChange in behaviorStateChanges)
                {
                    // Debug.Log("SC: " + behaviorStateChange.ToString());
                    if (!move.IsInStateChangeRecords(behaviorStateChange))
                    {
                        behaviorStateChange.Do();
                        move.AddStateChange(behaviorStateChange);
                    }
                    // PrintDynamicElementStatus();
                }
            }
        }

        move.Undo();

        return move;
    }

    public void PrintDynamicElementStatus()
    {
        foreach (GameObject go in dynamicElements)
        {
            Debug.Log("DO " + go.name + " is " + go.transform.position);
        }
    }
}

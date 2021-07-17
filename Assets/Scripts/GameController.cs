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
                // Check Player Input for State Changes
                StateChange playerRequestStateChange = null;
                bool canPlayerMove = CanPlayerMove();

                if (canPlayerMove && Input.GetKey(KeyCode.UpArrow))
                {
                    playerRequestStateChange = new TranslateStateChange(player, Vector3.forward);
                }
                else if (canPlayerMove && Input.GetKey(KeyCode.RightArrow))
                {
                    playerRequestStateChange = new TranslateStateChange(player, Vector3.right);
                }
                else if (canPlayerMove && Input.GetKey(KeyCode.DownArrow))
                {
                    playerRequestStateChange = new TranslateStateChange(player, Vector3.back);
                }
                else if (canPlayerMove && Input.GetKey(KeyCode.LeftArrow))
                {
                    playerRequestStateChange = new TranslateStateChange(player, Vector3.left);
                }
                else if (canPlayerMove && Input.GetKey(KeyCode.X))
                {
                    playerRequestStateChange = new VoidStateChange();
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
                    playerRequestStateChange = new VoidStateChange();
                }

                // If state changes are queued, attempt them
                if (playerRequestStateChange != null)
                {
                    // Check dynamic objects for state changes
                    List<StateChange> requestStateChanges = new List<StateChange>();

                    foreach (GameObject dynamicElement in dynamicElements)
                    {
                        IBehavior[] behaviors = dynamicElement.GetComponents<IBehavior>();

                        foreach (IBehavior behavior in behaviors)
                        {
                            List<StateChange> stateChanges = behavior.CheckForStateChanges();

                            if (stateChanges != null)
                            {
                                requestStateChanges.AddRange(stateChanges);
                            }
                        }
                    }

                    Move nextMove = new Move();
                    bool requestPossible = nextMove.RequestStateChange(playerRequestStateChange);

                    if (requestPossible)
                    {
                        foreach (StateChange requestStateChange in requestStateChanges)
                        {
                            nextMove.RequestStateChange(requestStateChange);
                        }

                        moves.Push(nextMove);

                        moveExecutionTimer = moveExecutionTime;
                        isUndo = false;
                    }
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
            List<StateChange> stateChanges = behavior.CheckForStateChanges();

            if (stateChanges != null)
            {
                return false;
            }
        }

        return true;
    }

    public void SortDynamicElements()
    {
        dynamicElements.Sort(delegate (GameObject objA, GameObject objB) 
        {
            Vector3 objAPosition = objA.transform.position;
            Vector3 objBPosition = objB.transform.position;

            if (objAPosition.y.CompareTo(objBPosition.y) != 0)
            {
                return objAPosition.y.CompareTo(objBPosition.y);
            }
            else if (objAPosition.z.CompareTo(objBPosition.z) != 0)
            {
                return objAPosition.z.CompareTo(objBPosition.z);
            }
            else
            {
                return objAPosition.x.CompareTo(objBPosition.x);
            }
        });
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
}

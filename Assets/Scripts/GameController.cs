using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    private Stack<Move> moves;
    private GameObject player;

    private float moveExecutionTime = 0.2f;
    private float moveExecutionTimer = 0f;
    private bool isUndo = false;

    void Start() 
    {
        moves = new Stack<Move>();
    }

    void Update() 
    {
        if (player != null)
        {
            if (moveExecutionTimer == 0f)
            {
                IStateChange playerAction = null;

                if (Input.GetKey(KeyCode.UpArrow))
                {
                    playerAction = new TranslateStateChange(player, Vector3.forward);
                }
                else if (Input.GetKey(KeyCode.RightArrow))
                {
                    playerAction = new TranslateStateChange(player, Vector3.right);
                }
                else if (Input.GetKey(KeyCode.DownArrow))
                {
                    playerAction = new TranslateStateChange(player, Vector3.back);
                }
                else if (Input.GetKey(KeyCode.LeftArrow))
                {
                    playerAction = new TranslateStateChange(player, Vector3.left);
                } 
                else if (Input.GetKey(KeyCode.Z))
                {
                    if (moves.Count > 0) 
                    {
                        moveExecutionTimer = moveExecutionTime;
                        isUndo = true;
                    }
                }

                if (playerAction != null)
                {
                    Move nextMove = new Move();
                    bool requestPossible = nextMove.RequestStateChange(playerAction);

                    if (requestPossible)
                    {
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
                        moveExecutionTimer = 0f;
                    }
                }
                else
                {
                    moves.Peek().Render(1 - scaledMoveExecutionTimer);

                    if (moveExecutionTimer <= 0)
                    {
                        moves.Peek().Do();
                        moveExecutionTimer = 0f;
                    }
                }
            }
        } 
        else
        {
            player = GameObject.Find("Player");
        }
    }
}

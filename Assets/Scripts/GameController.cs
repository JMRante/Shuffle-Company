using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    private Stack<Move> moves;
    private GameObject player;

    void Start() 
    {
        moves = new Stack<Move>();
    }

    void Update() 
    {
        if (player != null)
        {
            IAction playerAction = null;

            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                playerAction = new TranslateAction(player, new Vector3(0, 0, 1));
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                playerAction = new TranslateAction(player, new Vector3(1, 0, 0));
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                playerAction = new TranslateAction(player, new Vector3(0, 0, -1));
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                playerAction = new TranslateAction(player, new Vector3(-1, 0, 0));
            } 
            else if (Input.GetKeyDown(KeyCode.Z))
            {
                if (moves.Count > 0) 
                {
                    Move undoMove = moves.Pop();
                    undoMove.Undo();
                }
            }

            if (playerAction != null)
            {
                Move nextMove = new Move();
                nextMove.GetActions().Add(playerAction);
                nextMove.Do();
                moves.Push(nextMove);
            }
        } 
        else
        {
            player = GameObject.Find("Player");
        }
    }
}

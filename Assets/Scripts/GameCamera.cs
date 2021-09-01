using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCamera : MonoBehaviour
{
    public Vector3 offsetFromPlayer = new Vector3(0f, 9f, -5f);
    private GameObject player = null;

    void Update() 
    {
        if (player != null) 
        {
            this.transform.position = player.transform.position + offsetFromPlayer;
        } 
        else 
        {
            player = GameObject.Find("Player");

            if (player != null)
            {
                this.transform.position = player.transform.position + offsetFromPlayer;
                this.transform.LookAt(player.transform.position, this.transform.up);
            }
        }
    }
}

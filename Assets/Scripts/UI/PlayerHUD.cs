using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHUD : MonoBehaviour
{
    private StateVariables playerStateVariables;

    public Text redKeyCount;
    public Text yellowKeyCount;
    public Text greenKeyCount;
    public Text blueKeyCount;

    void Start()
    {
        playerStateVariables = GameObject.Find("Player").GetComponent<StateVariables>();
    }

    void Update()
    {
        redKeyCount.text = playerStateVariables.GetInt("redKeys").ToString();
        yellowKeyCount.text = playerStateVariables.GetInt("yellowKeys").ToString();
        greenKeyCount.text = playerStateVariables.GetInt("greenKeys").ToString();
        blueKeyCount.text = playerStateVariables.GetInt("blueKeys").ToString();
    }
}

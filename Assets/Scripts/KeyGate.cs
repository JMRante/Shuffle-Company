using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyGate : MonoBehaviour
{
    public string keyMatch;
    private bool isUnlocked;

    public bool IsUnlocked
    {
        get => isUnlocked;
        set => isUnlocked = value;
    }

    void Start()
    {
        isUnlocked = false;
    }

    public void Unlock()
    {
        isUnlocked = true;
        GameObject.Destroy(gameObject);
    }
}

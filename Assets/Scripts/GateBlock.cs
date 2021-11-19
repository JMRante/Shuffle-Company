using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateBlock : MonoBehaviour
{
    public GameObject toggleToGameObject;

    public void Toggle()
    {
        Instantiate(toggleToGameObject, transform.position, Quaternion.identity, transform.parent);
        Destroy(gameObject);
    }
}

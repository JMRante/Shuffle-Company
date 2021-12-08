using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour
{
    public string collectableName;
    public int collectableValue;
    public GameObject collectableDestroy;

    public void CollectDestroy()
    {
        if (collectableDestroy != null)
        {
            Instantiate(collectableDestroy, transform.position, transform.rotation, transform.parent);
        }

        GameObject.Destroy(gameObject);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridCollisionSystem : MonoBehaviour
{
    private Dictionary<Vector3Int, List<GameObject>> collisionHash;

    void Awake()
    {
        collisionHash = new Dictionary<Vector3Int, List<GameObject>>();
    }

    // Main Functions
    public void Hash(GameObject gameObject, Vector3 previousPosition, Vector3 nextPosition)
    {
        Vector3Int previousPositionInt = Vector3Int.FloorToInt(previousPosition);
        Vector3Int nextPositionInt = Vector3Int.FloorToInt(nextPosition);

        // Remove hash from previous position if it exists
        if (previousPosition != Vector3.negativeInfinity)
        {
            Vector3Int[] previousTransformHashList = GetTransformHashList(gameObject, previousPositionInt);

            for (int i = 0; i < previousTransformHashList.Length; i++)
            {
                if (collisionHash.ContainsKey(previousTransformHashList[i]))
                {
                    List<GameObject> collisionHashList = collisionHash[previousTransformHashList[i]];
                    collisionHashList.Remove(gameObject);
                    // Debug.Log("Remove from Hash: " + gameObject + ", " + previousPositionInt + ", " + nextPositionInt);
                }
            }
        }

        Vector3Int[] nextTransformHashList = GetTransformHashList(gameObject, nextPositionInt);

        // Add hash to next position if it exists
        if (nextPosition != Vector3.negativeInfinity)
        {
            for (int i = 0; i < nextTransformHashList.Length; i++)
            {
                if (collisionHash.ContainsKey(nextTransformHashList[i]))
                {
                    List<GameObject> collisionHashList = collisionHash[nextTransformHashList[i]];

                    if (!collisionHashList.Contains(gameObject))
                    {
                        collisionHashList.Add(gameObject);
                        // Debug.Log("Add to Hash: " + gameObject + ", " + previousPositionInt + ", " + nextPositionInt);
                    }
                }
                else
                {
                    List<GameObject> collisionHashList = new List<GameObject>();
                    collisionHashList.Add(gameObject);
                    collisionHash.Add(nextTransformHashList[i], collisionHashList);
                    // Debug.Log("Add to Hash: " + gameObject + ", " + previousPositionInt + ", " + nextPositionInt);
                }
            }
        }
    }

    private Vector3Int[] GetTransformHashList(GameObject gameObject, Vector3Int position)
    {
        Collider[] collisionBoxes = gameObject.GetComponentsInChildren<Collider>();
        Vector3Int[] transformHashList = new Vector3Int[collisionBoxes.Length];

        for (int i = 0; i < collisionBoxes.Length; i++)
        {
            Vector3Int localPosition = Vector3Int.FloorToInt(collisionBoxes[i].transform.localPosition);
            transformHashList[i] = position + localPosition;
        }

        return transformHashList;
    }


    // Query fuctions
    public GameObject FirstElementAtIndex(Vector3Int index, params ElementProperty[] elementProperties)
    {
        if (collisionHash.ContainsKey(index))
        {
            List<GameObject> elementsAtIndex = collisionHash[index];

            for (int i = 0; i < elementsAtIndex.Count; i++)
            {
                Element element = elementsAtIndex[i].GetComponent<Element>();

                if (element != null && element.isActiveAndEnabled && (AreAllElementsAccepted(elementProperties) || element.HasAnyProperty(elementProperties)))
                {
                    return element.gameObject;
                }
            }
        }

        return null;
    }

    public GameObject FirstElementAtIndex(Vector3 index, params ElementProperty[] elementProperties)
    {
        Vector3Int intIndex = Vector3Int.FloorToInt(index);
        return FirstElementAtIndex(intIndex, elementProperties);
    }

    public List<GameObject> AllElementsAtIndex(Vector3Int index, params ElementProperty[] elementProperties)
    {
        List<GameObject> elementList = new List<GameObject>();

        if (collisionHash.ContainsKey(index))
        {
            List<GameObject> elementsAtIndex = collisionHash[index];

            for (int i = 0; i < elementsAtIndex.Count; i++)
            {
                Element element = elementsAtIndex[i].GetComponent<Element>();

                if (element != null && element.isActiveAndEnabled && (AreAllElementsAccepted(elementProperties) || element.HasAnyProperty(elementProperties)))
                {
                    elementList.Add(element.gameObject);
                }
            }
        }

        return elementList;
    }

    public List<GameObject> AllElementsAtIndex(Vector3 index, params ElementProperty[] elementProperties)
    {
        Vector3Int intIndex = Vector3Int.FloorToInt(index);
        return AllElementsAtIndex(intIndex, elementProperties);
    }

    public bool ElementExistsAtIndex(Vector3Int index, params ElementProperty[] elementProperties)
    {
        if (collisionHash.ContainsKey(index))
        {
            List<GameObject> elementsAtIndex = collisionHash[index];

            for (int i = 0; i < elementsAtIndex.Count; i++)
            {
                Element element = elementsAtIndex[i].GetComponent<Element>();

                if (element != null && element.isActiveAndEnabled && (AreAllElementsAccepted(elementProperties) || element.HasAnyProperty(elementProperties)))
                {
                    return true;
                }
            }
        }

        return false;
    }

    public bool ElementExistsAtIndex(Vector3 index, params ElementProperty[] elementProperties)
    {
        Vector3Int intIndex = Vector3Int.FloorToInt(index);
        return ElementExistsAtIndex(intIndex, elementProperties);
    }

    public bool ElementHasProperty(GameObject gameObject, ElementProperty elementProperty)
    {
        Element element = gameObject.GetComponent<Element>();

        if (element != null && element.isActiveAndEnabled && element.HasProperty(elementProperty))
        {
            return true;
        }

        return false;
    }

    public bool AreAllElementsAccepted(params ElementProperty[] elementProperties)
    {
        foreach (ElementProperty ep in elementProperties)
        {
            if (ep == ElementProperty.All)
            {
                return true;
            }
        }

        return false;
    }

    // public int GetVariableAtIndex(Vector3 index, string key)
    // {

    // }

    // public int GetVariableAtIndexWithProperty(Vector3 index, string key, ElementProperty elementProperty)
    // {

    // }

    // public void SetVariableAtIndex(Vector3 index, string key, int value)
    // {

    // }

    // public void SetVariableAtIndexWithProperty(Vector3 index, string key, int value, ElementProperty elementProperty)
    // {

    // }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageGenerator : MonoBehaviour
{
    public GameObject[] elements;
    public GameController gameController;

    public int width = 10;
    public int depth = 10;

    // private int[] stage = new int[] {
    //     1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
    //     1, 0, 0, 0, 1, 1, 1, 1, 0, 1,
    //     1, 0, 1, 1, 1, 1, 0, 0, 0, 1,
    //     1, 0, 0, 1, 1, 0, 0, 1, 0, 1,
    //     1, 0, 0, 0, 0, 3, 0, 3, 0, 1,
    //     1, 0, 0, 1, 1, 1, 0, 1, 0, 1,
    //     1, 0, 1, 0, 0, 0, 0, 1, 0, 1,
    //     1, 0, 1, 1, 1, 0, 2, 1, 0, 1,
    //     1, 0, 0, 0, 0, 0, 0, 3, 0, 1,
    //     1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
    // };
    private int[] stage = new int[] {};

    void Start() 
    {
        for (int i = 0; i < stage.Length; i++) 
        {
            Vector3 position = new Vector3(i % width, 0f, i / depth);

            if (stage[i] > 0) 
            {
                GameObject element = elements[stage[i] - 1];
                GameObject elementInstance = Instantiate(element, position, Quaternion.identity, this.transform);

                elementInstance.name = elementInstance.name.Replace("(Clone)", "");
            }
        }

        gameController.GetElementsToTrack();
    }
}

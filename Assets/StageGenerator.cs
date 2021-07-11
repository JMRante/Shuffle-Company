using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageGenerator : MonoBehaviour
{
    public GameObject[] elements;

    private int width = 10;
    private int height = 10;

    private int[] stage = new int[] {
        1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
        1, 0, 0, 0, 1, 1, 1, 1, 0, 1,
        1, 0, 1, 1, 1, 1, 0, 0, 0, 1,
        1, 0, 0, 1, 1, 0, 0, 1, 0, 1,
        1, 0, 0, 0, 0, 0, 0, 0, 0, 1,
        1, 0, 0, 1, 1, 1, 0, 1, 0, 1,
        1, 0, 1, 0, 0, 0, 0, 1, 0, 1,
        1, 0, 1, 1, 1, 0, 0, 1, 0, 1,
        1, 0, 0, 0, 0, 0, 0, 0, 0, 1,
        1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
    };

    void Start() {
        for (int i = 0; i < stage.Length; i++) {
            Vector3 position = new Vector3(i % width, 0f, i / height);

            if (stage[i] > 0) {
                GameObject element = elements[stage[i] - 1];
                Instantiate(element, position, Quaternion.identity);
            }
        }
    }
}

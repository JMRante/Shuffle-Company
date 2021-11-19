using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ButtonType
{
    oneTime,
    switcher,
    hold
}

public class Button : MonoBehaviour
{
    public ButtonType buttonType;

    public GameObject normalModel;
    public GameObject pressedModel;

    private bool isPressed;
    private bool lastPressState;
    private Sensor sensor;

    void Start()
    {
        isPressed = false;
        lastPressState = false;
        sensor = GetComponentInChildren<Sensor>();
    }

    void Update()
    {
        bool isPressed = sensor.DoesRayContainElementProperty(Vector3.up, ElementProperty.Loose);

        if (isPressed && !lastPressState)
        {
            normalModel.SetActive(false);
            pressedModel.SetActive(true);
            lastPressState = true;

            transform.parent.BroadcastMessage("Toggle");
        }
        else if (!isPressed && lastPressState && buttonType != ButtonType.oneTime)
        {
            normalModel.SetActive(true);
            pressedModel.SetActive(false);
            lastPressState = false;

            if (buttonType == ButtonType.hold)
            {
                transform.parent.BroadcastMessage("Toggle");
            }
        }
    }
}

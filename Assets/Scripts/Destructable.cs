using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Destructable : MonoBehaviour
{
    public int strength;
    public GameObject destroyedForm;

    private bool isDestructionDetectionEnabled;
    
    public bool IsDestructionDetectionEnabled
    {
        get => isDestructionDetectionEnabled;
        set => isDestructionDetectionEnabled = value;
    }

    void Start()
    {
        isDestructionDetectionEnabled = true;
    }

    void Update()
    {
        DetectDestruction();
    }

    public void DetectDestruction()
    {
        if (isDestructionDetectionEnabled)
        {
            Sensor[] sensors = Utility.GetComponentsInDirectChildren(gameObject, typeof(Sensor)).Cast<Sensor>().ToArray();

            foreach (Sensor sensor in sensors)
            {
                Destructable collidingDestructable = (Destructable)sensor.GetComponentFromCell(Vector3.zero, typeof(Destructable));

                if (collidingDestructable != null)
                {
                    DestructCollision(collidingDestructable);
                }
                else if (sensor.IsCellBlocked(Vector3.zero))
                {
                    Destruct();
                }
            }
        }
    }

    public void Destruct()
    {
        Element[] childElements = Utility.GetComponentsInDirectChildren(gameObject, typeof(Element)).Cast<Element>().ToArray();

        for (int i = 0; i < childElements.Length; i++)
        {
            Element child = childElements[i];
            child.gameObject.transform.parent = gameObject.transform.parent;
            
            Destructable childDestructable = GetComponent<Destructable>();

            if (childDestructable != null)
            {
                childDestructable.DetectDestruction();
            }
        }

        GameObject.Instantiate(destroyedForm, transform.position, transform.rotation);
        GameObject.Destroy(gameObject);
    }

    public void DestructCollision(Destructable otherDestructable)
    {
        if (otherDestructable.strength > strength)
        {
            Destruct();
        }
        else if (otherDestructable.strength < strength)
        {
            otherDestructable.Destruct();
        }
        else if (otherDestructable.strength == strength)
        {
            otherDestructable.Destruct();
            Destruct();
        }
    }
}

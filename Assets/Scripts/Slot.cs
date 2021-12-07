using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slot : MonoBehaviour
{
    public Material emptyMaterial;
    public Material filledMaterial;

    private bool isFilled = false;
    private MeshRenderer meshRenderer;

    public bool IsFilled
    {
        get => isFilled;
    }

    void Start()
    {
        meshRenderer = GetComponentInChildren<MeshRenderer>();
    }

    void Update()
    {
        if (isFilled)
        {
            meshRenderer.material = filledMaterial;
        }
        else
        {
            meshRenderer.material = emptyMaterial;
        }
    }

    void OnTriggerStay(Collider col)
    {
        Element slottable = col.transform.GetComponentInParent<Element>();
        
        if (slottable.HasProperty(ElementProperty.Slotable))
        {
            KinematicMover slottableMover = slottable.GetComponent<KinematicMover>();

            if (slottableMover != null && slottableMover.Mode == KinematicMoverMode.snapped)
            {
                isFilled = true;
                return;
            }
        }

        isFilled = false;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeMaterialStateChange : StateChange
{
    private GameObject gameObject;
    private Material newMaterial;
    private Material oldMaterial;
    private float changeTime;

    public ChangeMaterialStateChange(GameObject gameObject, Material newMaterial, float changeTime)
    {
        this.gameObject = gameObject;
        this.newMaterial = newMaterial;
        this.oldMaterial = gameObject.GetComponent<MeshRenderer>().material;
        this.changeTime = changeTime;
    }

    public override StateChangeType GetStateChangeCode()
    {
        return StateChangeType.ChangeMaterial;
    }

    public override void Do()
    {
        gameObject.GetComponent<MeshRenderer>().material = newMaterial;
    }

    public override void Undo()
    {
        gameObject.GetComponent<MeshRenderer>().material = oldMaterial;
    }

    public override void Render(float timer)
    {
        if (timer >= changeTime)
        {
            gameObject.GetComponent<MeshRenderer>().material = newMaterial;            
        }
        else
        {
            gameObject.GetComponent<MeshRenderer>().material = oldMaterial;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeakBlock : MonoBehaviour
{
    public enum WeakBlockState
    {
        Stable,
        Unstable,
        Broken
    }
    
    public Mesh unstableMesh;
    public GameObject brokenWeakBlockPieces;

    private WeakBlockState state;
    private Sensor sensor;
    private MeshFilter meshFilter;
    
    void Start()
    {
        state = WeakBlockState.Stable;
        sensor = GetComponentInChildren<Sensor>();
        meshFilter = GetComponentInChildren<MeshFilter>();
    }

    void FixedUpdate()
    {
        bool looseObjectAbove = sensor.DoesCellContainElementProperty(Vector3.up, ElementProperty.Loose);

        if (state == WeakBlockState.Stable && looseObjectAbove)
        {
            state = WeakBlockState.Unstable;
            meshFilter.mesh = unstableMesh;
        }

        if (state == WeakBlockState.Unstable && !looseObjectAbove)
        {
            state = WeakBlockState.Broken;

            GameObject.Instantiate(brokenWeakBlockPieces, transform.position, transform.rotation);
            GameObject.Destroy(gameObject);
        }
    }
}

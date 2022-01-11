using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageMeshDefinition
{
    public Mesh edgeAboveLeft;
    public Mesh edgeAboveRight;
    public Mesh edgeBelowLeft;
    public Mesh edgeBelowRight;

    public Mesh outerCornerAboveLeft;
    public Mesh outerCornerAboveRight;
    public Mesh outerCornerBelowLeft;
    public Mesh outerCornerBelowRight;

    public Mesh edgeLeftCap;
    public Mesh edgeRightCap;
    public Mesh outerCornerCap;
    public Mesh centerCap;

    public StageMeshDefinition()
    {
        centerCap = Resources.Load<GameObject>("Models/StageMeshes/CenterCap").GetComponent<MeshFilter>().sharedMesh;
    }
}


// // Edge Inset
// public Mesh edgeInsetDef;

// // Outer Corner Inset Left
// public Mesh outerCornerInsetLeftDef;

// // Outer Corner Inset Right
// public Mesh outerCornerInsetRightDef;

// // Outer Corner Inset Both
// public Mesh outerCornerInsetBothDef;

// // Inner Corner Inset
// public Mesh innerCornerInsetDef;

// // Edge Inset Cap
// public Mesh edgeInsetCapDef;

// // Outer Corner Inset Left Cap
// public Mesh outerCornerInsetLeftCapDef;

// // Outer Corner Inset Right Cap
// public Mesh outerCornerInsetRightCapDef;

// // Outer Corner Inset Both Cap
// public Mesh outerCornerInsetBothCapDef;

// // Inner Corner Inset Cap
// public Mesh innerCornerInsetCapDef;

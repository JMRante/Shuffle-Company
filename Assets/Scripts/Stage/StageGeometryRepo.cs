using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StageGeometryType
{
    Square,
    SmallBevel,
    LargeBevel,
    SmallCurve,
    LargeCurve,
    WeakJagged,
    StrongJagged,
    WeakWave,
    StrongWave
}

public class StageGeometryRepo
{
    public JaggedStageMeshDefinition jaggedStageMeshDefinition;

    public StageGeometryRepo()
    {
        jaggedStageMeshDefinition = new JaggedStageMeshDefinition();
    }
}

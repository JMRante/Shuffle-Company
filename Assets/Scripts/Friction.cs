using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Friction : MonoBehaviour
{
    private KinematicMover parentMover;
    private SnappingGravity gravityComp;

    private Transform defaultParent;
    private bool trigger;

    void Start()
    {
        parentMover = GetComponent<KinematicMover>();
        gravityComp = GetComponent<SnappingGravity>();

        defaultParent = transform.parent;
    }

    void Update()
    {
        // if (!gravityComp.IsFalling && gravityComp.isFallingEnabled && gravityComp.IsSolidBelow)
        // {
        //     KinematicMover parentMoverCandidate = GetFrictionDominantKinematicMoverParent();

        //     if (parentMoverCandidate != null && CanMoveWithParent(parentMoverCandidate.Velocity.normalized))
        //     {
        //         mover.ParentMover = parentMoverCandidate;
        //     }
        //     else
        //     {
        //         mover.ParentMover = null;
        //     }
        // }
        // else
        // {
        //     mover.ParentMover = null;
        // }
        KinematicMover parentMoverCandidate = GetFrictionDominantKinematicMoverParent();

        if (!gravityComp.IsFalling && gravityComp.isFallingEnabled && parentMoverCandidate != null)
        {
            if (transform.parent != parentMoverCandidate.gameObject.transform)
            {
                transform.SetParent(parentMoverCandidate.gameObject.transform);
            }
        }
        else 
        {
            if (transform.parent != defaultParent)
            {
                transform.SetParent(defaultParent);
            }
        }
    }

    private KinematicMover GetFrictionDominantKinematicMoverParent()
    {
        Sensor[] sensors = Utility.GetComponentsInDirectChildren(gameObject, typeof(Sensor)).Cast<Sensor>().ToArray();

        Dictionary<KinematicMover, int> parentMovers = new Dictionary<KinematicMover, int>();

        foreach (Sensor sensor in sensors)
        {
            // if (gameObject.name == "RealtimeWoodCrate (22)") Debug.Log("Sensor :" + sensor.transform.position);
            if (sensor.DoesRayContainElementProperty(Vector3.down * 0.5f, ElementProperty.Frictional))
            {
                KinematicMover parentMover = (KinematicMover)sensor.GetComponentFromRay(Vector3.down, typeof(KinematicMover));
                
                if (!parentMovers.ContainsKey(parentMover))
                {
                    parentMovers[parentMover] = 1;
                }
                else
                {
                    parentMovers[parentMover] += 1;
                }
            }
        }

        if (parentMovers.Count == 0)
        {
            return null;
        }

        List<KinematicMover> topMovers = new List<KinematicMover>();
        int candidateMaxParentCoverage = 0;

        foreach (KeyValuePair<KinematicMover, int> entry in parentMovers)
        {
            if (entry.Value > candidateMaxParentCoverage)
            {
                topMovers.Clear();
                candidateMaxParentCoverage = entry.Value;
                topMovers.Add(entry.Key);
            }
            else if (entry.Value == candidateMaxParentCoverage)
            {
                topMovers.Add(entry.Key);
            }
        }

        if (topMovers.Count == 1)
        {
            return topMovers[0];
        }

        

        // List<KinematicMover> fastestMovers = new List<KinematicMover>();
        // float candidateMaxParentSpeed = 0f;

        // foreach (KinematicMover mover in topMovers)
        // {
        //     if (mover.GetNetVelocity().magnitude > candidateMaxParentSpeed)
        //     {
        //         fastestMovers.Clear();
        //         candidateMaxParentSpeed = mover.GetNetVelocity().magnitude;
        //         fastestMovers.Add(mover);
        //     }
        //     else if (mover.GetNetVelocity().magnitude == candidateMaxParentSpeed)
        //     {
        //         fastestMovers.Add(mover);
        //     }
        // }

        // if (fastestMovers.Count == 1)
        // {
        //     return fastestMovers[0];
        // }

        return null;
    }

    public bool CanMoveWithParent(Vector3 parentDirection)
    {
        if (!gravityComp.IsSolidBelow)
        {
            return false;
        }

        if (!gravityComp.IsFalling)
        {
            Sensor[] sensors = Utility.GetComponentsInDirectChildren(gameObject, typeof(Sensor)).Cast<Sensor>().ToArray();

            foreach (Sensor sensor in sensors)
            {
                if (sensor.IsCellBlocked(parentDirection))
                {
                    return false;
                }
            }

            return true;
        }
        else
        {
            return false;
        }
    }
}

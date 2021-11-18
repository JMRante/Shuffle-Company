using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Friction : MonoBehaviour
{
    private KinematicMover mover;
    private SnappingGravity gravityComp;

    private Transform defaultParent;

    void Start()
    {
        mover = GetComponent<KinematicMover>();
        gravityComp = GetComponent<SnappingGravity>();

        defaultParent = transform.parent;
    }

    void Update()
    {
        KinematicMover parentMoverCandidate = GetFrictionDominantKinematicMoverParent();
        
        if (!gravityComp.IsFalling && gravityComp.isFallingEnabled && parentMoverCandidate != null)
        {
            Transform parentTransformCandidate = parentMoverCandidate.gameObject.transform;

            if (CanMoveWithParent(parentTransformCandidate) || parentMoverCandidate.NetVelocity == Vector3.zero)
            {
                if (transform.parent != parentTransformCandidate)
                {
                    transform.SetParent(parentTransformCandidate);
                }
            }
            else
            {
                if (transform.parent != defaultParent)
                {
                    transform.SetParent(defaultParent);
                    mover.Snap(Utility.Round(transform.localPosition));
                }
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
        int stageParentShare = 0;

        foreach (Sensor sensor in sensors)
        {
            if (sensor.DoesRayContainElementProperty(Vector3.down * 0.5f, ElementProperty.Frictional))
            {
                KinematicMover parentMover = (KinematicMover)sensor.GetComponentFromRay(Vector3.down, typeof(KinematicMover));
                
                if (parentMover == null)
                {
                    stageParentShare += 1;
                }
                else
                {
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
        }

        if (parentMovers.Count == 0)
        {
            return null;
        }

        List<KinematicMover> topMovers = new List<KinematicMover>();
        int candidateMaxParentCoverage = stageParentShare;

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

        if (topMovers.Count == 0)
        {
            return null;
        }
        else if (topMovers.Count == 1)
        {
            return topMovers[0].GetComponent<KinematicMover>();
        }

        List<KinematicMover> fastestMovers = new List<KinematicMover>();
        float candidateMaxParentSpeed = 0f;

        foreach (KinematicMover mover in topMovers)
        {
            if (mover.Velocity.magnitude > candidateMaxParentSpeed)
            {
                fastestMovers.Clear();
                candidateMaxParentSpeed = mover.Velocity.magnitude;
                fastestMovers.Add(mover);
            }
            else if (mover.Velocity.magnitude == candidateMaxParentSpeed)
            {
                fastestMovers.Add(mover);
            }
        }

        if (fastestMovers.Count == 1)
        {
            return fastestMovers[0];
        }

        return null;
    }

    public bool CanMoveWithParent(Transform parentTransform)
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
                Vector3 positionOnParentCandidate = parentTransform.TransformPoint(Utility.Round(parentTransform.InverseTransformPoint(sensor.transform.position)));

                if (sensor.IsCellPositionBlocked(positionOnParentCandidate))
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

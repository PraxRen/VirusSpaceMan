using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "NewAngleToTargetScannerStrategy", menuName = "Core/ScannerStrategy/AngleToTargetScannerStrategy")]
public class AngleToTargetScannerStrategy : ScriptableObject, IScannerStrategy
{
    public IEnumerable<Collider> Sort(IEnumerable<Collider> targets, Transform transform)
    {
        return targets.OrderBy(target => GetAngleToTarget(transform, target.transform.position)).ToList();
    }

    private float GetAngleToTarget(Transform transform, Vector3 targetPosition)
    {
        return Vector3.SignedAngle(transform.forward, (targetPosition - transform.position).normalized, Vector3.up);
    }
}
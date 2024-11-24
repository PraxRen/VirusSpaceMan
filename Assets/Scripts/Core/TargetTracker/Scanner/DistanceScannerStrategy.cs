using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "NewDistanceScannerStrategy", menuName = "Core/ScannerStrategy/DistanceScannerStrategy")]
public class DistanceScannerStrategy : ScriptableObject, IScannerStrategy
{
    public IEnumerable<Collider> Sort(IEnumerable<Collider> targets, Transform transform)
    {
        return targets.OrderBy(target => (target.transform.position - transform.position).sqrMagnitude).ToList();
    }
}
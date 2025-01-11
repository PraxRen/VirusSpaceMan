using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "NewAngleToTargetScannerStrategy", menuName = "Core/ScannerStrategy/AngleToTargetScannerStrategy")]
public class AngleToTargetScannerStrategy : ScriptableObject, IScannerStrategy
{
    private static Transform _cameraTransform;
    private Transform CameraTransform
    {
        get
        {
            if (_cameraTransform == null)
            {
                _cameraTransform = Camera.main.transform;
            }

            return _cameraTransform;
        }
    }

    public IEnumerable<Collider> Sort(IEnumerable<Collider> targets, Transform transform)
    {
        return targets.OrderBy(target => GetAngleToTarget(CameraTransform, target.transform.position)).ToList();
    }

    private float GetAngleToTarget(Transform transform, Vector3 targetPosition)
    {
        return Vector3.SignedAngle(transform.forward, (targetPosition - transform.position).normalized, Vector3.up);
    }
}
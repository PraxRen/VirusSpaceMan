using UnityEngine;
using System.Linq;

public class ActivatorRigHit : MonoBehaviour, IHitReaction
{
    [SerializeField] private RigHit[] _rigsHit;
    [SerializeField] private float _distanceRaction;

    private Transform _transform;

    private void Awake()
    {
        _transform = transform;
    }

    public void ApplyHit(Vector3 forceDirection, Vector3 hitPoint)
    {
        bool isFindedRigHit = false;
        float sqrDistanceRaction = _distanceRaction * _distanceRaction;

        foreach (RigHit rigHitInHash in _rigsHit)
        {
            float sqrDistance = (rigHitInHash.transform.position - hitPoint).sqrMagnitude;

            if (sqrDistance < sqrDistanceRaction)
            {
                rigHitInHash.AddForce(forceDirection);
                isFindedRigHit = true;
            }
        }

        if (isFindedRigHit == false)
        {
            RigHit rigHit = _rigsHit.OrderBy(rigHit => (rigHit.transform.position - hitPoint).sqrMagnitude).FirstOrDefault();
            rigHit?.AddForce(forceDirection);
        }
    }

    public void Handle(IWeaponReadOnly weapon, Vector3 hitPoint, float damage)
    {
        Vector3 forceDirection = CalculateForceDirection(weapon,hitPoint);
#if UNITY_EDITOR
        Debug.DrawLine(hitPoint, hitPoint + forceDirection, Color.yellow, 2f);
#endif
        ApplyHit(forceDirection, hitPoint);
    }

    private Vector3 CalculateForceDirection(IWeaponReadOnly weapon, Vector3 hitPoint)
    {
        hitPoint = Vector3.Lerp(hitPoint, weapon.Position, 0.5f);
        Vector3 direction = (hitPoint - new Vector3(_transform.position.x, hitPoint.y, _transform.position.z)).normalized * -1;
        direction.y = 0;
        return direction;
    }
}
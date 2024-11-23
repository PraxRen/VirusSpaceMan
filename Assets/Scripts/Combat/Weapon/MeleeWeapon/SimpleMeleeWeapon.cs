using UnityEngine;

public abstract class SimpleMeleeWeapon : Weapon
{
    [SerializeField] private float _radiusDamage;
    [SerializeField] private Transform _pointRaycast;

    protected override void RunDamageAddon()
    {
        Vector3 direction = (Fighter.LookTracker.Position - _pointRaycast.position).normalized;
        bool hasHit = Physics.SphereCast(_pointRaycast.position, _radiusDamage, direction, out RaycastHit hit, Config.DistanceAttack, Fighter.LayerMaskCollision, QueryTriggerInteraction.Ignore);

        if (hasHit == false)
            return;

        if (CanCollide(hit.collider) == false)
            return;

#if UNITY_EDITOR
        Debug.DrawLine(hit.collider.transform.position, hit.point, Color.red, 2f);
#endif
        HandleCollide(hit.collider, hit.point);
    }
}
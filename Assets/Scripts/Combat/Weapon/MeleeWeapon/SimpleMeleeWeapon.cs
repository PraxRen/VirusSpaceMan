using UnityEngine;

public abstract class SimpleMeleeWeapon : Weapon
{
    [SerializeField] private float _radiusDamage;
    [SerializeField] private Transform _pointRaycast;

    protected override void RunDamageAddon()
    {
        bool hasHit = Physics.SphereCast(_pointRaycast.position, _radiusDamage, _pointRaycast.forward, out RaycastHit hit, Config.DistanceAttack, Fighter.LayerMaskCollision, QueryTriggerInteraction.Ignore);

        if (hasHit == false)
            return;

        if (CanCollide(hit.collider) == false)
            return;

        HandleCollide(hit.collider);
    }
}
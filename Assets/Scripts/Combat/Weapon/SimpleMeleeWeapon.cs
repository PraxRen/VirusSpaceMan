using UnityEngine;

public abstract class SimpleMeleeWeapon : Weapon
{
    [SerializeField] private float _radiusDamage;

    private Transform _transform;

    protected override void AwakeAddon()
    {
        _transform = transform;
    }

    protected override void RunDamageAddon()
    {
        bool hasHit = Physics.SphereCast(_transform.position, _radiusDamage, _transform.forward, out RaycastHit hit, Config.DistanceAttack, Fighter.LayerMaskCollision, QueryTriggerInteraction.Ignore);

        if (hasHit == false)
            return;

        if (Can—ollide(hit.collider) == false)
            return;

        Handle—ollide(hit.collider);
    }
}
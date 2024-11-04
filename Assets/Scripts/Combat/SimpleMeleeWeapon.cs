using UnityEngine;

public class SimpleMeleeWeapon : Weapon
{
    [SerializeField] private float _radiusDamage;

    private Transform _transform;

    private void Awake()
    {
        _transform = transform;
    }

    protected override void RunDamageAddon()
    {
        bool hasHit = Physics.SphereCast(_transform.position, _radiusDamage, _transform.forward, out RaycastHit hit, Config.DistanceAttack, Fighter.LayerMaskDamageable, QueryTriggerInteraction.Ignore);

        if (hasHit == false)
            return;

        if (Fighter.CanHit(transform) == false)
            return;

        Hit(hit.transform);
    }
}

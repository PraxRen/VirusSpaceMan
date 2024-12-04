using UnityEngine;

[RequireComponent(typeof(PlayerInputReader))]
public class Player : Character
{
    private PlayerInputReader _inputReader;

    protected override void AwakeAddon()
    {
        _inputReader = GetComponent<PlayerInputReader>();
    }

    protected override void EnableAddon()
    {
        Fighter.ChangedWeapon += OnChangedWeapon;
        Fighter.RemovedWeapon += OnRemovedWeapon;
        ScannerDamageable.ChangedCurrentTarget += OnChangedTarget;
        ScannerDamageable.RemovedCurrentTarget += OnRemovedTarget;
        _inputReader.ChangedScrollNextTarget += OnChangedScrollNextTarget;
        _inputReader.ChangedScrollPreviousTarget += OnChangedScrollPreviousTarget;

        if (Fighter.Weapon != null)
        {
            ScannerDamageable.StartScan(Fighter.LayerMaskDamageable, Fighter.Weapon.Config.DistanceAttack);
        }
    }

    protected override void DisableAddon()
    {
        Fighter.ChangedWeapon -= OnChangedWeapon;
        Fighter.RemovedWeapon -= OnRemovedWeapon;
        ScannerDamageable.ChangedCurrentTarget -= OnChangedTarget;
        ScannerDamageable.RemovedCurrentTarget -= OnRemovedTarget;
        _inputReader.ChangedScrollNextTarget -= OnChangedScrollNextTarget;
        _inputReader.ChangedScrollPreviousTarget -= OnChangedScrollPreviousTarget;
    }

    private void Update()
    {
        HandleLocomotion();
        HandleCombat();
    }

    private void HandleLocomotion()
    {
        if (_inputReader.DirectionMove == Vector2.zero)
            return;

        if (Mover.CanMove() == false)
        {
            if (HandlerInteraction.IsActive)
                HandlerInteraction.Cancel();

            return;
        }

        if (ScannerDamageable.Target == null)
            Mover.LookAtDirection(_inputReader.DirectionMove);

        Mover.Move(_inputReader.DirectionMove);
    }

    private void HandleCombat()
    {
        if (ScannerDamageable.Target == null)
            return;

        Vector3 direction = (LookTracker.Position - Transform.position).normalized;
        Mover.LookAtDirection(new Vector2(direction.x, direction.z));

        if (Fighter.CanAttack() == false)
            return;

        Fighter.Attack();
    }

    private void OnChangedTarget(Collider collider)
    {
        Fighter.ActivateWeapon();

        if (collider.TryGetComponent(out ITarget target) == false)
            return;

        float center = collider.bounds.center.y - target.Position.y;
        float offsetCenter = 0.2f;
        LookTracker.SetTarget(target, (target.Rotation * Vector3.up) * (center + offsetCenter));
    }

    private void OnRemovedTarget(Collider target)
    {
        Fighter.DeactivateWeapon();
        LookTracker.ResetTarget();
    }

    private void OnChangedWeapon(IWeaponReadOnly weapon)
    {
        ScannerDamageable.StartScan(Fighter.LayerMaskDamageable, weapon.Config.DistanceAttack);
    }

    private void OnRemovedWeapon()
    {
        ScannerDamageable.ResetRadius();
    }

    private void OnChangedScrollNextTarget()
    {
        ScannerDamageable.NextTarget();

    }

    private void OnChangedScrollPreviousTarget()
    {
        ScannerDamageable.PreviousTarget();
    }
}
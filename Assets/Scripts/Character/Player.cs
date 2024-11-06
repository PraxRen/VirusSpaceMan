using UnityEngine;

[RequireComponent(typeof(PlayerInputReader), typeof(Fighter), typeof(ScannerDamageable))]
public class Player : Character
{
    private PlayerInputReader _inputReader;
    private ScannerDamageable _scannerDamageable;
    private Fighter _fighter;

    protected override void AwakeAddon()
    {
        _inputReader = GetComponent<PlayerInputReader>();
        _fighter = GetComponent<Fighter>();
        _scannerDamageable = GetComponent<ScannerDamageable>();
    }

    protected override void EnableAddon()
    {
        _scannerDamageable.ChangedTarget += OnChangedTarget;
        _scannerDamageable.RemovedTarget += OnRemovedTarget;
    }

    protected override void DisableAddon()
    {
        _scannerDamageable.ChangedTarget -= OnChangedTarget;
        _scannerDamageable.RemovedTarget -= OnRemovedTarget;
    }

    private void Update()
    {
        HandleLocomotion();
        HandleCombat();
    }

    private void HandleLocomotion()
    {
        if (Mover.CanMove() == false)
            return;

        Mover.Move(_inputReader.DirectionMove);
    }

    private void HandleCombat()
    {
        if (_scannerDamageable.Target == null)
            return;

        _fighter.LookAtTarget((_scannerDamageable.Target.Position - Transform.position).normalized);

        if (_fighter.CanAttack() == false)
            return;

        _fighter.Attack();
    }

    private void OnChangedTarget(IDamageable damageable)
    {
        _fighter.ActivateWeapon();
    }

    private void OnRemovedTarget()
    {
        _fighter.DeactivateWeapon();
    }
}
using UnityEngine;

[RequireComponent(typeof(PlayerInputReader), typeof(Fighter), typeof(Scanner))]
public class Player : Character
{
    private PlayerInputReader _inputReader;
    private Scanner _scanner;
    private Fighter _fighter;

    protected override void AwakeAddon()
    {
        _inputReader = GetComponent<PlayerInputReader>();
        _fighter = GetComponent<Fighter>();
        _scanner = GetComponent<Scanner>();
    }

    protected override void EnableAddon()
    {
        _fighter.ChangedWeapon += OnChangedWeapon;
        _fighter.RemovedWeapon += OnRemovedWeapon;
        _scanner.ChangedCurrentTarget += OnChangedTarget;
        _scanner.RemovedCurrentTarget += OnRemovedTarget;
        _inputReader.ChangedScrollTarget += OnChangedScrollTarget;

        if (_fighter.Weapon != null)
        {
            _scanner.StartScan(_fighter.LayerMaskDamageable, _fighter.Weapon.Config.DistanceAttack);
        }
    }

    protected override void DisableAddon()
    {
        _fighter.ChangedWeapon -= OnChangedWeapon;
        _fighter.RemovedWeapon -= OnRemovedWeapon;
        _scanner.ChangedCurrentTarget -= OnChangedTarget;
        _scanner.RemovedCurrentTarget -= OnRemovedTarget;
        _inputReader.ChangedScrollTarget -= OnChangedScrollTarget;
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
        if (_scanner.Target == null)
            return;

        _fighter.LookAtTarget((_scanner.Target.transform.position - Transform.position).normalized);

        if (_fighter.CanAttack() == false)
            return;

        _fighter.Attack();
    }

    private void OnChangedTarget(Collider target)
    {
        _fighter.ActivateWeapon();
        Transform targetTransform = target.transform;
        float height = target.bounds.center.y - targetTransform.position.y;
        LookTarget.SetTarget(targetTransform, Vector3.up * height);
    }

    private void OnRemovedTarget(Collider target)
    {
        _fighter.DeactivateWeapon();
        LookTarget.ResetTarget();
    }

    private void OnChangedWeapon(IWeaponReadOnly weapon)
    {
        _scanner.StartScan(_fighter.LayerMaskDamageable, weapon.Config.DistanceAttack);
    }

    private void OnRemovedWeapon()
    {
        _scanner.ResetRadius();
    }

    private void OnChangedScrollTarget(float value)
    {
        if (value == 0)
            return;

        bool isNext = value > 0;

        if (isNext)
            _scanner.NextTarget();
        else
            _scanner.PreviousTarget();
    }
}
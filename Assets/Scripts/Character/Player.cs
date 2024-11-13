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
        _inputReader.ChangedScrollNextTarget += OnChangedScrollNextTarget;
        _inputReader.ChangedScrollPreviousTarget += OnChangedScrollPreviousTarget;

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
            return;

        Mover.Move(_inputReader.DirectionMove);
        Mover.LookAtDirection(_inputReader.DirectionMove);
    }

    private void HandleCombat()
    {
        if (_scanner.Target == null)
            return;

        Vector3 direction = (LookTarget.Position - Transform.position).normalized;
        Mover.LookAtDirection(new Vector2(direction.x, direction.z));

        if (_fighter.CanAttack() == false)
            return;

        _fighter.Attack();
    }

    private void OnChangedTarget(Collider target)
    {
        _fighter.ActivateWeapon();
        Transform targetTransform = target.transform;
        float center = target.bounds.center.y - targetTransform.position.y;
        float offsetCenter = 0.2f;
        LookTarget.SetTarget(targetTransform, Vector3.up * (center + offsetCenter));
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

    private void OnChangedScrollNextTarget()
    {
        _scanner.NextTarget();

    }

    private void OnChangedScrollPreviousTarget()
    {
        _scanner.PreviousTarget();
    }
}
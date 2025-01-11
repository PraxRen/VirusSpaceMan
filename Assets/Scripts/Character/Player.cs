using UnityEngine;

[RequireComponent(typeof(PlayerInputReader), typeof(ActivatorSimpleEvent))]
public class Player : Character
{
    [SerializeField] private LayerMask _layerMaskSimpleEventAttack;

    private PlayerInputReader _inputReader;
    private ActivatorSimpleEvent _activatorSimpleEvent;
    private SimpleEvent _simpleEventAttack;

    protected override void AwakeAddon()
    {
        _inputReader = GetComponent<PlayerInputReader>();
        _activatorSimpleEvent = GetComponent<ActivatorSimpleEvent>();
    }

    protected override void EnableAddon()
    {
        ScannerDamageable.ChangedCurrentTarget += OnChangedTarget;
        ScannerDamageable.RemovedCurrentTarget += OnRemovedTarget;
        _inputReader.ChangedScrollNextTarget += OnChangedScrollNextTarget;
        _inputReader.ChangedScrollPreviousTarget += OnChangedScrollPreviousTarget;
    }

    protected override void DisableAddon()
    {
        ScannerDamageable.ChangedCurrentTarget -= OnChangedTarget;
        ScannerDamageable.RemovedCurrentTarget -= OnRemovedTarget;
        _inputReader.ChangedScrollNextTarget -= OnChangedScrollNextTarget;
        _inputReader.ChangedScrollPreviousTarget -= OnChangedScrollPreviousTarget;
    }

    protected override void AddonOnChangedWeapon(IWeaponReadOnly weapon)
    {
        _simpleEventAttack = new SimpleEvent(TypeSimpleEvent.Attack, _layerMaskSimpleEventAttack, weapon.Config.DistanceNoise);
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
            if (Interactor.IsActive)
                Interactor.Cancel();

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
        _activatorSimpleEvent.Run(Fighter, (IDamageable)LookTracker.Target, _simpleEventAttack);
    }

    private void OnChangedTarget(Collider collider)
    {
        Fighter.ActivateWeapon();

        if (collider.TryGetComponent(out IDamageable damageable) == false)
            return;

        LookTracker.SetTarget(damageable, damageable.Center - damageable.Position);
    }

    private void OnRemovedTarget(Collider target)
    {
        Fighter.DeactivateWeapon();
        LookTracker.ResetTarget();
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
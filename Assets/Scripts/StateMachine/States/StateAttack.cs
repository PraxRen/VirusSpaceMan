using System;
using UnityEngine;
using UnityEngine.AI;

public class StateAttack : State, IModeMoverProvider
{
    private const float MinRadiusMovePosition = 0.7f;
    private const float MinTimeUpdatePosition = 3f;
    private const float MaxTimeUpdatePosition = 10f;
    private const float MinFactorDistanceAtack = 0.1f;
    private const float MaxFactorDistanceAtack = 0.7f;

    private readonly Fighter _fighter;
    private readonly Mover _mover;
    private readonly NavMeshAgent _navMeshAgent;

    private IDamageable _target;

    private float _timeUpdatePosition;
    private float _timerUpdatePosition;
    private Vector3 _positionMove;

    public ModeMover ModeMover { get; }

    public StateAttack(string id, AICharacter character, float timeSecondsWaitHandle, ModeMover modeMover) : base(id, character, timeSecondsWaitHandle)
    {
        if (character.TryGetComponent(out _fighter) == false)
            throw new InvalidOperationException($"Initialization error \"{nameof(State)}\"! The component \"{nameof(Fighter)}\" required for operation \"{GetType().Name}\".");

        if (character.TryGetComponent(out _mover) == false)
            throw new InvalidOperationException($"Initialization error \"{nameof(State)}\"! The component \"{nameof(Mover)}\" required for operation \"{GetType().Name}\".");

        if (character.TryGetComponent(out _navMeshAgent) == false)
            throw new InvalidOperationException($"Initialization error \"{nameof(State)}\"! The component \"{nameof(NavMeshAgent)}\" required for operation \"{GetType().Name}\".");

        ModeMover = modeMover;
    }

    protected override void EnterAfterAddon()
    {
        Collider colliderTarget = Character.ScannerDamageable.Target;

        if (colliderTarget == null || colliderTarget.TryGetComponent(out _target) == false)
            throw new InvalidOperationException($"The component \"{nameof(IDamageable)}\" required for operation \"{GetType().Name}\"");

        UpdatePosition();
        _fighter.ActivateWeapon();
        float center = colliderTarget.bounds.center.y - _target.Position.y;
        float offsetCenter = 0.2f;
        Character.LookTracker.SetTarget(_target, (_target.Rotation * Vector3.up) * (center + offsetCenter));
        Character.MoveTracker.SetTarget(_target, Vector3.zero);
    }

    protected override void TickAddon(float deltaTime)
    {
        _timerUpdatePosition += deltaTime;

        if (_timerUpdatePosition < _timeUpdatePosition)
            return;

        UpdatePosition();
    }

    private void UpdatePosition()
    {
        _positionMove = SimpleUtils.GetRandomPositionInsideCircle(_target.Position, _fighter.Weapon.Config.DistanceAttack * MaxFactorDistanceAtack, 1f + (_fighter.Weapon.Config.DistanceAttack * MinFactorDistanceAtack));
        _timeUpdatePosition = UnityEngine.Random.Range(MinTimeUpdatePosition, MaxTimeUpdatePosition);
        _timerUpdatePosition = 0f;
    }

    public override void Update()
    {
        if ((_positionMove - Character.Transform.position).sqrMagnitude > MinRadiusMovePosition * MinRadiusMovePosition)
        {
            Vector2 directionMove = Navigation.CalculateDirectionVector2(_navMeshAgent, Character.Transform, _positionMove);
            _mover.Move(directionMove);
        }

        Vector3 directionLook = (_target.Position - Character.Transform.position).normalized;
        _mover.LookAtDirection(new Vector2(directionLook.x, directionLook.z));

        if (_fighter.CanAttack())
            _fighter.Attack();
    }

    protected override void ExitAfterAddon() 
    {
        _fighter.DeactivateWeapon();
        Character.LookTracker.ResetTarget();
    }
}
using System;
using UnityEngine;
using UnityEngine.AI;

public class StateAttack : State, IModeMoverProvider
{
    private const float MinRadiusMovePosition = 0.7f;
    private const float MinTimeUpdatePosition = 2f;
    private const float MaxTimeUpdatePosition = 5f;
    private const float MinFactorDistanceAtack = 0.1f;
    private const float MaxFactorDistanceAtack = 0.7f;

    private readonly Fighter _fighter;
    private readonly Mover _mover;
    private readonly NavMeshAgent _navMeshAgent;
    private readonly ActivatorSimpleEvent _activatorSimpleEvent;
    private readonly ListenerSimpleEvent _listenerSimpleEvent;
    private readonly LayerMask _layerMaskSimpleEventAttack;

    private IDamageable _target;
    private float _timeUpdatePosition;
    private float _timerUpdatePosition;
    private Vector3 _positionMove;
    private SimpleEvent _simpleEventAttack;

    public ModeMover ModeMover { get; }

    public StateAttack(string id, AICharacter character, float timeSecondsWaitHandle, ModeMover modeMover, LayerMask layerMaskSimpleEventAttack) : base(id, character, timeSecondsWaitHandle)
    {
        if (character.TryGetComponent(out _fighter) == false)
            throw new InvalidOperationException($"Initialization error \"{nameof(State)}\"! The component \"{nameof(Fighter)}\" required for operation \"{GetType().Name}\".");

        if (character.TryGetComponent(out _mover) == false)
            throw new InvalidOperationException($"Initialization error \"{nameof(State)}\"! The component \"{nameof(Mover)}\" required for operation \"{GetType().Name}\".");

        if (character.TryGetComponent(out _navMeshAgent) == false)
            throw new InvalidOperationException($"Initialization error \"{nameof(State)}\"! The component \"{nameof(NavMeshAgent)}\" required for operation \"{GetType().Name}\".");

        if (character.TryGetComponent(out _activatorSimpleEvent) == false)
            throw new InvalidOperationException($"Initialization error \"{nameof(State)}\"! The component \"{nameof(ActivatorSimpleEvent)}\" required for operation \"{GetType().Name}\".");

        if (character.TryGetComponent(out _listenerSimpleEvent) == false)
            throw new InvalidOperationException($"Initialization error \"{nameof(State)}\"! The component \"{nameof(ListenerSimpleEvent)}\" required for operation \"{GetType().Name}\".");

        ModeMover = modeMover;
        _layerMaskSimpleEventAttack = layerMaskSimpleEventAttack;
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

        if (_fighter.CanAttack() == false)
            return;
        
        _fighter.Attack();
        _activatorSimpleEvent.Run(_fighter, _target, _simpleEventAttack);
    }

    protected override void EnterAddon()
    {
        Collider colliderTarget = Character.ScannerDamageable.Target;

        if (colliderTarget == null || colliderTarget.TryGetComponent(out _target) == false)
            throw new InvalidOperationException($"The component \"{nameof(IDamageable)}\" required for operation \"{GetType().Name}\"");

        _listenerSimpleEvent.RemoveSupportType(TypeSimpleEvent.Attack);
        _simpleEventAttack = new SimpleEvent(TypeSimpleEvent.Attack, _layerMaskSimpleEventAttack, _fighter.Weapon.Config.DistanceNoise);
        _fighter.ChangedWeapon += OnChangedWeapon;
        _fighter.ActivateWeapon();
        UpdatePosition();
    }

    protected override void ExitAfterAddon()
    {
        _fighter.DeactivateWeapon();
        _listenerSimpleEvent.AddSupportType(TypeSimpleEvent.Attack);
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

    private void OnChangedWeapon(IWeaponReadOnly weapon)
    {
        _simpleEventAttack = new SimpleEvent(TypeSimpleEvent.Attack, _layerMaskSimpleEventAttack, weapon.Config.DistanceNoise);
    }
}
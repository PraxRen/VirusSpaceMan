using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class Mover : MonoBehaviour, IMoverReadOnly, IAction, IModeMoverProvider
{
    private const float MinValueFactorSpeed = 0.1f;
    private const float MaxValueFactorSpeed = 1f;

    [SerializeField] private ModeMover _defaultModeMover;
    [SerializeField] private float _baseSpeedMove;
    [SerializeField] private float _baseSpeedRotation;
    [SerializeField] private float _gravity;
    [SerializeField] private float _speedUpdateInertia;
    [SerializeField] private ActionScheduler _actionScheduler;
    [SerializeField][SerializeInterface(typeof(IStepNotifier))] private MonoBehaviour _stepNotifierMonoBehaviour;
    [SerializeField] private LayerMask _groundLayer;
    [SerializeField][SerializeInterface(typeof(IModeMoverChanger))] private MonoBehaviour[] _modeMoverMonoBehaviourChangers;

    private Transform _transform;
    private CharacterController _characterController;
    private IModeMoverChanger[] _modeMoverChangers;
    private List<IModeMoverProvider> _modeMoverProviders;
    private IStepNotifier _stepNotifier;
    private Vector2 _inputMoveDirection;
    private Vector2 _inputLookDirection;
    private bool _canRotation = true;
    private float _factorSpeedMove;
    private float _factorSpeedRotation;

    public event Action StepTook;

    public Vector3 Velocity { get; private set; }
    public LayerMask GroundLayer => _groundLayer;
    public ModeMover ModeMover => _defaultModeMover;

    private void Awake()
    {
        _transform = transform;
        _characterController = GetComponent<CharacterController>();
        _stepNotifier = (IStepNotifier)_stepNotifierMonoBehaviour;
        _factorSpeedMove = 1f;
        _factorSpeedRotation = 1f;
        _modeMoverProviders = new List<IModeMoverProvider>();
        OnAddedModeMover(this);
        _modeMoverChangers = _modeMoverMonoBehaviourChangers.Cast<IModeMoverChanger>().ToArray();

    }

    private void OnEnable()
    {
        _stepNotifier.CreatedStep += OnCreatedStep;

        foreach (IModeMoverChanger modeMoverChanger in _modeMoverChangers)
        {
            modeMoverChanger.AddedModeMover += OnAddedModeMover;
            modeMoverChanger.RemovedModeMover += OnRemovedModeMover;
        }
    }

    private void OnDisable()
    {
        _stepNotifier.CreatedStep -= OnCreatedStep;

        foreach (IModeMoverChanger modeMoverChanger in _modeMoverChangers)
        {
            modeMoverChanger.AddedModeMover -= OnAddedModeMover;
            modeMoverChanger.RemovedModeMover -= OnRemovedModeMover;
        }
    }

    private void FixedUpdate()
    {
        HandleMove();
        HandleRotate();
    }

    public void Cancel() 
    {
        _inputMoveDirection = Vector3.zero;
        _inputLookDirection = Vector3.zero;
    }

    public bool CanMove()
    {
        return _actionScheduler.CanStartAction(this);
    }

    public void Move(Vector2 direction)
    {
        _actionScheduler.StartAction(this);
        _actionScheduler.SetBlock(this);
        _inputMoveDirection = direction;
    }

    public void SimpleMove(Vector2 direction)
    {
        _inputMoveDirection = direction;
    }

    public void LookAtDirection(Vector2 direction)
    {
        _inputLookDirection = direction;
    }

    public void BlockRotation()
    {
        _inputLookDirection = Vector3.zero;
        _canRotation = false;
    }

    public void UnblockRotation()
    {
        _canRotation = true;
    }

    private void HandleMove()
    {
        float speedMove = _baseSpeedMove * _factorSpeedMove;
        Velocity = new Vector3(_inputMoveDirection.x * speedMove, _gravity, _inputMoveDirection.y * speedMove);

        if (_inputMoveDirection == Vector2.zero)
        {
            _actionScheduler.ClearAction(this);
            return;
        }
        
        _characterController.Move(Velocity * Time.fixedDeltaTime);
        _inputMoveDirection = Vector2.MoveTowards(_inputMoveDirection, Vector2.zero, _speedUpdateInertia * Time.fixedDeltaTime);
    }

    private void HandleRotate()
    {
        if (_inputLookDirection == Vector2.zero)
            return;

        if (_canRotation == false)
            return;

        float speedRotation = _baseSpeedRotation * _factorSpeedRotation;
        transform.forward = Vector3.MoveTowards(_transform.forward, new Vector3(_inputLookDirection.x, 0f, _inputLookDirection.y), speedRotation * Time.fixedDeltaTime);
    }

    private void OnCreatedStep()
    {
        StepTook?.Invoke();
    }

    private void OnAddedModeMover(IModeMoverProvider modeMover)
    {
        if (_modeMoverProviders.Contains(modeMover))
            return;

        _modeMoverProviders.Add(modeMover);
        UpdateSpeed();
    }

    private void OnRemovedModeMover(IModeMoverProvider modeMover)
    {
        if (_modeMoverProviders.Contains(modeMover) == false)
            return;

        _modeMoverProviders.Remove(modeMover);
        UpdateSpeed();
    }

    private void UpdateSpeed()
    {
        _factorSpeedMove = 1f;
        _factorSpeedRotation = 1f;

        foreach (IModeMoverProvider provider in _modeMoverProviders)
        {
            _factorSpeedMove = Mathf.Clamp(_factorSpeedMove + provider.ModeMover.SpeedMove, MinValueFactorSpeed, MaxValueFactorSpeed);
            _factorSpeedRotation = Mathf.Clamp(_factorSpeedRotation + provider.ModeMover.SpeedRotation, MinValueFactorSpeed, MaxValueFactorSpeed);
        }
    }
}
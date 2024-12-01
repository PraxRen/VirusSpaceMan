using System;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class Mover : MonoBehaviour, IMoverReadOnly, IAction
{
    [SerializeField] private float _gravity;
    [SerializeField] private float _speedUpdateInertia;
    [SerializeField] private ActionScheduler _actionScheduler;
    [SerializeField][SerializeInterface(typeof(IStepNotifier))] private MonoBehaviour _stepNotifierMonoBehaviour;
    [SerializeField] private ModeMover _defaultModeMover;
    [SerializeField] private LayerMask _groundLayer;
    [SerializeField][SerializeInterface(typeof(IChangerModeMover))] private MonoBehaviour[] _switchesModeMoverMonoBehaviour;

    private Transform _transform;
    private CharacterController _characterController;
    private ModeMover _currentModeMover;
    private IChangerModeMover[] _switchesModeMover;
    private IStepNotifier _stepNotifier;
    private Vector2 _inputMoveDirection;
    private Vector2 _inputLookDirection;

    public event Action StepTook;

    public Vector3 Velocity { get; private set; }
    public LayerMask GroundLayer => _groundLayer;

    private void Awake()
    {
        _transform = transform;
        _characterController = GetComponent<CharacterController>();
        _stepNotifier = (IStepNotifier)_stepNotifierMonoBehaviour;
        _currentModeMover = _defaultModeMover;
        _switchesModeMover = _switchesModeMoverMonoBehaviour.Cast<IChangerModeMover>().ToArray();
    }

    private void OnEnable()
    {
        _stepNotifier.CreatedStep += OnCreatedStep;
        Array.ForEach(_switchesModeMover, (changerModeMover) => changerModeMover.ChangedModeMover += OnChangedModeMover);
    }

    private void OnDisable()
    {
        _stepNotifier.CreatedStep -= OnCreatedStep;
        Array.ForEach(_switchesModeMover, (changerModeMover) => changerModeMover.ChangedModeMover -= OnChangedModeMover);
    }

    private void FixedUpdate()
    {
        HandleMove();
        HandleRotate();
    }

    public void Cancel() 
    {
        _inputMoveDirection = Vector3.zero;
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

    public void LookAtDirection(Vector2 direction)
    {
        _inputLookDirection = direction;
    }

    private void HandleMove()
    {
        Velocity = new Vector3(_inputMoveDirection.x * _currentModeMover.SpeedMove, _gravity, _inputMoveDirection.y * _currentModeMover.SpeedMove);

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
        _transform.forward = Vector3.MoveTowards(_transform.forward, new Vector3(_inputLookDirection.x, 0f, _inputLookDirection.y), _currentModeMover.SpeedRotation * Time.fixedDeltaTime);
    }

    private void OnCreatedStep()
    {
        StepTook?.Invoke();
    }

    private void OnChangedModeMover(ModeMover modeMover)
    {
        _currentModeMover = modeMover ?? _defaultModeMover;
    }
}
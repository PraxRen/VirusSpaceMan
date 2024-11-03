using System;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class Mover : MonoBehaviour, IMover
{
    [SerializeField] private bool _canLookAtMoveDirection;
    [SerializeField] private float _gravity;
    [SerializeField][SerializeInterface(typeof(IStepNotifier))] private MonoBehaviour _stepNotifierMonoBehaviour;
    [SerializeField] private ModeMover _defaultModeMover;
    [SerializeField][SerializeInterface(typeof(IChangerModeMover))] private MonoBehaviour[] _switchesModeMoverMonoBehaviour;

    private Transform _transform;
    private CharacterController _characterController;
    private ModeMover _currentModeMover;
    private IChangerModeMover[] _switchesModeMover;
    private IStepNotifier _stepNotifier;
    private Vector2 _inputMoveDirection;

    public event Action StepTook;

    public Vector3 Velocity { get; private set; }

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
    }

    public void Move(Vector2 direction)
    {
        _inputMoveDirection = direction;
    }

    private void HandleMove()
    {
        Velocity = new Vector3(_inputMoveDirection.x * _currentModeMover.SpeedMove, _gravity, _inputMoveDirection.y * _currentModeMover.SpeedMove);
        _characterController.Move(Velocity * Time.fixedDeltaTime);

        if (_canLookAtMoveDirection && _inputMoveDirection != Vector2.zero)
            _transform.forward = Vector3.MoveTowards(_transform.forward, new Vector3(_inputMoveDirection.x, 0f, _inputMoveDirection.y), _currentModeMover.SpeedRotation * Time.fixedDeltaTime);
    }

    private void OnCreatedStep()
    {
        StepTook?.Invoke();
    }

    private void OnChangedModeMover(IModeMoverProvider modeMoverProvider)
    {
        if (modeMoverProvider == null)
        {
            _currentModeMover = _defaultModeMover;
            return;
        }

        _currentModeMover = modeMoverProvider.ModeMover;
    }
}
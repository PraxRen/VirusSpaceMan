using System;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class Mover : MonoBehaviour, IMover
{
    [SerializeField] private bool _canLookAtMoveDirection;
    [SerializeField] private float _speedMove;
    [SerializeField] private float _speedRotation;
    [SerializeField] private float _gravity;
    [SerializeField][SerializeInterface(typeof(IStepNotifier))] private MonoBehaviour _stepNotifierMonoBehaviour;

    private Transform _transform;
    private CharacterController _characterController;
    private IStepNotifier _stepNotifier;
    private Vector2 _inputMoveDirection;

    public event Action StepTook;

    public Vector3 Velocity { get; private set; }

    private void Awake()
    {
        _transform = transform;
        _characterController = GetComponent<CharacterController>();
        _stepNotifier = (IStepNotifier)_stepNotifierMonoBehaviour;
    }

    private void OnEnable()
    {
        _stepNotifier.CreatedStep += OnCreatedStep;
    }

    private void OnDisable()
    {
        _stepNotifier.CreatedStep -= OnCreatedStep;
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
        Velocity = new Vector3(_inputMoveDirection.x * _speedMove, _gravity, _inputMoveDirection.y * _speedMove);
        _characterController.Move(Velocity * Time.fixedDeltaTime);

        if (_canLookAtMoveDirection && _inputMoveDirection != Vector2.zero)
            _transform.forward = new Vector3(_inputMoveDirection.x, 0f, _inputMoveDirection.y).normalized * _speedRotation * Time.fixedDeltaTime;
    }

    private void OnCreatedStep()
    {
        StepTook?.Invoke();
    }
}
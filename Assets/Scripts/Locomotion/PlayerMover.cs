using System;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMover : MonoBehaviour, IMover
{
    [SerializeField] private bool _canLookAtMoveDirection;
    [SerializeField] private float _speedMove;
    [SerializeField] private float _speedRotation;
    [SerializeField] private float _gravity;
    [SerializeField][SerializeInterface(typeof(ICreatorStep))] private GameObject _creatorStepObject;

    private Transform _transform;
    private CharacterController _characterController;
    private ICreatorStep _creatorStep;
    private Vector3 _inputMoveDirection;
    private Vector3 _horizontalVelocity;
    private Vector3 _verticalVelocity;

    public event Action StepTook;

    public float Speed { get; private set; }

    private void Awake()
    {
        _transform = transform;
        _characterController = GetComponent<CharacterController>();
        _creatorStep = _creatorStepObject.GetComponent<ICreatorStep>();
    }

    private void OnEnable()
    {
        _creatorStep.CreatedStep += OnCreatedStep;
    }

    private void OnDisable()
    {
        _creatorStep.CreatedStep -= OnCreatedStep;
    }

    private void FixedUpdate()
    {
        CalculateVelocity();
        HandleMove();
    }

    public void Move(Vector2 direction)
    {
        _inputMoveDirection = new Vector3(direction.x, 0f, direction.y);
    }

    private void CalculateVelocity()
    {
        Speed = _inputMoveDirection.magnitude * _speedMove;
        _horizontalVelocity = _inputMoveDirection * _speedMove * Time.fixedDeltaTime;
        _verticalVelocity = Vector3.up * _gravity * _speedMove * Time.fixedDeltaTime;
    }

    private void HandleMove()
    {
        _characterController.Move(_horizontalVelocity + _verticalVelocity);

        if (_canLookAtMoveDirection && _horizontalVelocity != Vector3.zero)
            _transform.forward = _inputMoveDirection * _speedRotation * Time.fixedDeltaTime;
    }

    private void OnCreatedStep()
    {
        StepTook?.Invoke();
    }
}
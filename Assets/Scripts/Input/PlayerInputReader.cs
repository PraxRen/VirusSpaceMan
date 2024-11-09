using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputReader : MonoBehaviour, PlayerInput.IPlayerActions
{
    private PlayerInput _playerInput;

    public event Action<float> ChangedScrollTarget;

    public Vector2 DirectionMove { get; private set; }
    public float ScrollTarget { get; private set; } 

    private void Awake()
    {
        _playerInput = new PlayerInput();
        _playerInput.Player.SetCallbacks(this);
    }

    private void OnEnable()
    {
        _playerInput.Player.Enable();
    }

    private void OnDisable()
    {
        _playerInput.Player.Disable();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        DirectionMove = context.ReadValue<Vector2>();
    }

    public void OnScrollTarget(InputAction.CallbackContext context)
    {
        if (context.performed == false || context.canceled)
            return;

        ScrollTarget = context.ReadValue<Vector2>().y;
        ChangedScrollTarget?.Invoke(ScrollTarget);
    }
}

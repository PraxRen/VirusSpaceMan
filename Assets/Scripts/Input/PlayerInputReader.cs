using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputReader : MonoBehaviour, PlayerInput.IPlayerActions
{
    private PlayerInput _playerInput;

    public event Action ChangedScrollNextTarget;
    public event Action ChangedScrollPreviousTarget;
    public event Action CanceledScrollTarget;
    public event Action DownCancel;
    public event Action UpCancel;

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
        if (context.started)
            return;

        ScrollTarget = context.ReadValue<Vector2>().y;

        if (ScrollTarget == 0)
        {
            CanceledScrollTarget?.Invoke();
            return;
        }

        bool isNext = ScrollTarget > 0;

        if (isNext)
            ChangedScrollNextTarget?.Invoke();
        else
            ChangedScrollPreviousTarget?.Invoke();
    }

    public void OnCancel(InputAction.CallbackContext context)
    {
        if (context.performed)
            DownCancel?.Invoke();
        else if (context.canceled)
            UpCancel?.Invoke();
    }
}
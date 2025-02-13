using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputReader : MonoBehaviour, PlayerInput.IPlayerActions
{
    private PlayerInput _playerInput;

    public event Action BeforeScrollNextTarget;
    public event Action ScrollNextTarget;
    public event Action BeforeScrollPreviousTarget;
    public event Action ScrollPreviousTarget;
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

        bool isStarted = context.performed;
        bool isNext = ScrollTarget > 0;

        if (isStarted == false)
        {
            if (isNext)
                ScrollNextTarget?.Invoke();
            else
                ScrollPreviousTarget?.Invoke();

            ScrollTarget = 0;
            return;
        }

        ScrollTarget = context.ReadValue<Vector2>().y;
        isNext = ScrollTarget > 0;

        if (isNext)
            BeforeScrollNextTarget?.Invoke();
        else
            BeforeScrollPreviousTarget?.Invoke();
    }

    public void OnCancel(InputAction.CallbackContext context)
    {
        if (context.performed)
            DownCancel?.Invoke();
        else if (context.canceled)
            UpCancel?.Invoke();
    }
}
using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class ShopInputReader : MonoBehaviour, PlayerInput.IShopActions
{
    private PlayerInput _playerInput;

    public event Action ChangedScrollNextItem;
    public event Action ChangedScrollPreviousItem;
    public event Action CanceledScrollItem;
    public event Action DownCancel;
    public event Action UpCancel;

    public float ScrollTarget { get; private set; }

    private void Awake()
    {
        _playerInput = new PlayerInput();
        _playerInput.Shop.SetCallbacks(this);
    }

    private void OnEnable()
    {
        _playerInput.Shop.Enable();
    }

    private void OnDisable()
    {
        _playerInput.Shop.Disable();
    }

    public void OnScrollSaleItem(InputAction.CallbackContext context)
    {
        if (context.started)
            return;

        ScrollTarget = context.ReadValue<Vector2>().y;

        if (ScrollTarget == 0)
        {
            CanceledScrollItem?.Invoke();
            return;
        }

        bool isNext = ScrollTarget > 0;

        if (isNext)
            ChangedScrollNextItem?.Invoke();
        else
            ChangedScrollPreviousItem?.Invoke();
    }

    public void OnCancel(InputAction.CallbackContext context)
    {
        if (context.performed)
            DownCancel?.Invoke();
        else if (context.canceled)
            UpCancel?.Invoke();
    }
}
using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class ShopInputReader : MonoBehaviour, PlayerInput.IShopActions
{
    private PlayerInput _playerInput;

    public event Action BeforeScrollNextItem;
    public event Action ScrollNextItem;
    public event Action BeforeScrollPreviousItem;
    public event Action ScrollPreviousItem;
    public event Action BeforeCancel;
    public event Action Cancel;    
    public event Action BeforePayOne;
    public event Action PayOne;
    public event Action BeforePayTwo;
    public event Action PayTwo;

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

        bool isStarted = context.performed;
        bool isNext = ScrollTarget > 0;

        if (isStarted == false)
        {
            if (isNext)
                ScrollNextItem?.Invoke();
            else
                ScrollPreviousItem?.Invoke();

            ScrollTarget = 0;
            return;
        }

        ScrollTarget = context.ReadValue<Vector2>().y;
        isNext = ScrollTarget > 0;

        if (isNext)
            BeforeScrollNextItem?.Invoke();
        else
            BeforeScrollPreviousItem?.Invoke();
    }

    public void OnCancel(InputAction.CallbackContext context)
    {
        if (context.performed)
            BeforeCancel?.Invoke();
        else if (context.canceled)
            Cancel?.Invoke();
    }

    public void OnPay_One(InputAction.CallbackContext context)
    {
        if (context.performed)
            BeforePayOne?.Invoke();
        else if (context.canceled)
            PayOne?.Invoke();
    }

    public void OnPay_Two(InputAction.CallbackContext context)
    {
        if (context.performed)
            BeforePayTwo?.Invoke();
        else if (context.canceled)
            PayTwo?.Invoke();
    }
}
using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class ShopInputReader : MonoBehaviour, PlayerInput.IShopActions
{
    private PlayerInput _playerInput;

    public event Action ChangedScrollNextTarget;
    public event Action ChangedScrollPreviousTarget;
    public event Action CanceledScrollTarget;

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
            CanceledScrollTarget?.Invoke();
            return;
        }

        bool isNext = ScrollTarget > 0;

        if (isNext)
            ChangedScrollNextTarget?.Invoke();
        else
            ChangedScrollPreviousTarget?.Invoke();
    }
}
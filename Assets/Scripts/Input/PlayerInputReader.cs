using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputReader : MonoBehaviour, PlayerInput.IPlayerActions
{
    private PlayerInput _playerInput;

    public Vector2 DirectionMove { get; private set; }

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
}

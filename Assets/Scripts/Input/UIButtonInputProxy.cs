using UnityEngine;
using UnityEngine.InputSystem;

public class UIButtonInputProxy : MonoBehaviour
{
    [Header("Button")]
    [SerializeField] private UICastomButton _targetButton;

    [Header("Input Action")]
    [SerializeField] private InputActionAsset _inputActionAsset;
    [SerializeField] private string _actionMapName;
    [SerializeField] private string _actionName;

    private InputAction _inputAction;

    private void OnEnable()
    {
        if (_inputActionAsset == null)
        {
            Debug.LogError("InputActionAsset is not assigned!");
            return;
        }

        var actionMap = _inputActionAsset.FindActionMap(_actionMapName);

        if (actionMap == null)
        {
            Debug.LogError($"Action Map '{_actionMapName}' not found in InputActionAsset.");
            return;
        }

        _inputAction = actionMap.FindAction(_actionName);

        if (_inputAction == null)
        {
            Debug.LogError($"Action '{_actionName}' not found in Action Map '{_actionMapName}'.");
            return;
        }

        _inputAction.started += OnActionStarted;
        _inputAction.canceled += OnActionCanceled;
        _inputAction.Enable();
    }

    private void OnDisable()
    {
        if (_inputAction == null)
            return;

        _inputAction.started -= OnActionStarted;
        _inputAction.canceled -= OnActionCanceled;
        _inputAction.Disable();
    }

    private void OnActionStarted(InputAction.CallbackContext context)
    {
        if (_targetButton == null)
            return;

        _targetButton.Down();
    }

    private void OnActionCanceled(InputAction.CallbackContext context)
    {
        if (_targetButton == null)
            return;

        _targetButton.Up(true);
    }
}
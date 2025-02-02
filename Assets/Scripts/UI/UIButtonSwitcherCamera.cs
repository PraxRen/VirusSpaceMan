using Cinemachine;
using UnityEngine;

[RequireComponent(typeof(IReadOnlyButton))]
public class UIButtonSwitcherCamera : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera _currentCamera;
    [SerializeField] private CinemachineVirtualCamera _targetCamera;

    private IReadOnlyButton _button;

    private void Awake()
    {
        _button = GetComponent<IReadOnlyButton>();
    }

    private void OnEnable()
    {
        _button.ClickUpInBounds += OnClickUpInBounds;
    }

    private void OnDisable()
    {
        _button.ClickUp -= OnClickUpInBounds;
    }

    private void OnClickUpInBounds()
    {
        _currentCamera.enabled = false;

        if (_targetCamera != null)
            _targetCamera.enabled = true;
    }
}
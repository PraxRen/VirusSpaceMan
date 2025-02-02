using System;
using UnityEngine;

[RequireComponent(typeof(IReadOnlyButton))]
public class UIButtonSwitcherScene : MonoBehaviour
{
    [SerializeField] private TypeScene _currentScene;
    [SerializeField] private TypeScene _targetScene;

    private IReadOnlyButton _button;

    public event Action<bool> Clicked;

    private void Awake() => _button = GetComponent<IReadOnlyButton>();

    private void OnEnable() => _button.ClickUpInBounds += OnClickUpInBounds;

    private void OnDisable() => _button.ClickUp -= OnClickUpInBounds;

    private void OnClickUpInBounds() 
    {
        if (_currentScene == _targetScene)
        {
            Clicked?.Invoke(false);
            return;
        }

        Clicked?.Invoke(true);
        SceneLoader.Load(_targetScene);
    } 
}
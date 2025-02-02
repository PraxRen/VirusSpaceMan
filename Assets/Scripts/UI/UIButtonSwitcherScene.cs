using System;
using UnityEngine;

[RequireComponent(typeof(IReadOnlyButton))]
public class UIButtonSwitcherScene : MonoBehaviour
{
    [SerializeField] private TypeScene _scene;

    private IReadOnlyButton _button;

    private void Awake() => _button = GetComponent<IReadOnlyButton>();

    private void OnEnable() => _button.ClickUpInBounds += OnClickUpInBounds;

    private void OnDisable() => _button.ClickUp -= OnClickUpInBounds;

    private void OnClickUpInBounds() => SceneLoader.Load(_scene);
}
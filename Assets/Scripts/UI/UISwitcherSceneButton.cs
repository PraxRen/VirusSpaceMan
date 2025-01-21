using UnityEngine;

[RequireComponent(typeof(IReadOnlyButton))]
public class UISwitcherSceneButton : MonoBehaviour
{
    [SerializeField] private TypeScene _typeScene;

    private IReadOnlyButton _button;

    private void Awake() => _button = GetComponent<IReadOnlyButton>();

    private void OnEnable() => _button.ClickUpInBounds += OnClickUpInBounds;

    private void OnDisable() => _button.ClickUp -= OnClickUpInBounds;

    private void OnClickUpInBounds() => SceneLoader.Load(_typeScene);
}
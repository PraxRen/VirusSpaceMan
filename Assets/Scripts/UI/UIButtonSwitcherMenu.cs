using UnityEngine;

[RequireComponent(typeof(IReadOnlyButton))]
public class UIButtonSwitcherMenu : MonoBehaviour
{
    [SerializeField][SerializeInterface(typeof(IMenu))] private MonoBehaviour _currentMenuMonoBehaviour;
    [SerializeField][SerializeInterface(typeof(IMenu))] private MonoBehaviour _targetMonoBehaviour;

    private IMenu _currentMenu;
    private IMenu _targetMenu;
    private IReadOnlyButton _button;

    private void Awake()
    {
        _currentMenu = (IMenu)_currentMenuMonoBehaviour;
        _targetMenu = (IMenu)_targetMonoBehaviour;
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
        _currentMenu.Deactivate();
        _targetMenu.Activate();
    }
}
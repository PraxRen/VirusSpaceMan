using UnityEngine;

[RequireComponent(typeof(IReadOnlyButton))]
public class UISwitcherMenu : MonoBehaviour
{
    [SerializeField][SerializeInterface(typeof(IMenu))] private MonoBehaviour _menuMonoBehaviour;

    private IMenu _menu;
    private IReadOnlyButton _button;

    private void Awake()
    {
        _menu = (IMenu)_menuMonoBehaviour;
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
        _menu.Activate();
    }
}
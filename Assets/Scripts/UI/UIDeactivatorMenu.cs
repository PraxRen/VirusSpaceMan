using UnityEngine;

public class UIDeactivatorMenu : MonoBehaviour
{
    [SerializeField][SerializeInterface(typeof(IMenu))] private MonoBehaviour _menuMonoBehaviour;
    [SerializeField] private UIButtonSwitcherScene _buttonSwitcherScene;

    private IMenu _menu;

    private void Awake()
    {
        _menu = (IMenu)_menuMonoBehaviour;
    }

    private void OnEnable()
    {
        _buttonSwitcherScene.Clicked += OnClicked;
    }

    private void OnDisable()
    {
        _buttonSwitcherScene.Clicked -= OnClicked;
    }

    private void OnClicked(bool isClicked)
    {
        if (isClicked) 
            return;

        _menu.Deactivate();
    }
}

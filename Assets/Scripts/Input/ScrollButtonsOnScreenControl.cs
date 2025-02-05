using UnityEngine;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.OnScreen;

public class ScrollButtonsOnScreenControl : OnScreenControl
{
    [InputControl(layout = "Vector2")][SerializeField] private string _controlPath;
    [SerializeField] private UICastomButton _buttonLeft;
    [SerializeField] private UICastomButton _buttonRight;

    protected override string controlPathInternal
    {
        get => _controlPath;
        set => _controlPath = value;
    }

    protected override void OnEnable()
    {
        _buttonLeft.ClickDown += OnClickButtonLeft;
        _buttonRight.ClickDown += OnClickButtonRight;
        base.OnEnable();
    }

    protected override void OnDisable()
    {
        _buttonLeft.ClickDown -= OnClickButtonLeft;
        _buttonRight.ClickDown -= OnClickButtonRight;
        SendValueToControl(Vector2.zero);
        base.OnDisable();
    }

    private void OnClickButtonRight()
    {
        SendValueToControl(Vector2.up);
    }

    private void OnClickButtonLeft()
    {
        SendValueToControl(Vector2.down);
    }
}
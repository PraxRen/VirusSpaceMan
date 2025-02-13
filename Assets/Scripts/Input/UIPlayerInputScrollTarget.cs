using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPlayerInputScrollTarget : MonoBehaviour
{
    [SerializeField] private PlayerInputReader _playerInputReader;
    [SerializeField] private UICastomButton _buttonNext;
    [SerializeField] private UICastomButton _buttonPrevious;

    private void OnEnable()
    {
        _playerInputReader.BeforeScrollNextTarget += OnBeforeScrollNextTarget;
        _playerInputReader.ScrollNextTarget += OnScrollNextTarget;
        _playerInputReader.BeforeScrollPreviousTarget += OnBeforeScrollPreviousTarget;
        _playerInputReader.ScrollPreviousTarget += OnScrollPreviousTarget;
    }

    private void OnDisable()
    {
        _playerInputReader.BeforeScrollNextTarget -= OnBeforeScrollNextTarget;
        _playerInputReader.ScrollNextTarget -= OnScrollNextTarget;
        _playerInputReader.BeforeScrollPreviousTarget -= OnBeforeScrollPreviousTarget;
        _playerInputReader.ScrollPreviousTarget -= OnScrollPreviousTarget;
    }

    private void OnBeforeScrollNextTarget()
    {
        _buttonNext.Down();
    }

    private void OnScrollNextTarget()
    {
        _buttonNext.Up(true);
    }

    private void OnBeforeScrollPreviousTarget()
    {
        _buttonPrevious.Down();
    }

    private void OnScrollPreviousTarget()
    {
        _buttonPrevious.Up(true);
    }
}

using System;
using UnityEngine;

public class ActionScheduler : MonoBehaviour
{
    [SerializeField][SerializeInterface(typeof(IMatrixActionReanOnly))] private ScriptableObject _matrixActionScriptableObject;

    private IMatrixActionReanOnly _matrixAction;
    private IAction _currentAction;
    private bool _isBlockAction;

    private void Awake()
    {
        if (_matrixAction != null)
            return;

        _matrixAction = (IMatrixActionReanOnly)_matrixActionScriptableObject;
    }

    public bool CanStartAction(IAction action)
    {
        if (_currentAction == null)
            return true;

        if (_currentAction == action)
            return true;

        if (_isBlockAction)
        {
            return _matrixAction.CanUnblock(_currentAction, action) == false;
        }

        return true;
    }

    public void StartAction(IAction action)
    {
        if (_currentAction == action)
            return;

        if (_currentAction != null)
        {
            if (_matrixAction.CanCancel(_currentAction, action))
            {
                _currentAction.Cancel();
            }

            ClearAction(_currentAction);
        }

        _currentAction = action;
    }

    public void ClearAction(IAction action)
    {
        if (action != _currentAction)
            return;

        _currentAction = null;
        _isBlockAction = false;
    }

    public void SetBlock(IAction action)
    {
        if (action != _currentAction)
            throw new InvalidOperationException($"Нельзя установить блокировку для действия которое не является текущим действием!");

        _isBlockAction = true;
    }
}
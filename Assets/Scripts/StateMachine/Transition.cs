using System;
using UnityEngine;

public abstract class Transition : MonoBehaviour, IReadOnlyTransition
{
    [SerializeField] private State _targetState;

    private State _currentState;

    public event Action<IReadOnlyTransition, StatusTransition> ChangedStatus;

    public StatusTransition Status { get; private set; }
    public IReadOnlyState CurrentState => _currentState;
    public IReadOnlyState TargetState => _targetState;
    protected Character Character { get; private set; }

    public void Initialize(Character character, State currentState)
    {
        if (Status != StatusTransition.None)
            throw new InvalidOperationException($"������ ������������� \"{nameof(Transition)}\"! \"{GetType().Name}\" � \"{nameof(State)} -{_currentState.GetType().Name}\" ��� ���������������.");

        Character = character ?? throw new ArgumentNullException(nameof(character));
        _currentState = currentState ?? throw new ArgumentNullException(nameof(currentState));
        InitializeAddon();
        UpdateStatusTransition(StatusTransition.Initialized);
    }

    public void Activate()
    {
        if ((int)Status < (int)StatusTransition.Initialized)
        {
            string message = $"������ ��������� \"{nameof(Transition)}\"! \"{GetType().Name}\" �� ���������������.";
            throw new InvalidOperationException(message);
        }

        enabled = true;
        ActivateAddon();
        UpdateStatusTransition(StatusTransition.Activated);
    }

    public void Deactivate()
    {
        if ((int)Status < (int)StatusTransition.Activated)
        {
            string message = $"������ ����������� \"{nameof(Transition)}\"! \"{GetType().Name}\" �� �������.";
            throw new InvalidOperationException(message);
        }

        DeactivateAddon();
        UpdateStatusTransition(StatusTransition.Deactivated);
        enabled = false;
    }

    protected void SetNeedTransit() => UpdateStatusTransition(StatusTransition.NeedTransit);

    protected virtual void InitializeAddon() { }

    protected virtual void ActivateAddon() { }

    protected virtual void DeactivateAddon() { }

    private void UpdateStatusTransition(StatusTransition status)
    {
        if (status == Status)
            return;

        Status = status;
        ChangedStatus?.Invoke(this, Status);
    }
}
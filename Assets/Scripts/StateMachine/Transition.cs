using System;
using UnityEngine;

public abstract class Transition : IReadOnlyTransition
{
    private readonly State _currentState;
    private readonly State _targetState;


    public event Action<IReadOnlyTransition, StatusTransition> ChangedStatus;

    public StatusTransition Status { get; private set; }
    public IReadOnlyState CurrentState => _currentState;
    public IReadOnlyState TargetState => _targetState;
    protected Character Character { get; }

    public Transition(Character character, State currentState, State targetState)
    {
        Character = character ?? throw new ArgumentNullException(nameof(character));
        _currentState = currentState ?? throw new ArgumentNullException(nameof(currentState));
        _targetState = targetState ?? throw new ArgumentNullException(nameof(targetState));
    }

    public void Activate()
    {
        if (Status != StatusTransition.Initialized && Status != StatusTransition.Deactivated)
            throw new InvalidOperationException($"Ошибка активации \"{nameof(Transition)}\" \"{_currentState.GetType().Name}\"/\"{GetType().Name}\". Текущий статус -> \"{Status}\"!");

        ActivateAddon();

        if (Status == StatusTransition.NeedTransit)
            return;

        UpdateStatusTransition(StatusTransition.Activated);
    }

    public void Deactivate()
    {
        if ((int)Status < (int)StatusTransition.Activated)
        {
            string message = $"Ошибка деактивации \"{nameof(Transition)}\"! \"{GetType().Name}\" не активен.";
            throw new InvalidOperationException(message);
        }

        DeactivateAddon();
        UpdateStatusTransition(StatusTransition.Deactivated);
    }

    public virtual void Tick(float deltaTime) { }

    protected void SetNeedTransit() 
    {
        if (Status == StatusTransition.Initialized || Status == StatusTransition.Deactivated)
            UpdateStatusTransition(StatusTransition.Activated);

        UpdateStatusTransition(StatusTransition.NeedTransit);
    } 

    protected virtual void ActivateAddon() { }

    protected virtual void DeactivateAddon() { }

    private void UpdateStatusTransition(StatusTransition status)
    {
        if (status == Status)
            return;

        //Debug.Log($"UpdateStatusTransition: \"{_currentState.GetType().Name}\"/\"{GetType().Name}\" | {Status} -> {status}");
        Status = status;
        ChangedStatus?.Invoke(this, Status);
    }
}
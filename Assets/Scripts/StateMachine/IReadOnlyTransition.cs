using System;

public interface IReadOnlyTransition
{
    public event Action<IReadOnlyTransition, StatusTransition> ChangedStatus;

    public StatusTransition Status { get; }
    public IReadOnlyState CurrentState { get; }
    public IReadOnlyState TargetState { get; }
}
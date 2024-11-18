using System;

public interface IReadOnlyState
{
    event Action<StatusState> ChangedStatus;
    event Action<IReadOnlyState> GetedNextState;

    string Id { get; }
    StatusState Status { get; }
}
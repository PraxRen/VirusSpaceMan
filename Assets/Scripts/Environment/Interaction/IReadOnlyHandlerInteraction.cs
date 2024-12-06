using System;

public interface IReadOnlyHandlerInteraction
{
    bool IsActive { get; }
    IReadOnlyObjectInteraction ObjectInteraction { get; }

    event Action<IReadOnlyObjectInteraction> StartedInteract;
    event Action<IReadOnlyObjectInteraction> Interacted;
    event Action<IReadOnlyObjectInteraction> StoppedInteract;
}
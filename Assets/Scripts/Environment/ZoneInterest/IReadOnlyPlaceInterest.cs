using System;

public interface IReadOnlyPlaceInterest : ITarget
{
    event Action EnteredHandlerInteraction;

    string Name { get; }
    bool IsEmpty { get; }
    bool HasHandlerInteractionInside { get; }
    IReadOnlyInteractor HandlerInteraction { get; }

    void SetHandlerInteraction(IReadOnlyInteractor handlerInteraction);
    bool TryGetObjectInteraction(IReadOnlyInteractor handlerInteraction, out IObjectInteraction objectInteraction);
}
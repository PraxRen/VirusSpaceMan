using System;

public interface IReadOnlyPlaceInterest : ITarget
{
    event Action EnteredHandlerInteraction;

    bool IsEmpty { get; }
    bool HasHandlerInteractionInside { get; }
    IReadOnlyHandlerInteraction HandlerInteraction { get; }

    void SetHandlerInteraction(IReadOnlyHandlerInteraction handlerInteraction);
    bool TryGetObjectInteraction(IReadOnlyHandlerInteraction handlerInteraction, out IObjectInteraction objectInteraction);
}
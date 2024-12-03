using System;

public interface IReadOnlyPlaceInterest : ITarget
{
    event Action EnteredHandlerInteraction;

    bool IsEmpty { get; }
    bool HasHandlerInteractionInside { get; }
    HandlerInteraction HandlerInteraction { get; }
}
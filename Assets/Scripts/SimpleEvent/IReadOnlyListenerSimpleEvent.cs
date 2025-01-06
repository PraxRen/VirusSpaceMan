using System;

public interface IReadOnlyListenerSimpleEvent
{
    event Action<IReadOnlyCreatorSimpleEvent, ISimpleEventInitiator, SimpleEvent> Notified;
}
using System;

public interface IReadOnlyListenerSimpleEvent
{
    event Action<ISimpleEventCreator, ISimpleEventInitiator, SimpleEvent> BeforeNotified;
    event Action<ISimpleEventCreator, ISimpleEventInitiator, SimpleEvent> Notified;
}
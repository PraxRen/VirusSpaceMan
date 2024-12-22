using System;

public interface ISimpleEventInitiator
{
    event Action<ISimpleEventInitiator, SimpleEvent> SimpleEventStarted;
}
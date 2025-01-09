using System;
using UnityEngine;

public class ListenerSimpleEvent : MonoBehaviour, IReadOnlyListenerSimpleEvent
{
    public event Action<IReadOnlyCreatorSimpleEvent, ISimpleEventInitiator, SimpleEvent> BeforeNotified;
    public event Action<IReadOnlyCreatorSimpleEvent, ISimpleEventInitiator, SimpleEvent> Notified;

    public void Notify(IReadOnlyCreatorSimpleEvent creatorSimpleEvent, ISimpleEventInitiator initiator, SimpleEvent simpleEvent)
    {
        Debug.Log($"ListenerSimpleEvent.Notify: {((CreatorSimpleEvent)creatorSimpleEvent).transform.parent.name} | {transform.parent.name} | {simpleEvent.Type}");
        BeforeNotified?.Invoke(creatorSimpleEvent, initiator, simpleEvent);
        Notified?.Invoke(creatorSimpleEvent, initiator, simpleEvent);
    }
}
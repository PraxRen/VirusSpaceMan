using System;
using UnityEngine;

public class ListenerSimpleEvent : MonoBehaviour, IReadOnlyListenerSimpleEvent
{
    public event Action<IReadOnlyCreatorSimpleEvent, ISimpleEventInitiator, SimpleEvent> Notified;

    public void Notify(IReadOnlyCreatorSimpleEvent creatorSimpleEvent, ISimpleEventInitiator initiator, SimpleEvent simpleEvent)
    {
        Debug.Log($"ListenerSimpleEvent.Notify: {((CreatorSimpleEvent)creatorSimpleEvent).transform.parent.name} | {transform.parent.name} | {simpleEvent.Type}");
        Notified?.Invoke(creatorSimpleEvent, initiator, simpleEvent);
    }
}
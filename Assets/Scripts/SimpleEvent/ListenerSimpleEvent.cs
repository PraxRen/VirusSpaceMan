using System;
using UnityEngine;

public class ListenerSimpleEvent : MonoBehaviour
{
    public event Action<IReadOnlyCreatorSimpleEvent, ISimpleEventInitiator, SimpleEvent> Notified;

    public void Notify(IReadOnlyCreatorSimpleEvent creatorSimpleEvent, ISimpleEventInitiator initiator, SimpleEvent simpleEvent)
    {
        Notified?.Invoke(creatorSimpleEvent, initiator, simpleEvent);
    }
}
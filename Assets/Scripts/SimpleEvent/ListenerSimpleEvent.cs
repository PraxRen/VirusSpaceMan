using System;
using System.Collections.Generic;
using UnityEngine;

public class ListenerSimpleEvent : MonoBehaviour, IReadOnlyListenerSimpleEvent
{
    [SerializeField] private List<TypeSimpleEvent> _supportTypes;

    public event Action<ISimpleEventCreator, ISimpleEventInitiator, SimpleEvent> BeforeNotified;
    public event Action<ISimpleEventCreator, ISimpleEventInitiator, SimpleEvent> Notified;

    public void Notify(ISimpleEventCreator creatorSimpleEvent, ISimpleEventInitiator initiator, SimpleEvent simpleEvent)
    {
        if (_supportTypes.Contains(simpleEvent.Type) == false)
            return;

#if UNITY_EDITOR
        //Debug.Log($"ListenerSimpleEvent.Notify: {((CreatorSimpleEvent)creatorSimpleEvent).transform.parent.name} | {transform.parent.name} | {simpleEvent.Type}");
#endif
        BeforeNotified?.Invoke(creatorSimpleEvent, initiator, simpleEvent);
        Notified?.Invoke(creatorSimpleEvent, initiator, simpleEvent);
    }

    public void AddSupportType(TypeSimpleEvent typeSimpleEvent)
    {
        if (_supportTypes.Contains(typeSimpleEvent))
            throw new InvalidOperationException($"{transform.parent.name}: There is already support for this TypeSimpleEvent \"{typeSimpleEvent}\".");

        _supportTypes.Add(typeSimpleEvent);
    }

    public void RemoveSupportType(TypeSimpleEvent typeSimpleEvent)
    {
        if (_supportTypes.Contains(typeSimpleEvent) == false)
            throw new InvalidOperationException($"{transform.parent.name}: This TypeSimpleEvent \"{typeSimpleEvent}\" is not supported");

        _supportTypes.Remove(typeSimpleEvent);
    }
}
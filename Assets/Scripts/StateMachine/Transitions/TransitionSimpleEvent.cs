using System;
using UnityEngine;

public class TransitionSimpleEvent : Transition
{
    private readonly IReadOnlyListenerSimpleEvent _listenerSimpleEvent;
    private readonly TypeSimpleEvent _typeSimpleAction;

    public TransitionSimpleEvent(Character character, State currentState, State targetState, TypeSimpleEvent typeSimpleAction) : base(character, currentState, targetState)
    {
        if (character.TryGetComponent(out _listenerSimpleEvent) == false)
            throw new InvalidOperationException($"The component \"{nameof(IReadOnlyListenerSimpleEvent)}\" required for operation \"{GetType().Name}\".");

        _typeSimpleAction = typeSimpleAction;
    }

    protected override void ActivateAddon()
    {
        _listenerSimpleEvent.Notified += OnNotified;
    }

    protected override void DeactivateAddon() 
    {
        _listenerSimpleEvent.Notified -= OnNotified;
    }

    private void OnNotified(IReadOnlyCreatorSimpleEvent creatorSimpleEvent, ISimpleEventInitiator simpleEventInitiator, SimpleEvent simpleEvent)
    {
        if (simpleEvent.Type != _typeSimpleAction)
            return;

        Debug.Log($"TransitionSimpleEvent: {Character.Transform.parent.name}");
        SetNeedTransit();
    }
}   
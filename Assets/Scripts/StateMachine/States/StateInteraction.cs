using System;

public class StateInteraction : State
{
    private HandlerInteraction _handlerInteraction;
    private IObjectInteraction _currentObjectInteraction;

    public StateInteraction(string id, AICharacter character, float timeSecondsWaitHandle) : base(id, character, timeSecondsWaitHandle)
    {
        if (character.TryGetComponent(out _handlerInteraction) == false)
            throw new InvalidOperationException($"Initialization error \"{nameof(State)}\"! The component \"{nameof(HandlerInteraction)}\" required for operation \"{GetType().Name}\".");
    }

    public override void Update()
    {
        if (_handlerInteraction.CanStartInteract(_currentObjectInteraction) == false)
            return;

        _handlerInteraction.StartInteract(_currentObjectInteraction);
    }

    protected override void EnterBeforeAddon()
    {
        _currentObjectInteraction = GetObjectInteraction();

        if (_currentObjectInteraction == null)
            throw new InvalidOperationException($"Not find \"{nameof(IObjectInteraction)}\" for \"{nameof(StateInteraction)}\".");

        _handlerInteraction.StoppedInteract += OnStoppedInteract;
    }

    protected override void ExitAfterAddon()
    {
        _currentObjectInteraction = null;
        _handlerInteraction.StoppedInteract -= OnStoppedInteract;
    }

    private IObjectInteraction GetObjectInteraction() 
    {
        IObjectInteraction result = null;
        IReadOnlyPlaceInterest placeInterest = Character.MoveTracker.Target as IReadOnlyPlaceInterest;

        if (placeInterest != null && placeInterest.TryGetObjectInteraction(_handlerInteraction, out result))
            return result;
            
        return result;
    }

    private void OnStoppedInteract(IReadOnlyObjectInteraction objectInteraction)
    {
        if (objectInteraction != _currentObjectInteraction)
            return;

        Complete();
    }
}
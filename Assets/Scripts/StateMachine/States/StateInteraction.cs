using System;

public class StateInteraction : State
{
    private Interactor _interactor;
    private IObjectInteraction _currentObjectInteraction;

    public StateInteraction(string id, AICharacter character, float timeSecondsWaitHandle) : base(id, character, timeSecondsWaitHandle)
    {
        if (character.TryGetComponent(out _interactor) == false)
            throw new InvalidOperationException($"Initialization error \"{nameof(State)}\"! The component \"{nameof(Interactor)}\" required for operation \"{GetType().Name}\".");
    }

    public override void Update()
    {
        if (_interactor.CanStartInteract(_currentObjectInteraction) == false)
            return;

        _interactor.StartInteract(_currentObjectInteraction);
    }

    protected override void EnterBeforeAddon()
    {
        _currentObjectInteraction = GetObjectInteraction();

        if (_currentObjectInteraction == null)
            throw new InvalidOperationException($"Not find \"{nameof(IObjectInteraction)}\" for \"{nameof(StateInteraction)}\".");

        _interactor.StoppedInteract += OnStoppedInteract;
    }

    protected override void ExitAfterAddon()
    {
        _currentObjectInteraction = null;
        _interactor.StoppedInteract -= OnStoppedInteract;
    }

    private IObjectInteraction GetObjectInteraction() 
    {
        IObjectInteraction result = null;
        IReadOnlyPlaceInterest placeInterest = Character.MoveTracker.Target as IReadOnlyPlaceInterest;

        if (placeInterest != null && placeInterest.TryGetObjectInteraction(_interactor, out result))
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
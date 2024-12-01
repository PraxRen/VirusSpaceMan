using System;

public class StatePatrol : State, IModeMoverProvider
{
    private readonly Patrol _patrol;
    private readonly ModeMover _patrolModeMover;

    public StatePatrol(string id, AICharacter character, float timeSecondsWaitHandle, ModeMover patrolModeMover) : base(id, character, timeSecondsWaitHandle)
    {
        if (character.TryGetComponent(out _patrol) == false)
            throw new InvalidOperationException($"Initialization error \"{nameof(State)}\"! The component \"{nameof(Patrol)}\" required for operation \"{GetType().Name}\".");

        _patrolModeMover = patrolModeMover;
    }

    public ModeMover DefaultModeMover => _patrolModeMover;
    public ModeMover ActiveModeMover => _patrolModeMover;

    protected override void EnterAfterAddon()
    {
        _patrol.Run();
    }

    protected override void ExitAfterAddon() 
    {
        _patrol.Clear();
    }
}
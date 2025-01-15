using System;

public class StatePatrol : State
{
    private readonly Patrol _patrol;

    public StatePatrol(string id, AICharacter character, float timeSecondsWaitHandle) : base(id, character, timeSecondsWaitHandle)
    {
        if (character.TryGetComponent(out _patrol) == false)
            throw new InvalidOperationException($"Initialization error \"{nameof(State)}\"! The component \"{nameof(Patrol)}\" required for operation \"{GetType().Name}\".");
    }

    protected override void EnterAddon()
    {
        _patrol.Run();
    }

    protected override void ExitAfterAddon() 
    {
        _patrol.Clear();
    }
}
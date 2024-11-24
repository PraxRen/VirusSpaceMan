using System;

public class StateMoverToTarget : State
{
    private Navigation _navigation;

    public StateMoverToTarget(string id, Character character, float timeSecondsWaitHandle) : base(id, character, timeSecondsWaitHandle)
    {
        if (character.TryGetComponent(out _navigation) == false)
            throw new InvalidOperationException($"Initialization error \"{nameof(State)}\"! The component \"{nameof(Navigation)}\" required for operation \"{GetType().Name}\".");
    }

    public override void Update()
    {
        _navigation.MoveTargetPosition(Character.MoveTracker.Position);
    }

    protected override void EnterAfterAddon()
    {
        _navigation.ResetPath();
    }

    protected override void ExitAfterAddon()
    {
        _navigation.Stop();
    }
}
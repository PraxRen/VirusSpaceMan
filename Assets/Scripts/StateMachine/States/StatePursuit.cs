using System;

public class StatePursuit : StateMoverToTarget, IModeMoverProvider
{
    private readonly ListenerSimpleEvent _listenerSimpleEvent;

    public StatePursuit(string id, AICharacter character, float timeSecondsWaitHandle, ModeMover modeMover) : base(id, character, timeSecondsWaitHandle) 
    {
        ModeMover = modeMover;

        if (character.TryGetComponent(out _listenerSimpleEvent) == false)
            throw new InvalidOperationException($"Initialization error \"{nameof(State)}\"! The component \"{nameof(ListenerSimpleEvent)}\" required for operation \"{GetType().Name}\".");
    }

    public ModeMover ModeMover { get; }

    protected override void EnterAddon()
    {
        _listenerSimpleEvent.RemoveSupportType(TypeSimpleEvent.Attack);
        base.EnterAddon();
    }

    protected override void ExitAfterAddon()
    {
        base.ExitAfterAddon();
        _listenerSimpleEvent.AddSupportType(TypeSimpleEvent.Attack);
    }
}
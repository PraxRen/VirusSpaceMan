using System;

public class TransitionRagdollActivate : Transition
{
    private readonly SwitcherRagdoll _switcherRagdoll;

    public TransitionRagdollActivate(Character character, State currentState, State targetState) : base(character, currentState, targetState)
    {
        if (character.TryGetComponent(out _switcherRagdoll) == false)
            throw new InvalidOperationException($"The component \"{nameof(SwitcherRagdoll)}\" required for operation \"{GetType().Name}\".");
    }

    protected override void ActivateAddon()
    {
        if (_switcherRagdoll.IsActivated)
        {
            SetNeedTransit();
            return;
        }

        _switcherRagdoll.Activated += SetNeedTransit;
    }

    protected override void DeactivateAddon()
    {
        _switcherRagdoll.Activated -= SetNeedTransit;
    }
}
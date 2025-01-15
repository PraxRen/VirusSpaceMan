using System;

public class TransitionRagdollDeactivate : Transition
{
    private readonly SwitcherRagdoll _switcherRagdoll;

    public TransitionRagdollDeactivate(Character character, State currentState, State targetState) : base(character, currentState, targetState)
    {
        if (character.TryGetComponent(out _switcherRagdoll) == false)
            throw new InvalidOperationException($"The component \"{nameof(SwitcherRagdoll)}\" required for operation \"{GetType().Name}\".");
    }

    protected override void ActivateAddon()
    {
        if (_switcherRagdoll.IsActivated == false)
        {
            SetNeedTransit();
            return;
        }

        _switcherRagdoll.Deactivated += SetNeedTransit;
    }

    protected override void DeactivateAddon() 
    {
        _switcherRagdoll.Deactivated -= SetNeedTransit;
    }
}
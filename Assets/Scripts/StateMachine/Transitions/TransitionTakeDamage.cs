using System;

public class TransitionTakeDamage : Transition
{
    private readonly IDamageable _mainDamageable;

    public TransitionTakeDamage(Character character, State currentState, State targetState) : base(character, currentState, targetState) 
    {
        if (character.TryGetComponent(out _mainDamageable) == false)
            throw new InvalidOperationException($"The component \"{nameof(IDamageable)}\" required for operation \"{GetType().Name}\".");
    }

    protected override void ActivateAddon()
    {
        _mainDamageable.AfterTakeDamage += OnAfterTakeDamage;
    }

    protected override void DeactivateAddon()
    {
        _mainDamageable.AfterTakeDamage -= OnAfterTakeDamage;
    }

    private void OnAfterTakeDamage(Hit hit, float damage) => SetNeedTransit();
}
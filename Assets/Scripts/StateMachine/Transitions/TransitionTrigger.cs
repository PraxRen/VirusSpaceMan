using System;
using UnityEngine;

public class TransitionTrigger : Transition
{
    private readonly IReadOnlyTrigger _triggerDamageable;

    public TransitionTrigger(Character character, State currentState, State targetState) : base(character, currentState, targetState) 
    {
        AICharacter aICharacter = character as AICharacter ?? throw new InvalidOperationException($"The component \"{nameof(Mover)}\" required for operation \"{GetType().Name}\".");
        _triggerDamageable = aICharacter.TriggerDamageable;
    }

    protected override void ActivateAddon()
    {
        if (_triggerDamageable.Target != null)
        {
            OnChangedTarget(_triggerDamageable.Target);
            return;
        }

        _triggerDamageable.ChangedTarget += OnChangedTarget;
    }

    protected override void DeactivateAddon()
    {
        _triggerDamageable.ChangedTarget -= OnChangedTarget;
    }

    private void OnChangedTarget(Collider collider)
    {
        if (collider.TryGetComponent(out IDamageable damageable) == false)
            return;

        SetNeedTransit();
    }
}
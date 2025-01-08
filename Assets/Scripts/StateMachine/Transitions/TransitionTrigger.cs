using UnityEngine;

public class TransitionTrigger : Transition
{
    public TransitionTrigger(Character character, State currentState, State targetState) : base(character, currentState, targetState) { }

    protected override void ActivateAddon()
    {
        if (Character.TriggerDamageable.Target != null)
        {
            OnChangedTarget(Character.TriggerDamageable.Target);
            return;
        }

        Character.TriggerDamageable.ChangedTarget += OnChangedTarget;
    }

    protected override void DeactivateAddon()
    {
        Character.TriggerDamageable.ChangedTarget -= OnChangedTarget;
    }

    private void OnChangedTarget(Collider collider)
    {
        if (collider.TryGetComponent(out IDamageable damageable) == false)
            return;

        SetNeedTransit();
    }
}
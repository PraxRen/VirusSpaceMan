using UnityEngine;

public class TransitionScannerDamageableChangeTarget : Transition
{
    public TransitionScannerDamageableChangeTarget(Character character, State currentState, State targetState) : base(character, currentState, targetState) { }

    protected override void ActivateAddon()
    {
        if (Character.ScannerDamageable.Target != null)
        {
            OnChangedCurrentTarget(Character.ScannerDamageable.Target);
            return;
        }

        Character.ScannerDamageable.ChangedCurrentTarget += OnChangedCurrentTarget;
    }

    protected override void DeactivateAddon()
    {
        Character.ScannerDamageable.ChangedCurrentTarget -= OnChangedCurrentTarget;
    }

    private void OnChangedCurrentTarget(Collider collider)
    {
        SetNeedTransit();
    }
}
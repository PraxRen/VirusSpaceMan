using UnityEngine;

public class TransitionScannerDamageableRemoveTarget : Transition
{
    public TransitionScannerDamageableRemoveTarget(Character character, State currentState, State targetState) : base(character, currentState, targetState) { }

    protected override void ActivateAddon()
    {
        Character.ScannerDamageable.RemovedCurrentTarget += OnRemovedCurrentTarget;
    }

    protected override void DeactivateAddon()
    {
        Character.ScannerDamageable.RemovedCurrentTarget -= OnRemovedCurrentTarget;
    }

    private void OnRemovedCurrentTarget(Collider collider)
    {
        SetNeedTransit();
    }
}
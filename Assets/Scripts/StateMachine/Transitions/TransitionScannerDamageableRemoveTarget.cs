public class TransitionScannerDamageableRemoveTarget : Transition
{
    public TransitionScannerDamageableRemoveTarget(Character character, State currentState, State targetState) : base(character, currentState, targetState) { }

    public override void Tick(float deltaTime)
    {
        if (Status == StatusTransition.NeedTransit)
            return;

        if (Character.ScannerDamageable.Target == null)
            SetNeedTransit();
    }
}
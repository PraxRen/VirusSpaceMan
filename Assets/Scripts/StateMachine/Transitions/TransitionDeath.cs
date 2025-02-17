public class TransitionDeath : Transition
{
    public TransitionDeath(Character character, State currentState, State targetState) : base(character, currentState, targetState) { }

    protected override void ActivateAddon()
    {
        if (Character.Health.IsDied)
        {
            SetNeedTransit();
            return;
        }

        Character.Health.Died += SetNeedTransit;
    }

    protected override void DeactivateAddon()
    {
        Character.Health.Died -= SetNeedTransit;
    }
}
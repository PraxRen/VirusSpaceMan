public class TransitionStateComplete : Transition
{
    public TransitionStateComplete(Character character, State currentState, State targetState) : base(character, currentState, targetState) { }

    protected override void ActivateAddon()
    {
        if (CurrentState.Status == StatusState.Completed)
        {
            OnChangedStatus(StatusState.Completed);
            return;
        }

        CurrentState.ChangedStatus += OnChangedStatus;
    }

    protected override void DeactivateAddon() 
    {
        CurrentState.ChangedStatus -= OnChangedStatus;
    }
    private void OnChangedStatus(StatusState status)
    {
        if (status != StatusState.Completed)
            return;

        SetNeedTransit();
    }
}
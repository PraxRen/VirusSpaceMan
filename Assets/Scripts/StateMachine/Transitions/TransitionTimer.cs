public class TransitionTimer : Transition
{
    private readonly float _timeTimer;
    private readonly Timer _timer;

    public TransitionTimer(Character character, State currentState, State targetState, float timeTimer) : base(character, currentState, targetState) 
    {
        _timeTimer = timeTimer;
        _timer = new Timer(timeTimer);
    }

    public override void Tick(float deltaTime)
    {
        _timer.Tick(deltaTime);
    }

    protected override void ActivateAddon()
    {
        _timer.Completed += OnCompleted;
    }

    protected override void DeactivateAddon()
    {
        _timer.Completed -= OnCompleted;
        _timer.SetTime(_timeTimer);
    }

    private void OnCompleted(Timer timer) => SetNeedTransit();
}
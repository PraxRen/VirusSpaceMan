public class TransitionReachToMoveTarget : Transition
{
    public TransitionReachToMoveTarget(Character character, State currentState, State targetState) : base(character, currentState, targetState) { }

    public override void Tick(float deltaTime)
    {
        if (Character.MoveTracker.Target.CanReach(Character.Transform) == false)
            return;

        SetNeedTransit();
    }
}
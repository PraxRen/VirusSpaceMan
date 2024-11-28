public class TransitionDistanceToMoveTarget : Transition
{
    public TransitionDistanceToMoveTarget(Character character, State currentState, State targetState) : base(character, currentState, targetState) { }

    public override void Tick(float deltaTime)
    {
        if (Status == StatusTransition.NeedTransit)
            return;

        if (Character.MoveTracker.Target.CanReach(Character.Transform) == false)
            return;

        SetNeedTransit();
    }
}
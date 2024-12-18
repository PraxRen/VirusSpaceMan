public class TransitionDistanceToMoveTarget : Transition
{
    private readonly float _distanceSqr;

    public TransitionDistanceToMoveTarget(Character character, State currentState, State targetState, float distance) : base(character, currentState, targetState)
    {
        _distanceSqr = distance * distance;
    }

    public override void Tick(float deltaTime)
    {
        if ((Character.MoveTracker.Target.Position - Character.Transform.position).sqrMagnitude > _distanceSqr)
            return;

        SetNeedTransit();
    }
}
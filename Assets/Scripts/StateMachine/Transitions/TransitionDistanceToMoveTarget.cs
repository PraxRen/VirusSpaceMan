public class TransitionDistanceToMoveTarget : Transition
{
    private readonly float _distanceSqr; 

    public TransitionDistanceToMoveTarget(Character character, State currentState, State targetState, float distance) : base(character, currentState, targetState)
    {
        if (distance < 0)
            throw new System.ArgumentOutOfRangeException(nameof(distance));

        _distanceSqr = distance * distance;
    }

    public override void Tick(float deltaTime)
    {
        if (Status == StatusTransition.NeedTransit)
            return;

        if ((Character.MoveTracker.Position - Character.Transform.position).sqrMagnitude < _distanceSqr) 
        {
            SetNeedTransit();
        }
    }
}
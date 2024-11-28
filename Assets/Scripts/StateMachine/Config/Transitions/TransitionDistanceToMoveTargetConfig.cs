using UnityEngine;

[CreateAssetMenu(fileName = "NewTransitionDistanceToMoveTargetConfig", menuName = "StateMachine/Transitions/TransitionDistanceToMoveTargetConfig")]
public class TransitionDistanceToMoveTargetConfig : TransitionConfig
{
    protected override Transition CreatTransitionAddon(Character character, State currentState, State targetState)
    {
        return new TransitionDistanceToMoveTarget(character, currentState, targetState);
    }
}
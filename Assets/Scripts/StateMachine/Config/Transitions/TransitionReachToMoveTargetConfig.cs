using UnityEngine;

[CreateAssetMenu(fileName = "NewTransitionReachToMoveTargetConfig", menuName = "StateMachine/Transitions/TransitionReachToMoveTargetConfig")]
public class TransitionReachToMoveTargetConfig : TransitionConfig
{
    protected override Transition CreatTransitionAddon(Character character, State currentState, State targetState)
    {
        return new TransitionReachToMoveTarget(character, currentState, targetState);
    }
}
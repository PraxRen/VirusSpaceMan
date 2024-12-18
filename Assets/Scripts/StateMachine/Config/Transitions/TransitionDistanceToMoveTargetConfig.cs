using UnityEngine;

[CreateAssetMenu(fileName = "NewTransitionDistanceToMoveTargetConfig", menuName = "StateMachine/Transitions/TransitionDistanceToMoveTargetConfig")]
public class TransitionDistanceToMoveTargetConfig : TransitionConfig
{
    [SerializeField] private float _distance;

    protected override Transition CreatTransitionAddon(Character character, State currentState, State targetState) => new TransitionDistanceToMoveTarget(character, currentState, targetState, _distance);
}
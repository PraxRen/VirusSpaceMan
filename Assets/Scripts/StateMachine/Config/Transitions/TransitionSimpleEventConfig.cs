using UnityEngine;

[CreateAssetMenu(fileName = "NewTransitionSimpleEventConfig", menuName = "StateMachine/Transitions/TransitionSimpleEventConfig")]
public class TransitionSimpleEventConfig : TransitionConfig
{
    [SerializeField] private TypeSimpleEvent _typeSimpleEvent;

    protected override Transition CreatTransitionAddon(Character character, State currentState, State targetState) => new TransitionSimpleEvent(character, currentState, targetState, _typeSimpleEvent);
}

using UnityEngine;

[CreateAssetMenu(fileName = "NewTransitionStateCompleteConfig", menuName = "StateMachine/Transitions/TransitionStateCompleteConfig")]
public class TransitionStateCompleteConfig : TransitionConfig
{
    protected override Transition CreatTransitionAddon(Character character, State currentState, State targetState)
    {
        return new TransitionStateComplete(character, currentState, targetState);
    }
}

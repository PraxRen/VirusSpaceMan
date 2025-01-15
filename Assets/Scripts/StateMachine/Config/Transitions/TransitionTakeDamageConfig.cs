using UnityEngine;

[CreateAssetMenu(fileName = "NewTransitionTakeDamageConfig", menuName = "StateMachine/Transitions/TransitionTakeDamageConfig")]
public class TransitionTakeDamageConfig : TransitionConfig
{
    protected override Transition CreatTransitionAddon(Character character, State currentState, State targetState) => new TransitionTakeDamage(character, currentState, targetState);
}
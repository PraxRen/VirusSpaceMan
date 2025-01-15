using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "NewTransitionTriggerConfig", menuName = "StateMachine/Transitions/TransitionTriggerConfig")]
public class TransitionTriggerConfig : TransitionConfig
{
    protected override Transition CreatTransitionAddon(Character character, State currentState, State targetState) => new TransitionTrigger(character, currentState, targetState);
}
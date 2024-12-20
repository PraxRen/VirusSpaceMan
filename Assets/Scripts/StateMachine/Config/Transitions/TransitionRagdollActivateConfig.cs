using UnityEngine;

[CreateAssetMenu(fileName = "NewTransitionRagdollActivateConfig", menuName = "StateMachine/Transitions/TransitionRagdollActivateConfig")]
public class TransitionRagdollActivateConfig : TransitionConfig
{
    protected override Transition CreatTransitionAddon(Character character, State currentState, State targetState) => new TransitionRagdollActivate(character, currentState, targetState);
}
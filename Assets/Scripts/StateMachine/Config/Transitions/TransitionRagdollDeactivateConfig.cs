using UnityEngine;

[CreateAssetMenu(fileName = "NewTransitionRagdollDeactivateConfig", menuName = "StateMachine/Transitions/TransitionRagdollDeactivateConfig")]
public class TransitionRagdollDeactivateConfig : TransitionConfig
{
    protected override Transition CreatTransitionAddon(Character character, State currentState, State targetState) => new TransitionRagdollDeactivate(character, currentState, targetState);
}
using UnityEngine;

[CreateAssetMenu(fileName = "NewTransitionScannerDamageableRemoveTargetConfig", menuName = "StateMachine/Transitions/TransitionScannerDamageableRemoveTargetConfig")]
public class TransitionScannerDamageableRemoveTargetConfig : TransitionConfig
{
    protected override Transition CreatTransitionAddon(Character character, State currentState, State targetState) => new TransitionScannerDamageableRemoveTarget(character, currentState, targetState);
}
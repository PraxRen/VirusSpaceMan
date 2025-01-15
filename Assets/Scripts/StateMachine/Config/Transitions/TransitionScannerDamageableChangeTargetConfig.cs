using UnityEngine;

[CreateAssetMenu(fileName = "NewTransitionScannerDamageableChangeTargetConfig", menuName = "StateMachine/Transitions/TransitionScannerDamageableChangeTargetConfig")]
public class TransitionScannerDamageableChangeTargetConfig : TransitionConfig
{
    protected override Transition CreatTransitionAddon(Character character, State currentState, State targetState) => new TransitionScannerDamageableChangeTarget(character, currentState, targetState);
}
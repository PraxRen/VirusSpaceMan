using UnityEngine;

[CreateAssetMenu(fileName = "NewStateInteractionConfig", menuName = "StateMachine/States/StateInteractionConfig")]
public class StateInteractionConfig : StateConfig
{
    public override State CreateState(AICharacter character) => new StateInteraction(Id, character, TimeSecondsWaitUpdate);
}